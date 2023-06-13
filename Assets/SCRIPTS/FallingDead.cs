using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDead : MonoBehaviour
{
    GameObject player;
    public GameObject checkpt;
    void Start()
    {
        player = GameObject.Find("Player");
        //checkpt = GameObject.Find("Checkpoint");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            player.GetComponent<Transform>().position = MOVEMENT.checkpointPos;
            Debug.Log(this.gameObject.GetComponent<MonoBehaviour>());
        }
    }
}