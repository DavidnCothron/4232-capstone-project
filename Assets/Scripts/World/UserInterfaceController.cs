using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceController : MonoBehaviour {

	public static UserInterfaceController instance;


	void Awake () {
		//Code for singleton behaviour
		
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}else if(instance != this){
			Destroy (gameObject);
		}
		
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
