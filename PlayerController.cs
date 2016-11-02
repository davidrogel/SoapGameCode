using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float movForce;
    [SerializeField]
    private float jumpForce;

    private float horInput;
    private bool jumpInput;

    [SerializeField]
    private Transform[] points;
    private bool onGround;
    [SerializeField]
    private float distance;
    [SerializeField]
    private LayerMask ground;

    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField]
    private CircleCollider2D[] colls;

    private bool grande;

	void Start ()
    {
        grande = true;
        onGround = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();        
    }
	
	void Update ()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        
        onGround = CheckForGround();
    }

    void FixedUpdate()
    {
        Movement();
        Jump();
    }

    void Movement()
    {
        rb.AddForce(Vector2.right * horInput * movForce * Time.fixedDeltaTime);
    }

    void Jump()
    {
        if (jumpInput && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
        }
    }

    bool CheckForGround()
    {        
        foreach(Transform point in points)
        {
            RaycastHit2D[] ray = Physics2D.RaycastAll(point.position, Vector2.down, distance, ground);

            for (int i = 0; i < ray.Length; i++)
            {
                if (ray[i].collider.gameObject != gameObject)
                {
                    return true;
                }                
            }
        }

        return false;
    }

    void Decrementux()
    {
        for(int i = 0; i < colls.Length; i++)
        {
            colls[i].radius -= .66f;
        }
        colls[0].offset = new Vector2(-1.46f, 0);
        colls[1].offset = new Vector2(1.38f, 0);
        grande = false;
    }

    void Incrementux()
    {
        for (int i = 0; i < colls.Length; i++)
        {                       
            colls[i].radius += .66f;
        }
        colls[0].offset = new Vector2(-2.02f, 0);
        colls[1].offset = new Vector2(2.22f, 0);
        grande = true;
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.gameObject.tag == "DecrementPWUP" && grande)
        {
            anim.SetBool("increment", false);
            trig.gameObject.SetActive(false);
            anim.SetBool("decrement", true);
            Decrementux();
        }

        if(trig.gameObject.tag == "IncrementPWUP" && !grande)
        {
            anim.SetBool("decrement", false);
            trig.gameObject.SetActive(false);
            anim.SetBool("increment", true);
            Incrementux();
        }        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "BigBox" && grande)
        {
            col.gameObject.GetComponent<Rigidbody2D>().constraints = 0;
            col.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
