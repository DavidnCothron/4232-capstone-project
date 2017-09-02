using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour {
	
	private int maxMagic = 0;
	[SerializeField] private int magic = 0;
	[SerializeField] private int regenMagic = 0;

	//Magic Points getter and setter
	public int MP {
		get{return magic;}
		set{magic = maxMagic = value;}
	}

	public void decrease (int magicUsed) {
		magic -= magicUsed;
	}

	public int RegenMagic {
        set {
            regenMagic = value;
        }
	}

	//allows magic regen over time
	public void regen () {
		magic += regenMagic;
	}

	//restores magic a given amount
	public void restoreMagic (int magicAmont) {
		magic += magicAmont;
	}

}
