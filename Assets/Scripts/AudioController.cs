using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioController : MonoBehaviour {
	
	#region current
	private AudioSource currentIntroSource, currentLoopSource;
	private AudioMixerSnapshot currentIntroSnap, currentLoopSnap;
	#endregion


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
	private bool fightBoss, playOnTrue;

	private IEnumerator transitionCo, playMusicCo;

	void Awake() {
		playOnTrue = true;
	}
	
	void Start () {
		fightBoss = false;
		switchToIntro_title = 0f;
		//StartCoroutine(fightBossMusic());
	}

	void Update () {
		switchToIntro_title -= Time.deltaTime;
		currentRoom = GameControl.control.getCurrentRoom();
		if (currentRoom != null && currentRoom.getRoomID() != currentRoomID){
			currentRoomID = currentRoom.getRoomID();

			if (currentRoom.getMusicType() != currentMusicType) {
				
				currentMusicType = currentRoom.getMusicType();
				if (currentMusicType == null || !playOnTrue) 
				{
					musicInactive.TransitionTo(1f);
				}

				if (currentMusicType == "title") 
				{
					currentIntroSource = intro_titleSource;
					currentLoopSource = loop_titleSource;
					currentIntroSnap = title_intro;
					currentLoopSnap = title_loop;
					
				}
				if (currentMusicType == "roam")
				{
					currentIntroSource = intro_roamSource;
					currentLoopSource = loop_roamSource;
					currentIntroSnap = roam_intro;
					currentLoopSnap = roam_loop;
				}
				if (currentMusicType == "key")
				{
					currentIntroSource = intro_bossKeySource;
					currentLoopSource = loop_bossKeySource;
					currentIntroSnap = BossKey_intro;
					currentLoopSnap = BossKey_loop;
				}
				if (currentMusicType == "boss")
				{
					currentIntroSource = intro_bossFightSource;
					currentLoopSource = loop_bossFightSource;
					currentIntroSnap = BossFight_intro;
					currentLoopSnap = BossFight_loop;
				}

				if (playMusicCo != null) StopCoroutine(playMusicCo);
				playMusicCo = playMusic();
				StartCoroutine(playMusicCo);
			}
		}
	}

	public void setFightBoss(bool b) {
		fightBoss = b;
	}

	public void setPlayOnTrue(bool b) {
		playOnTrue = b;
	}

	IEnumerator playMusic() {
		while(true) {
			if (playOnTrue) {
				currentIntroSource.Stop();
				currentLoopSource.Stop();
				currentIntroSnap.TransitionTo(0.5f);
				currentIntroSource.Play();
				currentLoopSource.Play();
				if (transitionCo != null) StopCoroutine(transitionCo);
				transitionCo = transitionToLoop(currentIntroSource.clip.length, currentMusicType);
				StartCoroutine(transitionCo);
				break;
			}
			yield return new WaitForFixedUpdate();
		}
		yield return null;
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
