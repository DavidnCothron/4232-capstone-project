using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
	public PlayerManager player = null;
	public List<Enemy> enemies;

	// Use this for initialization
	void Start () {
		if (player == null)
		{
			player = GameObject.Find ("player").GetComponent(typeof(PlayerManager)) as PlayerManager;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
