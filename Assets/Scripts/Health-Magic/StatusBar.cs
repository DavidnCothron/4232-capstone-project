using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour {
	
	[SerializeField] private float lerpSpeed;
	[SerializeField] private float fillAmount;
	[SerializeField] private Image filler;
	public float MaxValue{ get; set;}

	public float Value{
		set{
			fillAmount = Bound(value, 0, MaxValue, 0, 1);
		}
	}

	void Start(){
		lerpSpeed = 5f;
		fillAmount = 1f;

	}

	void Update(){
		BarHandler();		
	}

	private void  BarHandler(){
		if(fillAmount != filler.fillAmount){
			filler.fillAmount = Mathf.Lerp(filler.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
		}
	}
		//used to bound heath values		
	private float Bound(float value, float inMin, float inMax, float outMin, float outMax){
		return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

}
