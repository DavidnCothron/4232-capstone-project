using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
	public PlayerManager player = null;
	public List<Enemy> enemies;

	// Use this for initialization
	void OnEnable () {
		if (player == null)
		{
			player = GameObject.Find ("Player").GetComponent(typeof(PlayerManager)) as PlayerManager;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
