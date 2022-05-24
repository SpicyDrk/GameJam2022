using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

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
    public float hp = 100f;

    void Start()
    {
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
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
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag("Enemy") && timeSinceLastDamaged > playerHitCooldown)
        {
            var damageAmmount = 10f;
            hp = (hp - damageAmmount) <= 0f ? 0f : hp - damageAmmount;
            gameManager.SetHp(hp);
        }
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
}

