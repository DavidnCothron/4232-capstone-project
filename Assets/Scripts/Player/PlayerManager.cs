using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public PlayerController moveController;
	public PlayerActionController actionController;
	//public playerProjectileScript projectileController;
	public RoomController currentRoom;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateRoomValues(){
		actionController.enemies = currentRoom.enemies;
	}
}
