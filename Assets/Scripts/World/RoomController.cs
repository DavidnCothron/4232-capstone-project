using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
	public PlayerManager player = null;
	public List<Enemy> enemies;
	[SerializeField] private SpriteRenderer roomExtents;
	[SerializeField] private string roomID;//ID that every room will have
	[SerializeField] private int areaRoomID;//ID for area entry and exit rooms
	[SerializeField] public bool isSaveRoom = false;
	[SerializeField] private string saveRoomId;
	public bool roam_music, title_music, boss_music, bossKey_music;

	[SerializeField]private GameObject enemyContainer;



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

	public void setRoomID() {
		roomID = saveRoomId;
	}

	public void setMusic(string type) {
		if (type.CompareTo("title") == 0) title_music = true;
		if (type.CompareTo("roam") == 0) roam_music = true;
		if (type.CompareTo("key") == 0) bossKey_music = true;
		if (type.CompareTo("boss") == 0) boss_music = true; 
	}

	public string getMusicType() {
		if (title_music) return "title";
		if (roam_music) return "roam";
		if (bossKey_music) return "key";
		if (boss_music) return "boss";
		return null;
	}

	public IEnumerator checkForPlayerExit() {
		while(this.roomID == GameControl.control.getCurrentRoom().getRoomID()) {
			yield return null;
		}
		Debug.Log("leaving room");
		yield return null;
	}
}
