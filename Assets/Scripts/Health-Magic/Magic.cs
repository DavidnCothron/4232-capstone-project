using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Magic{
	
	[SerializeField] private Stat magicBar;
	 private int maxMP = 3;
	 private int currentMP = 3;
	 private int regenMagic = 0;

	public void Initialize(int mp, int regen){
		CurrentMP = mp;
		MaxMP = mp;
		regenMagic = regen;		
	}

	//Magic Points getter and setter
	public int CurrentMP {
		get
		{
			return currentMP;
		}
		set
		{
			currentMP = Mathf.Clamp(value, 0, maxMP);
			magicBar.CurrentVal = (float)value;
		}
	}

	public int MaxMP {
		get
		{
			return maxMP;
		}
		set
		{
			maxMP = value;
			magicBar.MaxVal = (float)value;
		}
	}

	public void decrease (int magicUsed) {
		CurrentMP -= magicUsed;
		magicBar.CurrentVal = CurrentMP;
	}

	//restores magic a given amount
	public void restore (int magicAmont) {
		CurrentMP += magicAmont;
		magicBar.CurrentVal = CurrentMP;
	}

}
