using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionScript : MonoBehaviour
{

    private TurretScript turret;
    // Start is called before the first frame update
    void Start()
    {
        turret = GetComponentInParent<TurretScript>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            turret.playerInRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            turret.playerInRange = false;
        }   
    }
}
