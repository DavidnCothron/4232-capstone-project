using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
	[SerializeField]private AudioClip[] footsteps_dirt, sword_swoosh_ground, sword_swoosh_air, jump_grunts, landing_grunts;
	[SerializeField]private AudioSource audioSource, audioSource_jump_land, audioSource_sword, audioSource_voice;
	[SerializeField]private PlayerPlatformerController platformController;
	[SerializeField]private KinematicArrive playerArrive;
	IEnumerator runDirtCo, jumpDirtCo, landDirtCo, checkForLandingCo, swingSwordCo;
	int jumpCounter, landCounter;
	float fallTime;
	private static string jumpButton = "Jump";
	private AudioClip[] attack_orientation;

	void OnEnable () {
		StartCoroutine(checkForMovement());
		StartCoroutine(checkForJump());
		StartCoroutine(checkForFalling());
		StartCoroutine(checkForAttack());
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
			if(platformController.getGrounded() && Input.GetButtonDown(jumpButton)) {
				if (jumpDirtCo != null) StopCoroutine(jumpDirtCo);
				jumpDirtCo = jump_dirt();
				yield return StartCoroutine(jumpDirtCo);
			}
			yield return null;
		}
	}

	IEnumerator checkForFalling() {
		while (true) {
			if (!platformController.getGrounded() && !platformController.haltInput) {
				if (checkForLandingCo != null) StopCoroutine(checkForLandingCo);
				checkForLandingCo = checkForLanding();
				yield return StartCoroutine(checkForLandingCo);
			}
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator checkForAttack() {
		while(true) {
			if (Input.GetMouseButtonUp(0)) {
				yield return new WaitForSeconds(0.25f);
				if (platformController.getGrounded()) attack_orientation = sword_swoosh_ground;
				else attack_orientation = sword_swoosh_air;
				if (swingSwordCo != null) StopCoroutine(swingSwordCo);
				swingSwordCo = swing_sword();
				yield return StartCoroutine(swingSwordCo);
			}
			yield return null;
		}
	}

	IEnumerator checkForLanding() {
		fallTime = 0f;
		while(true) {
			if (platformController.getVelocity().y < 0) fallTime += Time.fixedDeltaTime;
			if (platformController.getGrounded()) {
				if (landDirtCo != null) StopCoroutine(landDirtCo);
				landDirtCo = land_dirt(fallTime);
				yield return StartCoroutine(landDirtCo);
				break;
			}
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(0.05f);
		audioSource.mute = false;
		yield return null;
	}

	IEnumerator run_dirt() {
		while(platformController.getGrounded() && (Mathf.Abs(platformController.getVelocity().x) > 0.01f || Mathf.Abs(playerArrive.getSteeringVelocity().x) > 0.1f)) {
			if (Input.GetButtonDown(jumpButton)) {
				yield return null;
			}
			audioSource.clip = footsteps_dirt[Random.Range(0, footsteps_dirt.Length)];
			audioSource.volume = 0.40f;
			audioSource.Play();
			yield return new WaitForSeconds(0.35f);
		}
		yield return null;
	}

	IEnumerator jump_dirt() {
		audioSource.mute = true;
		jumpCounter = 0;
		audioSource_jump_land.volume = 0f;
		//if (Random.Range(0,10) < 2){
		//	audioSource_voice.clip = jump_grunts[Random.Range(0, jump_grunts.Length)];
		//	audioSource_voice.volume = .15f;
		//	audioSource_voice.Play();
		//}
		while(jumpCounter < 2){
			audioSource_jump_land.clip = footsteps_dirt[jumpCounter];
			audioSource_jump_land.volume += 0.20f;
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
		if (fallTimeMult < 0.25f) fallTimeMult = 0.25f;

		if (fallTimeMult > 1f){ 
			fallTimeMult = 1f;
			audioSource_voice.clip = landing_grunts[Random.Range(0, landing_grunts.Length)];
			audioSource_voice.volume = 0.35f;
			audioSource_voice.Play();
		}
		while (landCounter < 2) {
			audioSource.mute = true;
			audioSource_jump_land.clip = footsteps_dirt[Random.Range(0, footsteps_dirt.Length)];
			audioSource_jump_land.volume = fallTimeMult;
			audioSource_jump_land.Play();
			yield return new WaitForSeconds(0.025f);
			landCounter++;
		}
		yield return null;
	}

	IEnumerator swing_sword() {
		audioSource_sword.clip = attack_orientation[Random.Range(0, attack_orientation.Length)];
		audioSource_sword.volume = 0.40f;
		audioSource_sword.Play();
		yield return null;
	}
}
