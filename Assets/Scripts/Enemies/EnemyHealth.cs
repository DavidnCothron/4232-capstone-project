using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	[SerializeField] private Health health;
	[SerializeField] private int enemyHP = 5;
	private int enemyCurrentHP;
	enemyController enemy;
	
	void Awake(){
		health.Initialize(enemyHP);
		enemy = this.GetComponent<enemyController>();
	}
	void FixedUpdate()
	{
		if(health.CurrentHP <= 0){
			enemy.Die();
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
