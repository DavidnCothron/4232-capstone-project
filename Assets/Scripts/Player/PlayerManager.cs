using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public static PlayerManager control;
	public PlayerController moveController;
	public RoomController currentRoom;

	void Awake()
	{
		if (control == null)
		{
			control = this;
			DontDestroyOnLoad (this.gameObject);
			//GameControl.control.UpdatePlayerReferences();
		}else if(control != this){
			Debug.Log("testing not null");
			//GameControl.control.UpdatePlayerReferences();
			Destroy (this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateRoomValues(){
	}
}
