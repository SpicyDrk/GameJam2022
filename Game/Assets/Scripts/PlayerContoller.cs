using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    Vector2 movement;
    public GameObject player;
    public GameObject slashHitBox;
    private Animator anim;

    private bool slashing;
    public float attackDuration = 0.2f;
    public float swingTimer = 1.5f;
    private float timeSinceLastAttack = 0f;

    void Start()
    {
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

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        anim.SetBool("MovingRight", movement.x == 1f);
        anim.SetBool("MovingLeft", movement.x == -1f);
        anim.SetBool("MovingUp", movement.y == 1f);
        anim.SetBool("MovingDown", movement.y == -1f);
    }

    private void StartSlash()
    {
        if (slashing || timeSinceLastAttack > swingTimer)
        {
            return;
        }
        slashing = true;
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        slashHitBox.transform.position = player.gameObject.transform.position + (rotation * (Vector3.right));
        slashHitBox.transform.rotation = rotation;
        slashHitBox.SetActive(true);
    }

    private void CheckSlash()
    {
        if (timeSinceLastAttack > swingTimer)
        {
            timeSinceLastAttack = 0f;
        }
        if (!slashing)
        {
            return;
        }
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > attackDuration)
        {
            slashing = false;
            slashHitBox.SetActive(false);
        }
    }
}

