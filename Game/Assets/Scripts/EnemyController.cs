using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private float timeSinceLastHit = 100f;
    private bool moving = true;
    private Rigidbody2D rb;

    public float knockbackDistance = 10f;
    public float hitboxCooldown = 0.3f;

    [SerializeField]
    GameObject player;
    public float startingHp = 100f;
    public float maxSpeed = 3f;

    [SerializeField]
    GameObject healthBar;
    private float hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = startingHp;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Slash" && timeSinceLastHit > hitboxCooldown)
        {
            sr.material.SetFloat("_FlashAmount", 1f);
            timeSinceLastHit = 0f;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, maxSpeed * Time.deltaTime * knockbackDistance * -1f);
            hp -= 20f;
            if (hp<= 0f)
            {
                this.gameObject.SetActive(false);
            }
            UpdateHealthBar();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "enemy")
        {
            transform.position = Vector3.MoveTowards(transform.position, col.transform.position, -.05f);
        }
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
        ShaderFader();
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, maxSpeed * Time.deltaTime);
            if (player.transform.position.x < transform.position.x)
            {
                sr.flipX = true;// transform.localScale = new Vector3(-1f, 1f, 1f);
            } else
            {
                sr.flipX = false;// transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        timeSinceLastHit += Time.deltaTime;
    }
    void ShaderFader()
    {
        var flashAmount = sr.material.GetFloat("_FlashAmount");
        if (flashAmount > 0f)
        {
            sr.material.SetFloat("_FlashAmount", flashAmount - Time.deltaTime * 2f);
        }
        if (flashAmount < 0f)
        {
            sr.material.SetFloat("_FlashAmount", 0f);
        }
    }

    void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3(hp / startingHp, healthBar.transform.localScale.z, healthBar.transform.localScale.z);
    }


}
