using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsScriptRight : MonoBehaviour
{
    public GameObject wal; //
    GameObject playerAnim;
    public LayerMask character;

    public bool jFR = false;
    void Start()
    {
        wal = GameObject.Find("WallJumpCollider"); //
        playerAnim = GameObject.Find("Player");
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.collider.GetType() == typeof(CapsuleCollider2D))
        {
            wal.GetComponent<WallJump>().jumpFR = true;
            if (playerAnim.GetComponent<PlayerMovement>().facingRight) playerAnim.GetComponent<PlayerMovement>().anim.SetBool("slide", true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.collider.GetType() == typeof(CapsuleCollider2D))
        {
            wal.GetComponent<WallJump>().jumpFR = false;
            playerAnim.GetComponent<PlayerMovement>().anim.SetBool("slide", false);
        }
    }*/
    private void FixedUpdate()
    {
        //wal.GetComponent<WallJump>().jumpFR = Physics2D.OverlapArea(new Vector2(-251.12f, 18.65f), new Vector2(-246.45f, -12.21f), character);
        jFR = Physics2D.OverlapArea(new Vector2(-251.12f, 18.65f), new Vector2(-246.45f, -12.21f), character);
    }
}
