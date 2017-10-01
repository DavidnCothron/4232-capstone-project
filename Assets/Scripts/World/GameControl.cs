using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.IO;
using System;

public class GameControl : MonoBehaviour {

	//delegate void onSceneChanged();
	public static GameControl control;
	public PlayerHealthAndMagicController phmc;
	public PlayerController pc;
	public PlayerMeleeScript pms;
	public playerProjectileScript pps;
	public RoomController rc;
	private bool beatBoss1, beatBoss2, beatBoss3; 
	[SerializeField] private bool phaseAbility, projectileAbility, chargeAttackAbility;
	private int roomID, areaID;

	//float value used to store time (in seconds) that a room transition takes
	[SerializeField] private float roomTransitionTime;

	// Use this for initialization
	void Awake () {
		//Code for singleton behaviour
		if (control == null)
		{
			control = this;
			DontDestroyOnLoad (gameObject);
		}
		else if(control != this){
			Destroy (gameObject);
		}
		setRoomIDs ();
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();	
		FileStream file = File.Open (Application.persistentDataPath + "/saveData.dat", FileMode.Open);

		GameData data = new GameData ();
		#region save data assignment
		data.health = phmc.GetHealth();
		data.maxMana = phmc.GetMaxMana ();
		data.hasChargeAttack = pms.hasChargeAttack;
		data.hasRangedAttack = pps.hasRangedAttack;
		data.areaID = 1; //Get area ID from RoomController
		data.roomID = 1; //Get room ID from RoomController
		data.beatBoss1 = beatBoss1; 
		data.beatBoss2 = beatBoss2;
		data.beatBoss3 = beatBoss3;
		#endregion
		bf.Serialize(file, data);
		file.Close ();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + "/saveData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/saveData.dat", FileMode.Open);
			GameData data = (GameData)bf.Deserialize (file);
			file.Close ();

			#region load saved data
			phmc.SetHealth(data.health);
			phmc.SetMaxMana(data.maxMana);
			pms.hasChargeAttack = data.hasChargeAttack;
			pps.hasRangedAttack = data.hasRangedAttack;
			//Set area ID
			//Set room ID
			beatBoss1 = data.beatBoss1;
			beatBoss2 = data.beatBoss2;
			beatBoss3 = data.beatBoss3;
			#endregion
		}	
	}

	public void SetRoomAreaID(int room, int area)
	{
		//Call to this from RoomController;
		roomID = room;
		areaID = area;
	}

	public void SetBossDefeated(int bossID){
		switch (bossID)
		{
		case 1:
			beatBoss1 = true;
			break;
		case 2:
			beatBoss2 = true;
			break;
		case 3:
			beatBoss3 = true;
			break;
		}
	}
	
		public void ItemPickup(string item, bool setValue){
		switch(item){
			case "healthRegain":
				phmc.GainHealth(1);
				break;
			case "chargeAttack":
				chargeAttackAbility = setValue;
				break;
			case "projectile":
				projectileAbility = setValue;
				break;
			case "phase":
				phaseAbility = setValue;
				break;
		}
	}


	/// <summary>
	/// Creates and returns a unique string ID
	/// </summary>
	/// <returns>The GUID.</returns>
	public string createGUID() {
		Guid g = Guid.NewGuid ();
		string GuidString = Convert.ToBase64String (g.ToByteArray ());
		GuidString = GuidString.Replace ("=", "");
		GuidString = GuidString.Replace ("+", "");
		return GuidString;
	}

	/// <summary>
	/// Gets the child game objects of a given game object.
	/// </summary>
	/// <returns>The child game objects.</returns>
	/// <param name="obj">Object.</param>
	public GameObject[] GetChildGameObjects(GameObject obj){
		Transform[] temp;
		temp = obj.GetComponentsInChildren<Transform> (true);

		GameObject[] children = new GameObject[temp.Length - 1];
		for (int i = 0; i < temp.Length - 1; i++) {
			children [i] = temp [i + 1].gameObject;
		}
		return children;
	}

	/// <summary>
	/// Returns a specific game object from an array of game objects by searching for a tag.
	/// </summary>
	/// <returns>The game object from array.</returns>
	/// <param name="objs">Objects.</param>
	/// <param name="s">S.</param>
	public GameObject FindGameObjectFromArray(GameObject[] objs, string s){
		foreach (GameObject obj in objs) {
			if (obj.tag == s)
				return obj;
		}
		Debug.Log ("Object with tag " + s + " could not be found.");
		return null;
	}

	/// <summary>
	/// Calls coroutines within the CameraController class that fade the image covering the camera viewport.
	/// </summary>
	/// <param name="s">S.</param>
	public void fadeImage(string s) {
		if (s.Equals ("black"))
			StartCoroutine (Camera.main.GetComponent<CameraController> ().fadeToBlack ());
		else
			StartCoroutine (Camera.main.GetComponent<CameraController> ().fadeToClear ());
	}

	/// <summary>
	/// Gives each room in the scene a unique string ID
	/// </summary>
	private void setRoomIDs() {
		GameObject[] roomObjects = GameObject.FindGameObjectsWithTag ("Room");
		foreach (GameObject obj in roomObjects) {
			obj.GetComponent<RoomController> ().setRoomID (createGUID ());
		}
	}

	//return the room transition time
	public float getRoomTransTime() {
		return roomTransitionTime;
	}

}

[Serializable]
class GameData
{
	public int health;
	public int maxMana;
	public bool hasChargeAttack;
	public bool hasRangedAttack;
	public int areaID;
	public int roomID;
	public bool beatBoss1;
	public bool beatBoss2;
	public bool beatBoss3;
}

