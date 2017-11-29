using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	bool paused = false;
	[SerializeField] public Transform PauseCanvas;
	[SerializeField] public string titleScene;
	[SerializeField] public Transform MainStartButton;
	
	void Start()
    {	
		PauseCanvas.gameObject.SetActive(true);
		Button ContinueButton = GameObject.Find("Continue").GetComponent<Button>();
		Button ReloadSaveButton = GameObject.Find("ReloadSave").GetComponent<Button>();
		Button QuitButton = GameObject.Find("Quit").GetComponent<Button>();
        ContinueButton.onClick.AddListener(ResumeGame);
		ReloadSaveButton.onClick.AddListener(ReturnToSaveRoom);
		QuitButton.onClick.AddListener(QuitGame);
		PauseCanvas.gameObject.SetActive(false);
    }

	void Update()
	{
		if(SceneManager.GetActiveScene().name != "Title_Scene"){
			if(Input.GetKeyDown(KeyCode.Escape) && !paused){//press escape to pause
				PauseGame();
			}else if(Input.GetKeyDown(KeyCode.Escape) && paused){//press escape to unpause
				ResumeGame();
			}
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

	public void QuitGame()//quits to the title scene, set in inspector
	{
		paused = false;
		PauseCanvas.gameObject.SetActive (false);
		GameControl.control.setFadeImage(true);//sets the 'cameraFadeImage' object to active otherwise it cannot be found in the hierarchy when title scene is loaded
		Time.timeScale = 1;
		SceneManager.LoadScene ("Title_Scene");
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