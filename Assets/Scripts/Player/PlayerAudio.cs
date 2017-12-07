using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
	[SerializeField]private AudioClip[] footsteps_dirt;
	[SerializeField]private AudioSource audioSource, audioSource_jump_land;
	[SerializeField]private PlayerPlatformerController platformController;
	[SerializeField]private KinematicArrive playerArrive;
	IEnumerator runDirtCo, jumpDirtCo, landDirtCo, checkForLandingCo;
	int jumpCounter, landCounter;
	float fallTime;

	void OnEnable () {
		runDirtCo = run_dirt();
		StartCoroutine(checkForMovement());
		StartCoroutine(checkForJump());
		StartCoroutine(checkForFalling());
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

	void initializeCoroutines() {
		runDirtCo = run_dirt();
		jumpDirtCo = jump_dirt();
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
		while(true) {
			if(platformController.getGrounded() && Input.GetButtonDown("Jump")) {
				if (jumpDirtCo != null) StopCoroutine(jumpDirtCo);
				jumpDirtCo = jump_dirt();
				yield return StartCoroutine(jumpDirtCo);
			}
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator checkForFalling() {
		while (true) {
			if (!platformController.getGrounded()) {
				if (checkForLandingCo != null) StopCoroutine(checkForLandingCo);
				checkForLandingCo = checkForLanding();
				yield return StartCoroutine(checkForLandingCo);
			}
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator checkForLanding() {
		fallTime = 0f;
		while(true) {
			fallTime += Time.fixedDeltaTime;
			if (platformController.getGrounded()) {
				if (landDirtCo != null) StopCoroutine(landDirtCo);
				landDirtCo = land_dirt(fallTime);
				yield return StartCoroutine(landDirtCo);
				break;
			}
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(0.025f);
		audioSource.mute = false;
		yield return null;
	}

	IEnumerator run_dirt() {
		while(platformController.getGrounded() && (Mathf.Abs(platformController.getVelocity().x) > 0.01f || Mathf.Abs(playerArrive.getSteeringVelocity().x) > 0.1f)) {
			if (Input.GetButtonDown("Jump")) {
				yield return null;
			}
			audioSource.clip = footsteps_dirt[Random.Range(0, footsteps_dirt.Length)];
			audioSource.volume = 0.20f;
			audioSource.Play();
			yield return new WaitForSeconds(0.35f);
		}
		yield return null;
	}

	IEnumerator jump_dirt() {
		audioSource.mute = true;
		jumpCounter = 0;
		audioSource_jump_land.volume = 0f;
		while(jumpCounter < 2){
			audioSource_jump_land.clip = footsteps_dirt[jumpCounter];
			audioSource_jump_land.volume += 0.05f;
			audioSource_jump_land.Play();
			yield return new WaitForSeconds(0.025f);
			jumpCounter++;
		}
		yield return new WaitForSeconds(0.025f);
		audioSource.mute = false;
		yield return null;
	}

	IEnumerator land_dirt(float fallTimeMult) {
		landCounter = 0;
		if (fallTimeMult > 1f) fallTimeMult = 1f;
		while (landCounter < 2) {
			audioSource.mute = true;
			audioSource_jump_land.clip = footsteps_dirt[Random.Range(0, footsteps_dirt.Length)];
			audioSource_jump_land.volume = fallTimeMult/2f;
			audioSource_jump_land.Play();
			yield return new WaitForSeconds(0.025f);
			landCounter++;
		}
		yield return null;
	}
}
