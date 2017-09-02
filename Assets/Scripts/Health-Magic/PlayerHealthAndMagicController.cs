using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthAndMagicController : MonoBehaviour {

	//status bars
	[SerializeField] private StatusBar healthBar;
	[SerializeField] private StatusBar magicBar;

	// Player magic
    private Magic magic;
    [SerializeField] private int playerMagic, magicRegen;

	// Player health
    private Health health;
    [SerializeField] private int hp;

	void Awake(){
		health = GetComponent<Health>();
		magic = GetComponent<Magic>();
	}

	void Start(){
		playerMagic = 3;
		magicRegen = 1;
		hp = 5;		

		magic.MP = playerMagic;
		magic.RegenMagic = magicRegen;
		magicBar.MaxValue = playerMagic;

		health.HP = hp;
		healthBar.MaxValue = hp;
	}
	
	void Update()
	{
		if(health.HP <= 0){
			//TODO: Implement Lose state
		}
	}
	
	public void decreaseHealth(int damage){
		health.decrease(damage);
		healthBar.Value = health.HP;
	}

	public void restoreHealth(int restoreAmount){
		health.restoreHealth(restoreAmount);
		healthBar.Value = health.HP;
	}

	public void useMagic(int magicAmout){
		magic.decrease(magicAmout);		
	}

	public void restoreMagic(int restoreAmount){
		magic.restoreMagic(restoreAmount);
		magicBar.Value = magic.MP;
	}

}
