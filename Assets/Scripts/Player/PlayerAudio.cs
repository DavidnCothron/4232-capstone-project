using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
	[SerializeField]private AudioClip[] footsteps_dirt;
	[SerializeField]private AudioSource audioSource;
	[SerializeField]private PlayerPlatformerController platformController;
	IEnumerator runDirtCo;
	// Use this for initialization
	void OnEnable () {
		runDirtCo = run_dirt();
		StartCoroutine(checkForMovement());
	}
	
	void stopAllCoroutines() {
		if (runDirtCo != null) {
			StopCoroutine(runDirtCo);
		}
	}

	IEnumerator checkForMovement() {
		while(true) {
			if (platformController.getGrounded() && Mathf.Abs(platformController.getVelocity().x) > 0.01f) {
				if (runDirtCo != null) StopCoroutine(runDirtCo);
				runDirtCo = run_dirt();
				yield return StartCoroutine(runDirtCo);
			}
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator run_dirt() {
		while(platformController.getGrounded() && Mathf.Abs(platformController.getVelocity().x) > 0.01f) {
			audioSource.clip = footsteps_dirt[Random.Range(0, footsteps_dirt.Length)];
			audioSource.volume = 0.05f;
			audioSource.Play();
			yield return new WaitForSeconds(0.3f);
		}
		yield return null;
	}
}
