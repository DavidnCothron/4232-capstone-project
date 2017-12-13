using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightMusicTrigger : MonoBehaviour {
	[SerializeField] private AudioController audioController;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D c){
		if (c.CompareTag("Player")) audioController.setFightBoss(true);
	}
}
