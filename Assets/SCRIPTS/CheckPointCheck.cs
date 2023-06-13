using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointCheck : MonoBehaviour
{
    public GameObject player;
    public LayerMask playerLayer;
    public bool inArea;
    public float width;
    public float height;

    private void Update()
    {
        Vector2 pointA = new Vector2(transform.position.x + player.GetComponent<MOVEMENT>().playerSize.x * width * 0.5f,
                                    transform.position.y + player.GetComponent<MOVEMENT>().playerSize.y * height * 0.5f);
        Vector2 pointB = new Vector2(transform.position.x - player.GetComponent<MOVEMENT>().playerSize.x * width * 0.5f,
                                    transform.position.y - player.GetComponent<MOVEMENT>().playerSize.y * height * 0.5f);
        inArea = Physics2D.OverlapArea(pointA, pointB, playerLayer);

        if (inArea)
        {
            enabled = false;
            MOVEMENT.checkpointPos = transform.position;
            SaveSystem.SavePlayer();
        }
    }

    private void OnDrawGizmos()
    {
        Color pastelGreen = new Color(0.64f, 0.9f, 0.66f, 1f);
        Color babyOrange = new Color(0.99f, 0.84f, 0.69f, 1f);
        Gizmos.color = pastelGreen;

        Vector3 R1L1 = new Vector3(transform.position.x + player.GetComponent<MOVEMENT>().playerSize.x * width * 0.5f,
                                    transform.position.y + player.GetComponent<MOVEMENT>().playerSize.y * height * 0.5f, 0f);
        Vector3 R1L0 = new Vector3(transform.position.x + player.GetComponent<MOVEMENT>().playerSize.x * width * 0.5f,
                                    transform.position.y - player.GetComponent<MOVEMENT>().playerSize.y * height * 0.5f, 0f);
        Vector3 R0L0 = new Vector3(transform.position.x - player.GetComponent<MOVEMENT>().playerSize.x * width * 0.5f,
                                    transform.position.y - player.GetComponent<MOVEMENT>().playerSize.y * height * 0.5f, 0f);
        Vector3 R0L1 = new Vector3(transform.position.x - player.GetComponent<MOVEMENT>().playerSize.x * width * 0.5f,
                                    transform.position.y + player.GetComponent<MOVEMENT>().playerSize.y * height * 0.5f, 0f);
        //Edges
        Gizmos.DrawLine(R1L1, R1L0);
        Gizmos.DrawLine(R1L0, R0L0);
        Gizmos.DrawLine(R0L0, R0L1);
        Gizmos.DrawLine(R0L1, R1L1);

        //Diagonals
        Gizmos.color = babyOrange;
        Gizmos.DrawLine(R1L1, R0L0);
        Gizmos.DrawLine(R1L0, R0L1);
    }
}