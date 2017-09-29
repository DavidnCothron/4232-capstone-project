using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
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
	private int roomID, areaID;

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

