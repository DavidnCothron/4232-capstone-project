using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.IO;
using System;

public struct AreaTransTuple{
	public int areaID;
	public int accessPointID;
}

public class GameControl : MonoBehaviour {

	//delegate void onSceneChanged();
	private RoomController currentOccupiedRoom;
	public static GameControl control;
	public GameObject player;
	public PlayerHealthAndMagicController phmc;
	public PlayerPlatformerController pc;
	public PlayerMeleeScript pms;
	public playerProjectileScript pps;
	public RoomController rc;
	private bool beatBoss1, beatBoss2, beatBoss3; 
	[SerializeField] private bool phaseAbility, projectileAbility, chargeAttackAbility;
	[SerializeField] private AreaTransTuple nextArea;
	private Dictionary<AreaTransTuple, AreaTransTuple> areaAccessPointMappings = new Dictionary<AreaTransTuple, AreaTransTuple>(); //could save this dictionary in save file at some point

	//float value used to store time (in seconds) that a room transition takes
	[SerializeField] private float roomTransitionTime;
	private string currentRoomID;

	// Use this for initialization
	void Awake () {
	//	for (int i = 0; i < 256; i++) 
	//	{
	//		Debug.Log (combinations (i, 8));
	//	}
		
		
		//Code for singleton behaviour
		
		if (control == null)
		{
			control = this;
			DontDestroyOnLoad (gameObject);
			GameControl.control.UpdatePlayerReferences();
		}else if(control != this){
			GameControl.control.UpdatePlayerReferences();
			Destroy (gameObject);
		}
		setRoomComponents ();
		
	}
	
	void Update(){
		
		if(Input.GetKeyDown(KeyCode.O))
		{
			phmc.LoseHealth(1);
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			phmc.LoseMana(1);
		}	
		if(Input.GetKeyDown(KeyCode.H)){
			phmc.ScaleHealth(1);
		}
		if(Input.GetKeyDown(KeyCode.M)){
			phmc.ScaleMagic(1);
		}		
	}

	public Transform GetPlayerTransform()
	{
		return pc.transform;
	}
	
	public void UpdatePlayerReferences(){
		player = GameObject.Find("Player");
		phmc = player.GetComponent(typeof(PlayerHealthAndMagicController)) as PlayerHealthAndMagicController;
		pc = player.GetComponent(typeof(PlayerPlatformerController)) as PlayerPlatformerController;
		pms = player.GetComponent(typeof(PlayerMeleeScript)) as PlayerMeleeScript;
		pps = player.GetComponent(typeof(playerProjectileScript)) as playerProjectileScript;

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
	
	public void MapAreaTransitionDoors(AreaTransTuple current, AreaTransTuple next){
		if(!areaAccessPointMappings.ContainsKey(current)){
			areaAccessPointMappings.Add(current, next);
			Debug.Log("linked current to next\n next area ID: " + areaAccessPointMappings[current].areaID);
			if(!areaAccessPointMappings.ContainsKey(next))
				areaAccessPointMappings.Add(next, current); Debug.Log("linked next to current\n previous area ID: " + areaAccessPointMappings[next].areaID);			
		}		
	}
	public void TransitionAreas(AreaTransTuple current, AreaTransTuple next){
		MapAreaTransitionDoors(current, next);
		SetNextArea(areaAccessPointMappings[current]);
		Debug.Log("next area set to: " + areaAccessPointMappings[current].areaID);
	}

	public void SetNextArea(AreaTransTuple next)
	{
		nextArea = next;
	}

	public AreaTransTuple GetNextArea(){
		return nextArea;
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

	public void KillPlayer(){
		
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
		else if(s.Equals("startBlack"))
			Camera.main.GetComponent<CameraController> ().setToBlack();
		else
			StartCoroutine (Camera.main.GetComponent<CameraController> ().fadeToClear ());
	}

	/// <summary>
	/// Gives each room in the scene a unique string ID
	/// </summary>
	private void setRoomComponents() {
		GameObject[] roomObjects = GameObject.FindGameObjectsWithTag ("Room");
		foreach (GameObject obj in roomObjects) {
			if(obj.GetComponent<RoomController> ().isSaveRoom)
				obj.GetComponent<RoomController> ().setRoomID ();
			else
				obj.GetComponent<RoomController> ().setRoomID (createGUID ());
			obj.GetComponent<RoomController> ().setRoomExtents (FindGameObjectFromArray (GetChildGameObjects (obj), "RoomBackground").GetComponent<SpriteRenderer> ());
		}
	}

	//return the room transition time
	public float getRoomTransTime() {//stores next area spawn room based on previous area exit
		return roomTransitionTime;
	}
	
	public void setCurrentRoom(RoomController room) {
		currentOccupiedRoom = room;
	}

	public RoomController getCurrentRoom() {
		return currentOccupiedRoom;
	}

	string combinations(int n, int len) {
		return (len > 1 ? combinations (n >> 1, len - 1) : null) + "01" [n & 1];
	}

/* OK you need to figure out how to get a save room reference in order to do this 
	/// <summary>
	/// Handles the transition between rooms when the player dies
	/// </summary>
	/// <returns>The transition.</returns>
	/// <param name="c">C.</param>
	public IEnumerator SaveRoomTransition() {
		GameObject c = GameControl.control.GetPlayerTransform().gameObject;
		//Set body type to kinematic to ensure smooth transition (doesn't look right yet)
		c.GetComponent<PlayerPlatformerController> ().haltInput = true;
		c.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		//fade to black > move player into door > move player behind other door > fade to clear > move player out of other door
		GameControl.control.fadeImage ("black");
		yield return StartCoroutine (movePlayer (c, playerSpawn.transform.position));
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		c.transform.position = other.getSpawn ().transform.position;
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		GameControl.control.fadeImage ("");
		yield return StartCoroutine (movePlayer (c, other.getDestination ().transform.position));

		//Resets the RigidbodyType2D to Dynamic and returns input control to the player
		c.GetComponent<PlayerPlatformerController> ().haltInput = false;
	}

	/// <summary>
	/// Coroutine called from roomTransition coroutine that physically moves the player
	/// from one room to another using Kinematic Movement
	/// </summary>
	/// <returns>The player.</returns>
	/// <param name="c">C.</param>
	/// <param name="t">T.</param>
	IEnumerator movePlayer(GameObject c, Vector3 t) {
		KinematicArrive.KinematicSteering steering;
		while (true) {
			c.GetComponent<KinematicArrive> ().setTarget (new Vector3(t.x, c.transform.position.y, t.z));
			steering = c.GetComponent<KinematicArrive> ().getSteering ();
			c.GetComponent<KinematicArrive> ().setOrientations (steering);
			if (c.GetComponent<KinematicArrive> ().getArrived ())
				yield break;
			else
				yield return null;
		}
	}


*/

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

