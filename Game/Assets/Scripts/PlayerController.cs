using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] GameObject slashGO;
    public float moveSpeed = 5f;

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
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
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
            var damageAmmount = 10f; //TODO Enemy damage varies?
            hp = (hp - damageAmmount) <= 0f ? 0f : hp - damageAmmount;
            uiManager.SetHp(hp);
            timeSinceLastDamaged = 0f;
        }
    }

    private void StartSlash()
    {
        if (attackOnCooldown)
        {
            return;
        }
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
        if (currentExp > expToLevel)
        {
            expPct = LevelUp();
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
        attackDamage = 100;
        uiManager.LevelUp();
        return currentExp / expToLevel;
    }
    public void LevelUpAtk()
    {
        attackDamage *= 1.1f;
    }
    public void LevelUpAtkSpd()
    {
        attackDamage *= .85f;
    }
    public void LevelUpRange()
    {
        slashGO.transform.localScale = Vector3.Scale(slashGO.transform.localScale, new Vector3(1.1f, 1.1f, 1f));
    }
    public void LevelUpHp()
    {
        var oldMaxHp = maxHp;
        maxHp *= 1.1f;
        maxHp = MathF.Floor(maxHp);
        hp += (oldMaxHp-maxHp) + 30f;
        uiManager.SetHp(hp, maxHp);
    }

    public float GetDamage()
    {
        return attackDamage;
    }
}

