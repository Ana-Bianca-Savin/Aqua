using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    public GameObject player;
    public bool isInArea;
    public SpriteRenderer sprite;
    public LayerMask playerLayer;

    private void FixedUpdate()
    {
        Vector2 pointA = (Vector2)transform.position + Vector2.up * sprite.bounds.extents.y + Vector2.right * sprite.bounds.extents.x;
        Vector2 pointB = (Vector2)transform.position + Vector2.down * sprite.bounds.extents.y + Vector2.left * sprite.bounds.extents.x;

        isInArea = Physics2D.OverlapArea(pointA, pointB, playerLayer);

        //player.GetComponent<MOVEMENT>().anim.SetBool("wind", isInArea);
        player.GetComponent<MOVEMENT>().wind = isInArea;
    }

    private void OnDrawGizmos()
    {
        Color meallow = new Color(0.97f, 0.87f, 0.49f, 1f);
        Gizmos.color = meallow;

        Vector2 A = (Vector2)transform.position + Vector2.up * sprite.bounds.extents.y + Vector2.right * sprite.bounds.extents.x;
        Vector2 B = (Vector2)transform.position + Vector2.down * sprite.bounds.extents.y + Vector2.left * sprite.bounds.extents.x;

        Gizmos.DrawLine(A, B);
    }
}
