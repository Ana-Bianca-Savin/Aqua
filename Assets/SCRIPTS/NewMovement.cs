﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovement : MonoBehaviour
{
    [Header("Move")]
    public Rigidbody2D rb;
    public Vector2 checkpointPos;

    public float horizontal;
    public float vertical;
    public float speed;
    public bool facingRight;

    public bool leftArr;
    public bool rightArr;

    [Header("Jump")]
    public LayerMask ground;
    public bool upArrJump;
    public bool grounded = false;
    public float groundedSkin = 0.05f;
    public float jumpForce;
    public bool holdUpArr;
    public bool jumpNow;

    public Vector2 playerSize;
    public Vector2 boxSize;
    public Vector2 boxOffset;

    [Header("Better Jump")]
    public float smallJump;
    public float fallJump;
    public bool slowJump;
    public float slowFallJump;
    public float fall;
    public float fallSpeed;
    public float gravityScale = 1f;

    [Header("Wall Jump")]
    public LayerMask wall;
    public Vector2 boxSizeWall;
    public bool canMove;
    public float moveTime;
    public bool onRightWall;
    public bool onLeftWall;
    public bool upArrOnWall;
    public float wallJumpPush;
    public float wallJumpForce;

    public bool wallSlide;
    public float slideDown;

    public float wallJumpLerp;

    [Header("Special Jump")]
    public bool wallGround;
    public bool specialJump;

    public bool wallGroundJump;
    public bool specialWallJump;
    public bool upArrSpecial;

    [Header("Special Move")]
    public float time;
    public bool ZPressed;
    public bool specialTr;
    public bool wallJumpTr;
    public bool ZPressedDown;

    public bool RightArrPressed;
    public bool RightArrPressedDown;
    public bool LeftArrPressed;
    public bool LeftArrPressedDown;
    public bool UpArrPressed;
    public bool UpArrPressedDown;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<BoxCollider2D>().size;
        boxOffset = GetComponent<BoxCollider2D>().offset;
        boxSize = new Vector2(playerSize.x, groundedSkin);
        boxSizeWall = new Vector2(groundedSkin, playerSize.y);
        facingRight = true;
        canMove = true;
        wallSlide = false;
        //
        /*slowJump = false;
        jumpNowDouble = false;
        speed = 8f;
        jumpForce = 9f;
        smallJump = 6f;
        fallJump = 4f;
        slowFallJump = 4f;
        fall = 4f;
        fallSpeed = 20f;*/

        checkpointPos = transform.position;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        SmootherMovement();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (grounded)
                upArrJump = true;
            if (!upArrJump)
                upArrSpecial = true;
        }

        holdUpArr = Input.GetKey(KeyCode.UpArrow);
        leftArr = Input.GetKey(KeyCode.LeftArrow);
        rightArr = Input.GetKey(KeyCode.RightArrow);

        if (Input.GetKeyDown(KeyCode.UpArrow) && (onRightWall || onLeftWall) && !grounded && wallSlide)
            upArrOnWall = true;

        LeftArrPressed = Input.GetKey(KeyCode.LeftArrow);
        RightArrPressed = Input.GetKey(KeyCode.RightArrow);
        UpArrPressed = Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            RightArrPressedDown = true;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            LeftArrPressedDown = true;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            UpArrPressedDown = true;
        if (Input.GetKey(KeyCode.Z))
            ZPressed = true;
        if (Input.GetKeyDown(KeyCode.Z) && specialTr && (onLeftWall || onRightWall) && grounded)
            ZPressedDown = true;
    }

    private void FixedUpdate()
    {
        Move();

        Vector2 boxCenter = (Vector2)transform.position + GetComponent<BoxCollider2D>().offset + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
        grounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, ground);

        if (grounded)
        {
            upArrSpecial = false; //
        }

        Jump();

        if (canMove && !wallSlide)
        {
            if (facingRight && horizontal < 0)
                Flip();
            else if (!facingRight && horizontal > 0)
                Flip();
        }

        Vector2 boxCenterWallLeft = new Vector2(transform.position.x - (playerSize.x + boxSizeWall.x) * 0.5f
                + GetComponent<BoxCollider2D>().offset.x, transform.position.y + GetComponent<BoxCollider2D>().offset.y);
        Vector2 boxCenterWallRight = new Vector2(transform.position.x + (playerSize.x + boxSizeWall.x) * 0.5f
                + GetComponent<BoxCollider2D>().offset.x, transform.position.y + GetComponent<BoxCollider2D>().offset.y);
        onRightWall = Physics2D.OverlapBox(boxCenterWallRight, boxSizeWall, 0f, wall);
        onLeftWall = Physics2D.OverlapBox(boxCenterWallLeft, boxSizeWall, 0f, wall);
        SpecialWallJump();
        WallJump();
        SpecialJump();
        WallSlide();
        SpecialJump();
        SpecialMove();
    }

    //My methods
    ///
    ///
    //My methods
    void Move()
    {
        if (!canMove)
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(horizontal * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        else if (wallSlide)
            rb.velocity = new Vector2(rb.velocity.x, -slideDown);
        else if (rightArr && leftArr)
            rb.velocity = new Vector2(0f, rb.velocity.y);
        else if (!grounded)
            rb.velocity = new Vector2(horizontal * speed * 0.95f, rb.velocity.y);
        else
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void SmootherMovement()
    {
        if (leftArr && horizontal > 0f)
            horizontal = 0;
        else if (rightArr && horizontal < 0f)
            horizontal = 0f;
    }

    void Flip()
    {
        facingRight = !facingRight;
        if (!GetComponent<SpriteRenderer>().flipX)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (GetComponent<SpriteRenderer>().flipX)
            GetComponent<SpriteRenderer>().flipX = false;
    }

    void Jump()
    {
        if (grounded && !upArrJump)
            jumpNow = false;

        if (upArrJump)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpNow = true;
            upArrJump = false;
        }

        if (rb.velocity.y < 0f && !jumpNow)
            rb.gravityScale = fall;
        else if (!slowJump)
        {
            if (!holdUpArr && jumpNow)
            {
                rb.gravityScale = smallJump;
            }
        }
        else if (holdUpArr && rb.velocity.y > 0f && jumpNow)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.velocity.y < 0f && jumpNow && holdUpArr)
        {
            rb.gravityScale = slowFallJump;
        }

        if (vertical > 0.45f)
            vertical = 1;
        if (vertical == 1f)
            slowJump = true;
        if (grounded)
        {
            rb.gravityScale = gravityScale;
            slowJump = false;
        }
        if (rb.velocity.y < -fallSpeed)
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -fallSpeed, Mathf.Infinity));
    }


    IEnumerator CanMove(float moveTime)
    {
        canMove = false;
        yield return new WaitForSeconds(moveTime);
        canMove = true;
    }

    void WallJump()
    {
        if (onRightWall && !upArrOnWall || onLeftWall && !upArrOnWall)
            jumpNow = false;
        if (upArrOnWall && onRightWall && !grounded || onRightWall && specialWallJump && upArrSpecial)
        {
            upArrSpecial = false;
            jumpNow = true;
            upArrOnWall = false;
            StopCoroutine(CanMove(0f));
            StartCoroutine(CanMove(moveTime));
            rb.velocity = new Vector2(wallJumpForce * -1, jumpForce);
            Flip();
        }
        else if (upArrOnWall && onLeftWall && !grounded || onLeftWall && specialWallJump && upArrSpecial)
        {
            upArrSpecial = false;
            jumpNow = true;
            upArrOnWall = false;
            StopCoroutine(CanMove(0f));
            StartCoroutine(CanMove(moveTime));
            rb.velocity = new Vector2(wallJumpForce, jumpForce);
            Flip();
        }
        wallJumpTr = true;
    }

    void WallSlide()
    {
        if (onLeftWall && horizontal < 0f && rb.velocity.y > 0f || onRightWall && horizontal > 0f && rb.velocity.y > 0f)
            wallSlide = false;
        else if (onLeftWall && horizontal > 0.1f)
            wallSlide = false;
        else if (onRightWall && horizontal < -0.1f)
            wallSlide = false;
        else if (!onRightWall && !onLeftWall)
            wallSlide = false;
        else if (grounded)
            wallSlide = false;
        else if (specialJump)
            wallSlide = false;
        else if (onLeftWall && horizontal < 0f || onRightWall && horizontal > 0f)
            wallSlide = true;
    }

    void SpecialJump()
    {
        if (grounded && (onRightWall && rightArr || onLeftWall && leftArr))
            wallGround = true;
        if (wallGround && holdUpArr && rb.velocity.y > 0f)
            specialJump = true;
        else
            specialJump = false;
        if (!rightArr && !leftArr)
            wallGround = false;
    }

    void SpecialWallJump()
    {
        if (grounded && (onRightWall || onLeftWall))
            wallGroundJump = true;
        if (wallGroundJump && holdUpArr)
            specialWallJump = true;
        else
            specialWallJump = false;
        if (!onRightWall && !onLeftWall)
            wallGroundJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            wallSlide = true;
        }
    }

    void SpecialMove()
    {
        if (ZPressedDown)
        {
            time = 3f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            specialTr = false;
            UpArrPressedDown = false;
            ZPressedDown = false;
        }
        if (rb.constraints == RigidbodyConstraints2D.FreezeAll)
            time -= Time.deltaTime;
        if (time <= 0 || UpArrPressedDown)
        {
            if (UpArrPressed && time > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                /*if (RightArrPressed || LeftArrPressed)
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);*/
            }
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            time = -1;
            UpArrPressedDown = false;
        }
        if (!grounded || (!onRightWall && !onLeftWall))
            specialTr = true;
    }

    void OnDrawGizmos()
    {
        //Walls Check
        Color meallow = new Color(0.97f, 0.87f, 0.49f, 1f);
        Gizmos.color = meallow;

        //Right Wall Check
        Vector3 R1 = new Vector3(transform.position.x + playerSize.x / 2f, transform.position.y + playerSize.y / 2f, 0f);
        Vector3 R2 = new Vector3(transform.position.x + playerSize.x / 2f + boxSizeWall.x, transform.position.y + playerSize.y / 2f, 0f);
        Vector3 R3 = new Vector3(transform.position.x + playerSize.x / 2f + boxSizeWall.x, transform.position.y - playerSize.y / 2f, 0f);
        Vector3 R4 = new Vector3(transform.position.x + playerSize.x / 2f, transform.position.y - playerSize.y / 2f, 0f);

        Gizmos.DrawLine(R1, R2);
        Gizmos.DrawLine(R2, R3);
        Gizmos.DrawLine(R3, R4);
        Gizmos.DrawLine(R4, R1);

        //Left Wall Check
        Vector3 L1 = new Vector3(transform.position.x - playerSize.x / 2f, transform.position.y + playerSize.y / 2f, 0f);
        Vector3 L2 = new Vector3(transform.position.x - playerSize.x / 2f - boxSizeWall.x, transform.position.y + playerSize.y / 2f, 0f);
        Vector3 L3 = new Vector3(transform.position.x - playerSize.x / 2f - boxSizeWall.x, transform.position.y - playerSize.y / 2f, 0f);
        Vector3 L4 = new Vector3(transform.position.x - playerSize.x / 2f, transform.position.y - playerSize.y / 2f, 0f);

        Gizmos.DrawLine(L1, L2);
        Gizmos.DrawLine(L2, L3);
        Gizmos.DrawLine(L3, L4);
        Gizmos.DrawLine(L4, L1);

        //Grounded Check
        Color jade = new Color(0f, 0.65f, 0.419f, 1f);
        Gizmos.color = jade;

        Vector3 G1 = new Vector3(transform.position.x - playerSize.x / 2f, transform.position.y - playerSize.y / 2f, 0f);
        Vector3 G2 = new Vector3(transform.position.x + playerSize.x / 2f, transform.position.y - playerSize.y / 2f, 0f);
        Vector3 G3 = new Vector3(transform.position.x + playerSize.x / 2f, transform.position.y - playerSize.y / 2f - boxSize.y, 0f);
        Vector3 G4 = new Vector3(transform.position.x - playerSize.x / 2f, transform.position.y - playerSize.y / 2f - boxSize.y, 0f);

        Gizmos.DrawLine(G1, G2);
        Gizmos.DrawLine(G2, G3);
        Gizmos.DrawLine(G3, G4);
        Gizmos.DrawLine(G4, G1);
    }
}
