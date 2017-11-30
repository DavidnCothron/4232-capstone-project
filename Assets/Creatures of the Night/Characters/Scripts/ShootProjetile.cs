using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjetile : MonoBehaviour {
    public GameObject PrefabProjectile;
    public Transform ProjectileInitialPosition;


	public void ShootNow()
    {
        print("SHOOT");
        GameObject instant = (GameObject)Instantiate(PrefabProjectile, ProjectileInitialPosition.position, Quaternion.identity);
        Vector3 scale = instant.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(transform.localScale.x);
        instant.transform.localScale = scale;
        Physics2D.IgnoreCollision(instant.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
}
