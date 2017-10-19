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
	[SerializeField] GameObject barToScale;
	private bool isMagicRegenDelay = false;
	public bool isMagicRegen = true;
	private int maxMP = 3;
	private int currentMP = 3;
	private int regenMagic = 0;
	[SerializeField] private GameObject newBarSeparator;
	
	void Awake(){
		health.Initialize(playerHealth);
		magic.Initialize(playerMagic, magicRegen, regenSpeed, regenDelay);
		magicRegenDelayCountdown = magicRegenDelay;
		magicRegenSpeedCountdown = magicRegenSpeed;
		ScaleHealth(0);
		ScaleMagic(0);
	}
		
	void Update()
	{
		MagicRegen ();

		if(Input.GetKeyDown(KeyCode.O))
		{
			LoseHealth(1);
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			LoseMana(1);
		}
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

	public void SetMaxHealth(int amount){
		health.MaxHP = amount;
	}

	public void SetMana(int amount){
		magic.CurrentMP = amount;
	}

	public void SetMaxMana(int amount){
		magic.MaxMP = amount;
	}
	#endregion

	public void ScaleHealth(int amountIncrease){
		playerHealth += amountIncrease;
		SetMaxHealth(playerHealth);
		SetHealth(playerHealth);
		barToScale = GameObject.Find("Health");
		GameObject healthMask = GameControl.control.FindGameObjectFromArray(GameControl.control.GetChildGameObjects(barToScale), "healthMask");
		RedrawBars(healthMask, healthMask.GetComponent<RectTransform>(), playerHealth);
	}

	public void ScaleMagic(int amountIncrease){
		playerMagic += amountIncrease;
		SetMaxMana(playerMagic);
		SetMana(playerMagic);
		barToScale = GameObject.Find("Magic");
		GameObject magicMask = GameControl.control.FindGameObjectFromArray(GameControl.control.GetChildGameObjects(barToScale), "magicMask");
		RedrawBars(magicMask, magicMask.GetComponent<RectTransform>(), playerMagic);

	}

	void RedrawBars(GameObject mask, RectTransform maskTrans, int points){
		GameObject [] separatorArray = GameControl.control.GetChildGameObjects(mask);
		float posRatio = maskTrans.rect.width/points;
		int divisions = points-1;
		int barNum = 1;
		RectTransform currentBarToMove;
		
		foreach(var separator in separatorArray){
			currentBarToMove = separator.GetComponent<RectTransform>();
			if(separator.tag == "barSeparator" && divisions != 0){					
				currentBarToMove.transform.localPosition = new Vector3((barNum * posRatio)-100, 0, 0);
				barNum ++;
				divisions--;
			}else{
				if(separator.tag == "barSeparator")
					Destroy(separator.gameObject);
			}
		}

		if(divisions > 0){
			Debug.Log("divs: " + divisions);
			for(int i = divisions; i > 0; i--){
				GameObject newBar = Instantiate(newBarSeparator) as GameObject;					
				newBar.transform.parent = mask.transform;
				newBar.transform.localScale = mask.transform.localScale;
				newBar.transform.localPosition =  new Vector3((barNum * posRatio)-100, 0, 0);
				barNum ++;
				divisions--;
			}
		}
	}

	void MagicRegen(){
		if (isMagicRegen)
		{
			//Debug.Log ("Passed Regen Check");
			if (!isMagicRegenDelay)
			{
				//Debug.Log ("Countdown after delay check");
				if (magicRegenSpeedCountdown > 0 && GetMana() < GetMaxMana())
				{
					magicRegenSpeedCountdown -= Time.deltaTime;
				}
				else
					if (magicRegenSpeedCountdown <= 0 && GetMana() < GetMaxMana())
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
