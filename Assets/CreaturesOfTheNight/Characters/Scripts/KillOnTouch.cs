using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnTouch : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.SendMessage("Die");
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        col.gameObject.SendMessage("Die");
    }
}
