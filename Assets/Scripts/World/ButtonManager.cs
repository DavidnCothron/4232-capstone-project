using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	bool paused = false;
	[SerializeField] public Transform PauseCanvas;
	[SerializeField] public Transform MainStartButton;

	void Start()
    {	
		PauseCanvas.gameObject.SetActive(true);
		GameObject ContinueButton = GameObject.Find("Continue");
        Button btn = ContinueButton.GetComponent<Button>();
		Debug.Log(btn);
        btn.onClick.AddListener(delegate {ResumeGame();});
		PauseCanvas.gameObject.SetActive(false);
    }

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && !paused){//press escape to pause
			PauseGame();
		}else if(Input.GetKeyDown(KeyCode.Escape) && paused){//press escape to unpause
			ResumeGame();
		}
	}

	public void PauseGame()//pauses time and turns on the pause menu
	{
		paused = true;
		Time.timeScale = 0;
		PauseCanvas.gameObject.SetActive(true);
	}	

	public void ResumeGame()//unpauses time and turns off the pause menu
	{
		paused = false;
		PauseCanvas.gameObject.SetActive (false);
		Time.timeScale = 1;
	}

	public void QuitGame(string titleScreenName)//quits to the title scene, set in inspector
	{
		SceneManager.LoadScene (titleScreenName);
	}

	public void ReturnToSaveRoom(){//returns the player to the last known save room and reloads their last save

		//this will eventually need logic to handle multiple areas when there is more than one level
		PauseCanvas.gameObject.SetActive(false);
		Time.timeScale = 1;
		StartCoroutine(GameControl.control.TransitionToNewRoom(GameControl.control.lastSaveRoom));
	}

	public void ExitToWindows()//closes the application
	{
		Application.Quit ();
	}

	public void StartGameBtn(string newGameLevel)//not implemented yet
	{
		//MainStartButton.gameObject.SetActive (false);
	}


	
	
}