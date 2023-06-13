using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsScriptLeft : MonoBehaviour
{
    public GameObject wal;
    GameObject playerAnim;
    void Start()
    {
        wal = GameObject.Find("WallJumpCollider");
        playerAnim = GameObject.Find("Player");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.collider.GetType() == typeof(CapsuleCollider2D))
        {
            wal.GetComponent<WallJump>().jumpFL = true;
            if (playerAnim.GetComponent<PlayerMovement>().facingRight == false) playerAnim.GetComponent<PlayerMovement>().anim.SetBool("slide", true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.collider.GetType() == typeof(CapsuleCollider2D))
        {
            wal.GetComponent<WallJump>().jumpFL = false;
            playerAnim.GetComponent<PlayerMovement>().anim.SetBool("slide", false);
        }
    }
}
