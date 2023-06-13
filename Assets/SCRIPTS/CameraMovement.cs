using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 difference;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        difference = transform.position - player.transform.position;
    }
    void Update()
    {
        transform.position = player.transform.position + difference;
    }
}
