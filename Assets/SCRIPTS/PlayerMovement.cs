using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public float move;
    public float speed = 12f;
    public bool facingRight = true;

    public bool jumptr = false;
    public LayerMask whatISGround;
    public Transform groundCheck;
    public float checkRadius = 0.5f;
    public bool grounded = false;
    public float jumpForce = 9f;

    public bool Zpressed = false;
    public float time = 8f;
    public bool ZJump = true;
    public bool jumptrZ = false;
    public bool ZMoveInput = false;

    public Animator anim;
    GameObject wall;
    private bool timeAnim = false;

    public GameObject wal; //

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        wall = GameObject.Find("WallJumpCollider");
        Physics2D.gravity = new Vector2(0, -13);
    }
    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal") * speed;
        if (Input.GetKeyDown(KeyCode.Z) && ZJump) Zpressed = true;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumptrZ = true;
            if (grounded)
                jumptr = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            ZMoveInput = true;
       
    }
    void FixedUpdate()
    {
        //move
        rb.velocity = new Vector2(move, rb.velocity.y);
        anim.SetFloat("move", Mathf.Abs(move));

        //flip
        if (facingRight == true && move < 0)
            Flip();
        else if (facingRight == false && move > 0)
            Flip();

        //sprint
        /*if (Input.GetKey(KeyCode.LeftShift))
            speed = 10f;
        else
            speed = 8f;*/

        //jump
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatISGround);
        if (jumptr && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumptr = false;
            jumptrZ = false;
        }
        anim.SetBool("grounded", grounded);
        if (rb.velocity.y < 0)
            anim.SetBool("isFalling", true);
        else anim.SetBool("isFalling", false);

        //z
        if (Zpressed && ZJump == true)
        {
            time = 5f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            ZJump = false;
            Zpressed = false;
        }
        anim.SetBool("ZJump", ZJump);
        if (rb.constraints == RigidbodyConstraints2D.FreezeAll)
            time -= Time.deltaTime;
        if (time <= 0 || ZMoveInput || jumptrZ || Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (jumptrZ && time > 0)
            {
                rb.velocity = new Vector2(0f, jumpForce);
                jumptrZ = false;
            }
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            ZMoveInput = false;
            time = -1;
        }
        if (grounded == true || wall.GetComponent<WallJump>().jumpFL || wal.GetComponent<WallsScriptRight>().jFR)
            ZJump = true;
        anim.SetFloat("time", time);
        if (time > 0 && !timeAnim)
        {
            anim.SetBool("timeAnim", true);
            timeAnim = true;
        }
        else if (time <= 0 && timeAnim)
        {
            anim.SetBool("timeAnim", false);
            timeAnim = false;
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
