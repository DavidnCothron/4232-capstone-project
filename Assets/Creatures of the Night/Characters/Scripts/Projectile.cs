using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public Vector2 ConstantVelocity=Vector3.right;
    Rigidbody2D rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (transform.localScale.x < 0f)
        {
            ConstantVelocity.x = -ConstantVelocity.x;
        }

    }
	
	// Update is called once per frame
	void Update () {

        rb.velocity = ConstantVelocity;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
            Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
            Destroy(gameObject);
   }
}
