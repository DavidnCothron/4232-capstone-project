using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Health
{

	//status bars
	[SerializeField] private Stat healthBar;
	 private int maxHP = 4;
	 private int currentHP = 4;
	 private int regenHP = 0;

	public void Initialize(int hp){
		CurrentHP = hp;
		MaxHP = hp;
	}

	//Health Points getter and setter
	public int CurrentHP {
		get
		{
			return currentHP;
		}
		set
		{
			currentHP = Mathf.Clamp(value, 0, maxHP);
			healthBar.CurrentVal = (float)value;
		}
	}

	public int MaxHP {
		get
		{
			return maxHP;
		}
		set
		{
			maxHP = value;
			healthBar.MaxVal = (float)value;
		}
	}

	public void decrease (int damage) {
		CurrentHP -= damage;
		healthBar.CurrentVal = CurrentHP;
	}


	public void restore (int healAmount) {
		CurrentHP += healAmount;
		healthBar.CurrentVal = CurrentHP;
	}

}