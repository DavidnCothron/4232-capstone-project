using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
	public PlayerManager player = null;
	public List<Enemy> enemies;
	[SerializeField]private string roomID;

	// Use this for initialization
	void OnEnable () {
		if (player == null)
		{
			player = GameObject.Find ("Player").GetComponent(typeof(PlayerManager)) as PlayerManager;
		}
		roomID = GameControl.control.createGUID ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string getRoomID() {
		return roomID;
	}
}
