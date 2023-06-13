using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    GameObject playerJ;
    public GameObject wal;
    public bool jumpFR = false;
    public bool jumpFL = false;

    public bool LeftArr = false;
    public bool UpArr = false;
    void Start()
    {
        playerJ = GameObject.Find("Player");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            LeftArr = true;
        if (Input.GetKey(KeyCode.UpArrow))
            UpArr = true;
    }
    void FixedUpdate()
    {
        if (UpArr && wal.GetComponent<WallsScriptRight>().jFR)
        {
            if(LeftArr) //playerJ.GetComponent<PlayerMovement>().move < 0
            {
                playerJ.GetComponent<PlayerMovement>().rb.velocity = new Vector2(playerJ.GetComponent<PlayerMovement>().rb.velocity.x, playerJ.GetComponent<PlayerMovement>().jumpForce);
                LeftArr = false;
                UpArr = false;
            }
        }
        if (Input.GetKey(KeyCode.UpArrow) && jumpFL)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) //playerJ.GetComponent<PlayerMovement>().move > 0
                playerJ.GetComponent<PlayerMovement>().rb.velocity = new Vector2(playerJ.GetComponent<PlayerMovement>().move, playerJ.GetComponent<PlayerMovement>().jumpForce);
        }
    }
}
