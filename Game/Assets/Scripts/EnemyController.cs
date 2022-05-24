using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private float timeSinceLastHit = 100f;
    private bool moving = true;
    private Rigidbody2D rb;
    private bool dying;
    private Animator anim;
    private CircleCollider2D cCollider;
    private GameObject player;

    private float timeToLiveAfterDeath = 3f;

    public float knockbackDistance = 10f;
    public float hitboxCooldown = 0.3f;

    [SerializeField] 
    public float startingHp = 100f;
    public float maxSpeed = 3f;

    [SerializeField] GameObject healthBar, healthBarBackground;
    private float hp;

    private float minX, maxX, minY, maxY, extendedBounds = 3f;


    // Start is called before the first frame update
    void Start()
    {
        SetScreenBounds();
        player = GameObject.FindGameObjectWithTag("Player");
        hp = startingHp;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cCollider = GetComponent<CircleCollider2D>();
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
                StartDeath();
            }
            UpdateHealthBar();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "enemy" && !dying)
        {
            transform.position = Vector3.MoveTowards(transform.position, col.transform.position, -.01f);
        }
    }

    private void StartDeath()
    {
        dying = true;
        anim.SetBool("IsDying", true); 
        moving = false;
        rb.simulated = false;
        cCollider.enabled = false;
        healthBar.SetActive(false);
        healthBarBackground.SetActive(false);
        sr.material.SetFloat("_FlashAmount", 0f);
    }

    void Update()
    {
        if (dying)
        {
            timeToLiveAfterDeath -= Time.deltaTime;
            if (timeToLiveAfterDeath <= 0f)
            {
                gameObject.SetActive(false);
            }
            if (timeToLiveAfterDeath < 1f)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, timeToLiveAfterDeath / 1f);
            }
            return;
        }

        // for fixing Z plane issues
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;

        ShaderFader();
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, maxSpeed * Time.deltaTime);
            if (player.transform.position.x < transform.position.x)
            {
                sr.flipX = true;
            } else
            {
                sr.flipX = false;
            }
        }
        timeSinceLastHit += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (dying)
            return;
        
        if (transform.position.x - player.transform.position.x  < minX)
        {
            var randomyY = UnityEngine.Random.Range(minY + transform.position.y + 1f, maxY + transform.position.y - 1f);
            transform.position = new Vector2(maxX + player.transform.position.x -1f, randomyY);
        }
        if (transform.position.x - player.transform.position.x > maxX)
        {
            var randomyY = UnityEngine.Random.Range(minY + transform.position.y + 1f, maxY + transform.position.y-1f);
            transform.position = new Vector2(minX + player.transform.position.x + 1f, randomyY);
        }
        if (transform.position.y - player.transform.position.y < minY)
        {
            var randomX = UnityEngine.Random.Range(minX + transform.position.x + 1f, maxX + transform.position.x - 1f);
            transform.position = new Vector2(randomX, maxY + player.transform.position.y - 1f);
        }
        if (transform.position.y - player.transform.position.y > maxY)
        {
            var randomX = UnityEngine.Random.Range(minX + transform.position.x + 1f, maxX + transform.position.x - 1f);
            transform.position = new Vector2(randomX, minY + player.transform.position.y + 1f);
        }
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
        var scale = hp < 0 ? 0f : hp / startingHp;
        healthBar.transform.localScale = new Vector3(scale , healthBar.transform.localScale.z, healthBar.transform.localScale.z);
    }

    void SetScreenBounds()
    {
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        maxX = bounds.x + extendedBounds;
        minX = -bounds.x - extendedBounds;
        minY = -bounds.y - extendedBounds;
        maxY = bounds.y + extendedBounds;
    }
}
