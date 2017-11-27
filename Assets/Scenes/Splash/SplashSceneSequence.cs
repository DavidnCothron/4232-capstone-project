using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashSceneSequence : MonoBehaviour {

	public static int SceneNumber;
	// Use this for initialization
	void Start () {
		if(SceneNumber == 0) {
			StartCoroutine(ToSplashTwo());
		}
		else if(SceneNumber == 1){
			StartCoroutine(ToMainMenu());
		}
		
	}

	IEnumerator ToSplashTwo(){
		yield return new WaitForSeconds(5);
		SceneNumber = 1;
		SceneManager.LoadScene(1);
	}

	IEnumerator ToMainMenu(){
		yield return new WaitForSeconds(5);
		SceneNumber = 2;
		SceneManager.LoadScene(2);
	}
}
