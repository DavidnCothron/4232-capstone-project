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
		
	}
	
	void Start(){
		ScaleHealth(0);
		ScaleMagic(0);
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
		playerHealth += amountIncrease;//set health in this script
		SetMaxHealth(playerHealth);//set health max for the 'Health.cs' script
		SetHealth(playerHealth);//set health for the 'Health.cs' script
		barToScale = GameObject.Find("Health");//get reference to the Health component in the Health and magic UI
		GameObject healthMask = GameControl.control.FindGameObjectFromArray(GameControl.control.GetChildGameObjects(barToScale), "healthMask");// Gets an array of objects childed to the Health component and finds hild component with tag 'healthMask'
		RedrawBars(healthMask, healthMask.GetComponent<RectTransform>(), playerHealth);//calls method to redraw the bar
	}

	public void ScaleMagic(int amountIncrease){
		playerMagic += amountIncrease;//set magic in this script
		SetMaxMana(playerMagic);//set magic max for the 'Health.cs' script
		SetMana(playerMagic);//set magic for the 'Health.cs' script
		barToScale = GameObject.Find("Magic");//get reference to the Magic component in the Health and magic UI
		GameObject magicMask = GameControl.control.FindGameObjectFromArray(GameControl.control.GetChildGameObjects(barToScale), "magicMask");// Gets an array of objects childed to the Magic component and finds hild component with tag 'magicMask'
		RedrawBars(magicMask, magicMask.GetComponent<RectTransform>(), playerMagic);//calls method to redraw the bar

	}

	void RedrawBars(GameObject mask, RectTransform maskTrans, int numCells){//takes in the mask of the bar that needs to be scaled, its rect-transform, and the number of cells required in the new bar 
		GameObject [] separatorArray = GameControl.control.GetChildGameObjects(mask);//gets all children of the mask game object
		float posRatio = maskTrans.rect.width/numCells;//total width of the rectTransform divided by the number of cells needed
		int divisions = numCells-1;//number of separator bars needed = number of cells needed -1
		int barNum = 1;//counter for number of bars, multiplied by the posRatio for spacing
		RectTransform currentBarToMove;//temp variable that holds the current bar being moved
		
		foreach(var separator in separatorArray){//loops through the array of mask children and scales the current separators that exist 	
			if(separator.tag == "barSeparator" && divisions != 0){//as long as there are move divisions needed and the current array element is tagged with barSeparator
				currentBarToMove = separator.GetComponent<RectTransform>();//gets elements rectTrans	
				currentBarToMove.transform.localPosition = new Vector3((barNum * posRatio)-100, 0, 0);//sets the new Position
				barNum ++;
				divisions--;
			}else{
				if(separator.tag == "barSeparator")
					Destroy(separator.gameObject);//this destroys and extra bars that are left over after scaling, this will only be called if bar is scaled down
			}
		}

		if(divisions > 0){//if all existing separators have beed scaled but new divisions are needed then this executes
			for(int i = divisions; i > 0; i--){
				GameObject newSep = Instantiate(newBarSeparator) as GameObject;//instantiates new separator	
				newSep.transform.SetParent(mask.transform, false);//sets parent to the mask of the current health bar and sets worldPositionStays to false so that its scaled properly	
				//newSep.transform.parent = mask.transform;//childs the new separator to the mask object
				//newSep.transform.localScale = mask.transform.localScale;//sets the new separator scale to that of its parent

				newSep.transform.localPosition =  new Vector3((barNum * posRatio)-100, 0, 0); //sets position of the new separator
				
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
