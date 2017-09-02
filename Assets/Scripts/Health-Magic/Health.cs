using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	
	private int maxHP = 0;
	[SerializeField] private int hp = 0;
	[SerializeField] private int regenHP = 0;

	//Health Points getter and setter
	public int HP {
		get{return hp;}
		set{hp = maxHP = value;}
	}

	public void decrease (int damage) {
		hp -= damage;
	}

	public int RegenHP {
        set {
            regenHP = value;
        }
	}

	//allows healing over time
	public void regen () {
		hp += regenHP;
	}

	//heals a given amount
	public void restoreHealth (int healAmount) {
		hp += healAmount;
	}

}