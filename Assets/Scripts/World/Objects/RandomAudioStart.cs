using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioStart : MonoBehaviour {
	private AudioSource audioSource;
	private float waitTime;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		waitTime = Random.Range(0,5);
	}
	
	// Update is called once per frame
	void Update () {
		waitTime -= Time.deltaTime;
		if (waitTime <= 0) {
			audioSource.Play();
			this.GetComponent<RandomAudioStart>().enabled = false;
		}
	}
}
