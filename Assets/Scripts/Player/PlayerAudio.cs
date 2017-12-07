using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
	[SerializeField]private AudioClip[] footsteps_dirt;
	[SerializeField]private AudioSource audioSource;
	[SerializeField]private PlayerPlatformerController platformController;
	[SerializeField]private KinematicArrive playerArrive;
	IEnumerator runDirtCo, jumpDirtCo, landDirtCo;
	int counter;
	// Use this for initialization
	void OnEnable () {
		runDirtCo = run_dirt();
		StartCoroutine(checkForMovement());
		StartCoroutine(checkForJump());
	}
	
	void stopAllCoroutines() {
		if (runDirtCo != null) {
			StopCoroutine(runDirtCo);
		}
		if(jumpDirtCo != null) {
			StopCoroutine(jumpDirtCo);
		}
		if(landDirtCo != null) {
			StopCoroutine(landDirtCo);
		}
	}

	IEnumerator checkForMovement() {
		while(true) {
			if (platformController.getGrounded() && (Mathf.Abs(platformController.getVelocity().x) > 0.01f) || Mathf.Abs(playerArrive.getSteeringVelocity().x) > 0.1f) {
				if (runDirtCo != null) StopCoroutine(runDirtCo);
				runDirtCo = run_dirt();
				yield return StartCoroutine(runDirtCo);
			}
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator checkForJump() {
		if(platformController.getGrounded() && Input.GetButtonDown("Jump")) {
			//if (jumpDirtCo != null) StopCoroutine(jumpDirtCo);
			stopAllCoroutines();
			jumpDirtCo = jump_dirt();
			yield return StartCoroutine(jumpDirtCo);
		}
		yield return null;
	}

	IEnumerator run_dirt() {
		while(platformController.getGrounded() && (Mathf.Abs(platformController.getVelocity().x) > 0.01f || Mathf.Abs(playerArrive.getSteeringVelocity().x) > 0.1f)) {
			//Debug.Log(playerArrive.getSteering().velocity.x);
			audioSource.clip = footsteps_dirt[Random.Range(0, footsteps_dirt.Length)];
			audioSource.volume = 0.35f;
			audioSource.Play();
			yield return new WaitForSeconds(0.3f);
		}
		yield return null;
	}

	IEnumerator jump_dirt() {
		counter = 0;
		while(counter < 2){
			audioSource.clip = footsteps_dirt[Random.Range(0, footsteps_dirt.Length)];
			audioSource.volume = 0.45f;
			audioSource.Play();
			yield return new WaitForSeconds(0.1f);
			counter++;
		}
		yield return null;
	}

	IEnumerator land_dirt() {
		yield return null;
	}
}
