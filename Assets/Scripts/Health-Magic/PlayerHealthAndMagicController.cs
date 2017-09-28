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
	[SerializeField] private float regenSpeed = 0.25f;
	[SerializeField] private float regenDelay = 0.5f;

	[SerializeField] private float magicRegenSpeed = 0.25f, magicRegenSpeedCountdown;
	[SerializeField] private float magicRegenDelay = 0.5f, magicRegenDelayCountdown;
	private bool isMagicRegenDelay = false;
	public bool isMagicRegen = true;
	private int maxMP = 3;
	private int currentMP = 3;
	private int regenMagic = 0;
	
	void Awake(){
		health.Initialize(playerHealth);
		magic.Initialize(playerMagic, magicRegen, regenSpeed, regenDelay);
		magicRegenDelayCountdown = magicRegenDelay;
		magicRegenSpeedCountdown = magicRegenSpeed;
	}
		
	void Update()
	{
		MagicRegen ();
	}

	#region Getters/Setters
	public void GainHealth(int amount){
		health.restore (amount);
	}

	public void LoseHealth(int amount){
		health.decrease (amount);
	}

	public void GainMana(int amount){
		magic.restore (amount);
	}

	public void LoseMana(int amount){
		magic.decrease (amount);
	}

	public int GetHealth(){
		return health.CurrentHP;
	}

	public int GetMana(){
		return magic.CurrentMP;
	}

	public int GetMaxMana(){
		return magic.MaxMP;
	}

	public void SetHealth(int amount){
		health.CurrentHP = amount;
	}

	public void SetMana(int amount){
		magic.CurrentMP = amount;
	}

	public void SetMaxMana(int amount){
		magic.MaxMP = amount;
	}
	#endregion

	void MagicRegen(){
		if (isMagicRegen)
		{
			//Debug.Log ("Passed Regen Check");
			if (!isMagicRegenDelay)
			{
				//Debug.Log ("Countdown after delay check");
				if (magicRegenSpeedCountdown > 0 && GetMana() < maxMP)
				{
					magicRegenSpeedCountdown -= Time.deltaTime;
				}
				else
					if (magicRegenSpeedCountdown <= 0 && GetMana() < maxMP)
					{
						GainMana (magicRegen);
						magicRegenSpeedCountdown = magicRegenSpeed;
					}
			}
			else //Delay regeneration until regen delay is down.
			{
				if (magicRegenDelayCountdown > 0)
				{
					magicRegenDelayCountdown -= Time.deltaTime;
				}
				else
				{
					magicRegenDelayCountdown = magicRegenDelay;
					isMagicRegenDelay = false;
				}
			}

			if (GetMana() <= 0 && isMagicRegenDelay)
			{
				isMagicRegenDelay = true;
			}
		}
	}
}
