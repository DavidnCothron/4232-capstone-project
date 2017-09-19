using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour {



	#region Attribute Variables
	public float maxChargeTime = 2.0f;
	public float attackDuration = 0.1f;
	public bool hasHolySword = false;
	public bool hasRangedAttack = false;
	//public List<Enemy> enemies;
	public ObjectPooler projectilePooler;
	#endregion

	#region Ability Damage
	public int basicAttackDamage;
	public int rangedAttackDamage;
	public int chargedAttackDamage;
	#endregion

	#region direction vector
	Vector3 attackDirection;
	Vector3 mousePos;
	#endregion

	#region Other Controllers
	public PlayerController playerController;
	PlayerManager manager;
	#endregion

	#region Flow Control
	bool isAttacking = false;
	bool isChargeAttacking = false;
	bool isShooting = false;
	float attackDurationRemaining;
	float chargeTime;
	#endregion

	// Use this for initialization
	void Awake () {
		chargeTime = maxChargeTime;
		attackDurationRemaining = attackDuration;
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
			if(Input.GetKey(KeyCode.LeftShift) && hasRangedAttack)
			{
				isShooting = true;
				isAttacking = false;
			}
			else if (chargeTime > 0)
			{
				isAttacking = true;
			}
			else if (chargeTime <= 0 && hasHolySword)
			{
				isChargeAttacking = true;
			}
		}
		else
		{
			chargeTime = maxChargeTime;
		}
		#endregion

		if (attackDurationRemaining <= 0)
		{
			isAttacking = false;
			attackDurationRemaining = attackDuration;
		}
	}
}
