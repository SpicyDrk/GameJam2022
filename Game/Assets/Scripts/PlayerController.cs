using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] SoundManager soundManager;
    [SerializeField] GameObject slashGO, attackBox;
    public float moveSpeed = 5f;
    public EnemySpawner enemySpawner;

    public Rigidbody2D rb;
    Vector2 movement;
    public GameObject player;
    public GameObject slashHitBox;
    [SerializeField] GameObject swingTimerGO;
    private Animator anim;

    private float timeSinceLastDamaged = 10f;
    private float playerHitCooldown = 1f; // invulnerability time after getting hit

    private float timeSinceLastAttack = 10f;
    private bool attackOnCooldown = false;
    private AudioSource audioSource;
    [SerializeField] AudioClip meow;

    public bool playerIsDead = false;

    public float attackDuration = 0.1f;
    public float swingTimer = 2f;    
    public float attackDamage = 10f;

    private float maxHp;
    public float hp = 100f;

    private float currentExp = 0f;
    private float expToLevel = 100f;
    private float expScaling = 1.1f; 

    void Start()
    {
        maxHp = hp;
        uiManager = FindObjectOfType(typeof(UIManager)) as UIManager;
        soundManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
        enemySpawner = FindObjectOfType(typeof(EnemySpawner)) as EnemySpawner;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); 
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        UpdateAttackBox();
        if (Input.GetMouseButton(0))
        {
            StartSlash();
        }
        CheckSlash();
        timeSinceLastDamaged += Time.deltaTime;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement.normalized);
        anim.SetBool("MovingRight", movement.x == 1f);
        anim.SetBool("MovingLeft", movement.x == -1f);
        anim.SetBool("MovingUp", movement.y == 1f);
        anim.SetBool("MovingDown", movement.y == -1f);
    }

    private void LateUpdate()
    {
        UpdateSwingTimer();
        
    }

    private void OnTriggerStay2D(Collider2D target)
    {
        if (target.gameObject.CompareTag("Enemy") && timeSinceLastDamaged > playerHitCooldown)
        {
            var damageAmmount = 10f;
            hp = (hp - damageAmmount) <= 0f ? 0f : hp - damageAmmount;
            uiManager.SetHp(hp);
            timeSinceLastDamaged = 0f;
            audioSource.pitch = Random.Range(0.8f, 1.4f);
            audioSource.PlayOneShot(meow);

            if (hp == 0){
                Death();
            }
        }
    }

    private void Death()
    {
        playerIsDead = true;
        enabled = false;
        enemySpawner.enabled = false;
        soundManager.PlayDeathMusic();
        uiManager.PlayerDied();
    }

    private void UpdateAttackBox()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        attackBox.transform.position = player.gameObject.transform.position + (rotation * (Vector3.right));
        attackBox.transform.rotation = rotation;
    }

    private void StartSlash()
    {
        if (attackOnCooldown)
        {
            return;
        }
        soundManager.PlayerAttackSound();
        attackOnCooldown = true;
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        slashHitBox.transform.position = player.gameObject.transform.position + (rotation * (Vector3.right));
        slashHitBox.transform.rotation = rotation;
        timeSinceLastAttack = 0f;
        slashHitBox.SetActive(true);
    }

    private void CheckSlash()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > swingTimer)
        {
            attackOnCooldown = false;
        }
        if (timeSinceLastAttack > attackDuration)
        {
            slashHitBox.SetActive(false);
        }
    }

    private void UpdateSwingTimer()
    {
        if (attackOnCooldown)
        {
            swingTimerGO.transform.localScale = new Vector3(timeSinceLastAttack / swingTimer, 1f, 1f);
        }
    }
    public void GainExp(float expGained)
    {
        currentExp += expGained;
        float expPct;
        if (currentExp >= expToLevel)
        {
            expPct = LevelUp();
            soundManager.PlayLevelUpSound();
        }  
        else
        {
            expPct = currentExp / expToLevel;
        }
        uiManager.SetExpAsPercent(expPct);
    }

    /*
     * levels up and returns remaining exp;.
     */
    private float  LevelUp()
    {
        currentExp -= expToLevel;
        expToLevel *= expScaling;
        uiManager.LevelUp();
        return currentExp / expToLevel;
    }
    public void LevelUpAtk()
    {
        attackDamage *= 1.15f;
    }
    public void LevelUpAtkSpd()
    {
        swingTimer *= .85f;
    }
    public void LevelUpRange()
    {
        slashGO.transform.localScale = Vector3.Scale(slashGO.transform.localScale, new Vector3(1.1f, 1.1f, 1f));
        attackBox.transform.localScale = Vector3.Scale(attackBox.transform.localScale, new Vector3(1.1f, 1.1f, 1f));
    }
    public void LevelUpHp()
    {
        var oldMaxHp = maxHp;
        maxHp *= 1.1f;
        maxHp = MathF.Floor(maxHp);
        hp += (oldMaxHp-maxHp) + 0.3f*maxHp;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        uiManager.SetHp(hp, maxHp);
    }

    public float GetDamage()
    {
        return attackDamage;
    }
}

