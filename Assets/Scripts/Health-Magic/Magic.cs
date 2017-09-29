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
	private float regenSpeed = 0.25f, regenSpeedCountdown;
	private float regenDelay = 0.5f, regenDelayCountdown;
	private bool isRegenDelay = false;
	public bool isRegen = true;

	public void Initialize(int mp, int regen, float regenSpeed, float regenDelay){
		CurrentMP = mp;
		MaxMP = mp;
		regenMagic = regen;
		this.regenSpeed = regenSpeed;
		this.regenDelay = regenDelay;
		regenSpeedCountdown = regenSpeed;
		regenDelayCountdown = regenDelay;
		isRegen = true;
	}

	void Update () {
		if (true)
		{
			Debug.Log ("Passed Regen Check");
			if (!isRegenDelay)
			{
				Debug.Log ("Countdown after delay check");
				if (regenSpeedCountdown > 0 && currentMP < maxMP)
				{
					regenSpeedCountdown -= Time.deltaTime;
				}
				else
				if (regenSpeedCountdown <= 0 && currentMP < maxMP)
				{
					Debug.Log("Regenerating...");
					currentMP += regenMagic;
					regenSpeedCountdown = regenSpeed;
				}
			}
			else //Delay regeneration until regen delay is down.
			{
				Debug.Log ("Delaying regen");
				if (regenDelayCountdown > 0)
				{
					regenDelayCountdown -= Time.deltaTime;
				}
				else
				{
					regenDelayCountdown = regenDelay;
					isRegenDelay = false;
				}
			}

			if (currentMP <= 0)
			{
				Debug.Log ("Out of mana, initiate delay");
				isRegenDelay = true;
			}
		}
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
