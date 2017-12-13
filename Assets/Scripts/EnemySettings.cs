using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySettings : MonoBehaviour {
	[SerializeField] private Vector3 initialPosition;
	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void enemyReset() {
		
	}


}
