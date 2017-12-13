using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioController : MonoBehaviour {
	[SerializeField]private AudioSource intro_titleSource, loop_titleSource;
	[SerializeField]private AudioSource intro_roamSource, loop_roamSource;
	[SerializeField]private AudioSource intro_bossKeySource, loop_bossKeySource;
	[SerializeField]private AudioSource intro_bossFightSource, loop_bossFightSource;
	[SerializeField] private AudioMixerSnapshot title_intro, title_loop;
	[SerializeField] private AudioMixerSnapshot roam_intro, roam_loop;
	[SerializeField] private AudioMixerSnapshot BossKey_intro, BossKey_loop;
	[SerializeField] private AudioMixerSnapshot BossFight_intro, BossFight_loop;
	[SerializeField] private AudioMixerSnapshot musicInactive;
	[SerializeField] private AudioClip[] title_music, roam_music, bossFight_music, bossKey_music;
	private RoomController currentRoom;
	private float switchToIntro_title;
	[SerializeField] private string currentRoomID, currentMusicType;
	private bool fightBoss;

	//CHECK COROUTINE EXECUTION FOR NUMBER OF COROUTINES RUNNING
	// Use this for initialization
	void Start () {
		fightBoss = false;
		switchToIntro_title = 0f;
		StartCoroutine(fightBossMusic());
	}
	
	// Update is called once per frame
	void Update () {
		switchToIntro_title -= Time.deltaTime;
		currentRoom = GameControl.control.getCurrentRoom();
		if (currentRoom != null && currentRoom.getRoomID() != currentRoomID){
			currentRoomID = currentRoom.getRoomID();
			if (currentRoom.getMusicType() != currentMusicType) {
				
				currentMusicType = currentRoom.getMusicType();

				if (currentMusicType == "boss" && !fightBoss) 
				{
					musicInactive.TransitionTo(1f);
				}
				if (currentMusicType == "title") 
				{
					if (!intro_titleSource.isPlaying){ 
						intro_titleSource.Play();
						loop_titleSource.Play();
					}
					intro_titleSource.Stop();
					loop_titleSource.Stop();
					title_intro.TransitionTo(0.5f);
					loop_titleSource.Play();
					intro_titleSource.Play();
					StartCoroutine(transitionToLoop(intro_titleSource.clip.length, currentMusicType));
				}
				if (currentMusicType == "roam")
				{
					if (!intro_roamSource.isPlaying) {
						intro_roamSource.Play();
						loop_roamSource.Play();
					}
					intro_roamSource.Stop();
					loop_roamSource.Stop();
					roam_intro.TransitionTo(0.5f);
					intro_roamSource.Play();
					loop_roamSource.Play();
					StartCoroutine(transitionToLoop(intro_roamSource.clip.length, currentMusicType));
				}
				if (currentMusicType == "key")
				{
					if (!intro_bossKeySource.isPlaying) {
						intro_bossKeySource.Play();
						loop_bossKeySource.Play();
					}
					intro_bossKeySource.Stop();
					loop_bossKeySource.Stop();
					BossKey_intro.TransitionTo(0.5f);
					intro_bossKeySource.Play();
					loop_bossKeySource.Play();
					StartCoroutine(transitionToLoop(intro_bossKeySource.clip.length, currentMusicType));
				}
				if (currentMusicType == "boss" && fightBoss)
				{
					if (!intro_bossFightSource.isPlaying) {
						intro_bossFightSource.Play();
						loop_bossFightSource.Play();
					}
					intro_bossFightSource.Stop();
					loop_bossFightSource.Stop();
					BossFight_intro.TransitionTo(0.5f);
					intro_bossFightSource.Play();
					loop_bossFightSource.Play();
					StartCoroutine(transitionToLoop(intro_bossFightSource.clip.length, currentMusicType));
				}
			}
		}
	}

	public void setFightBoss(bool b) {
		fightBoss = b;
	}

	IEnumerator fightBossMusic() {
		while(true) {
			if (fightBoss) {
				intro_bossFightSource.Stop();
				intro_bossFightSource.Play();
				loop_bossFightSource.Stop();
				loop_bossFightSource.Play();
				BossFight_intro.TransitionTo(0.01f);
				StartCoroutine(transitionToLoop(intro_bossFightSource.clip.length, currentMusicType));
				break;
			}
			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}
	
	IEnumerator transitionToLoop(float length, string type) {
		float timeGone = 0f;
		while (true) {
			timeGone += Time.fixedDeltaTime;
			if (type != currentMusicType) {
				//Debug.Log("stopping transition coroutine");
				//break;
				timeGone = 0f;
			}
			if (timeGone >= length) 
			{
				if (type == "title" && type == currentMusicType) {
					title_loop.TransitionTo(0.5f);
					break;
				}
				if (type == "roam" && type == currentMusicType) {
					roam_loop.TransitionTo(0.5f);
					break;
				}
				if (type == "key" && type == currentMusicType) {
					BossKey_loop.TransitionTo(0.5f);
					break;
				}
				if (type == "boss" && type == currentMusicType) {
					BossFight_loop.TransitionTo(0.5f);
					break;
				}
			}
			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}
}
