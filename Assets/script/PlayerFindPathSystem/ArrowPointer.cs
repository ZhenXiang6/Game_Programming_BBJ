using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPointer2D : MonoBehaviour
{
    public ArrowData ad;
    public Transform player; // 角色物體
    public Transform destination; // 目的地物體
    public GameObject arrowPanel; // 箭頭圖像
    public Image Arrow;
    public float touchDistance = 1f; // Distance threshold to consider as "touching" the portal

    void Start () 
    {
        ad.showArrow = false;
        destination = ad.destination;
        Arrow.gameObject.SetActive(false);
    }

    void Update()
    {
        if (ad.showArrow) 
        {
            destination = ad.destination;
            Arrow.gameObject.SetActive(true);
            if (player != null && destination != null)
            {
                PointArrowTowardsDestination();

                // Check if the arrow (or player) is "touching" the portal
                if (Vector3.Distance(player.position, destination.position) <= touchDistance)
                {
                    ad.showArrow = false;
                    Arrow.gameObject.SetActive(false);
                    Debug.Log("Arrow reached the portal!");
                }
            }
        }
        else
        {
            Arrow.gameObject.SetActive(false);
        }
    }

    void PointArrowTowardsDestination()
    {
        // Calculate direction from player to destination
        Vector3 direction = destination.position - player.position;

        // Calculate arrow rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowPanel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
