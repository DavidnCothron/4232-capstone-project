using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour {

	#region Flow Control Variables
	public float maxChargeTime = 2.0f;
	float chargeTime;
	bool hasTeleport = false;
	bool hasHolySword = false;
	#endregion

	#region Other Controllers
	public PlayerController playerController;
	public PlayerHitBoxController playerHitBoxController;
	#endregion

	// Use this for initialization
	void Awake () {
		chargeTime = maxChargeTime;
	}
	
	// Update is called once per frame
	void Update () {
		checkInput ();
	}

	void checkInput()
	{
		#region Left Mouse Button Flow Control
		if (Input.GetMouseButton (0))
		{
			chargeTime -= Time.deltaTime;

			if (chargeTime > 0)
			{
				BasicAttack ();
			}
			else if (chargeTime <= 0 && hasHolySword)
			{
				ChargeAttack ();
			}
		}
		else
		{
			chargeTime = maxChargeTime;
		}
		#endregion

		if (Input.GetMouseButtonDown (1) && hasTeleport)
		{
			TeleportAbility ();
		}
	}

	void BasicAttack(){
		if (playerController.isRolling)
		{
			RollAttack ();
		}
		else
		{
			//Do attack stuff
			playerHitBoxController.isHitboxActive = true;
		}
	}

	void ChargeAttack(){
		playerHitBoxController.isHitboxActive = false;
	}

	void RollAttack(){
		playerHitBoxController.isHitboxActive = false;
	}

	void TeleportAbility(){
		
	}
}
