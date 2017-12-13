using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	[SerializeField] private Health health;
	[SerializeField] private int enemyHP = 5;
	private int enemyCurrentHP;
	bool isBat;
	bool isBones;
	enemyController bones;
	BatController bat;
	
	void Awake(){
		health.Initialize(enemyHP);
		if(gameObject.name.Contains("ShamblingBones")){
			isBones = true;
			bones = this.GetComponent<enemyController>();
		}else if(gameObject.name.Contains("Bat")){
			isBat = true;
			bat = this.GetComponent<BatController>();
		}
		
	}
	void FixedUpdate()
	{
		if(health.CurrentHP <= 0){
			if(isBones)
				bones.Die();
			if(isBat)
				bat.Die();
		}
	}

	public void GainHealth(int amount){
		health.restore (amount);
	}

	public void LoseHealth(int amount){
		health.decrease (amount);
	}

	public void SetHealth(int amount){
		if(amount < health.MaxHP){
			health.CurrentHP = amount;
		}
	}

	public void SetMaxHealth(int amount){
		health.MaxHP = amount;
	}
}
