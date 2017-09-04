using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthAndMagicController : MonoBehaviour {
	

	// Player magic
    [SerializeField] private Magic magic;
	[SerializeField] private Health health;
	
    [SerializeField] private int playerMagic = 3;
	[SerializeField] private int magicRegen = 1;
    [SerializeField] private int playerHealth = 4;
	
	void Awake(){
		health.Initialize(playerHealth);
		magic.Initialize(playerMagic, magicRegen);
	}

	//update health here
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.O)){
			health.decrease(1);
			Debug.Log(health.CurrentHP);
		}
		if(Input.GetKeyDown(KeyCode.P)){
			health.restore(1);
			Debug.Log(health.CurrentHP);
		}
		if(health.CurrentHP <= 0){
			Debug.Log("you ran out of health");
			//TODO: Implement Lose state
		}

		if(Input.GetKeyDown(KeyCode.K)){
			magic.decrease(1);
			Debug.Log(magic.CurrentMP);
		}
		if(Input.GetKeyDown(KeyCode.L)){
			magic.restore(1);
			Debug.Log(magic.CurrentMP);
		}

		if(magic.CurrentMP <= 0){
			Debug.Log("you ran out of magic");
		}
	}	
}
