using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public static PlayerManager control;
	public PlayerController moveController;
	public RoomController currentRoom;

	[SerializeField]private PlayerPlatformerController ppc;

	private bool alive;
	
	//Daivd wanted me to note that this script is currently only used for the singleton... your welcome David - Josh 
	void Awake()
	{
		if (control == null)
		{
			control = this;
			DontDestroyOnLoad (this.gameObject);
			//GameControl.control.UpdatePlayerReferences();
		}else if(control != this){
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

	public void setAlive(bool b) {
		alive = b;
	}
}
