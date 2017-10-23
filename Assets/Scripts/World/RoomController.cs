using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
	public PlayerManager player = null;
	public List<Enemy> enemies;
	[SerializeField] private SpriteRenderer roomExtents;
	[SerializeField] private string roomID;//ID that every room will have
	[SerializeField] private int areaRoomID;//ID for area entry and exit rooms

	// Use this for initialization
	void OnEnable () {
		if (player == null)
		{
			player = GameObject.Find ("Player").GetComponent(typeof(PlayerManager)) as PlayerManager;
		}
	}

	public SpriteRenderer getRoomExtents() {
		return roomExtents;
	}

	public void setRoomExtents(SpriteRenderer r) {
		roomExtents = r;
	}

	public string getRoomID() {
		return roomID;
	}

	public void setRoomID(string s) {
		roomID = s;
	}
}
