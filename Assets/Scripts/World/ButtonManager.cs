using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	bool paused = false;
	bool death = false;
	[SerializeField] public Transform PauseCanvas;
	[SerializeField] public Transform DeathCanvas;
	[SerializeField] public string titleScene;
	[SerializeField] public Transform MainStartButton;
	AreaControl AreaControl;
	
	void Start()
    {	
		PauseCanvas.gameObject.SetActive(true);
		Button PauseContinueButton = GameObject.Find("Continue").GetComponent<Button>();
		Button PauseReloadSaveButton = GameObject.Find("ReloadSave").GetComponent<Button>();
		Button PauseQuitButton = GameObject.Find("Quit").GetComponent<Button>();
        PauseContinueButton.onClick.AddListener(ResumeGame);
		PauseReloadSaveButton.onClick.AddListener(ReturnToSaveRoom);
		PauseQuitButton.onClick.AddListener(QuitGame);
		PauseCanvas.gameObject.SetActive(false);

		DeathCanvas.gameObject.SetActive(true);
		Button DeathContinueButton = GameObject.Find("Continue").GetComponent<Button>();
		Button DeathQuitButton = GameObject.Find("Quit").GetComponent<Button>();
        DeathContinueButton.onClick.AddListener(ReturnToLevelStart);
		DeathQuitButton.onClick.AddListener(QuitGame);
		DeathCanvas.gameObject.SetActive(false);


		AreaControl = GameControl.control.getAreaControl();
    }

	void Update()
	{		
		if(SceneManager.GetActiveScene().name != "Title_Scene"){
			if(!death){
				if(Input.GetKeyDown(KeyCode.Escape) && !paused){//press escape to pause
					PauseGame();
				}else if(Input.GetKeyDown(KeyCode.Escape) && paused){//press escape to unpause
					ResumeGame();
				}
			}
		}		
	}

	public void PauseGame()//pauses time and turns on the pause menu
	{
		paused = true;
		PauseCanvas.gameObject.SetActive(true);
		GameControl.control.SetPlayerMeleeActivity(false);		
		Time.timeScale = 0;		
	}	

	public void ResumeGame()//unpauses time and turns off the pause menu
	{
		paused = false;		
		Time.timeScale = 1;
		GameControl.control.SetPlayerMeleeActivity(true);
		PauseCanvas.gameObject.SetActive (false);
	}

	public void DeathMenuActive(){
		death = true;
		DeathCanvas.gameObject.SetActive(true);
		Time.timeScale = 0;
		//Debug.Log(DeathCanvas.gameObject.activeInHierarchy);
	}

	public void DeathMenuInactive(){
		Time.timeScale = 1;
		death = false;			
		DeathCanvas.gameObject.SetActive(false);
		//Debug.Log(DeathCanvas.gameObject.activeInHierarchy);
	}

	public void QuitGame()//quits to the title scene, set in inspector
	{
		paused = false;
		Time.timeScale = 1;
		PauseCanvas.gameObject.SetActive (false);
		DeathMenuInactive();
		GameControl.control.setFadeImage(true);//sets the 'cameraFadeImage' object to active otherwise it cannot be found in the hierarchy when title scene is loaded
		Debug.Log("reset player health");
		GameControl.control.ResetPlayerHealth();
		SceneManager.LoadScene ("Title_Scene");
	}

	public void ReturnToSaveRoom(){//returns the player to the last known save room and reloads their last save

		//this will eventually need logic to handle multiple areas when there is more than one level
		PauseCanvas.gameObject.SetActive(false);
		Time.timeScale = 1;
		StartCoroutine(GameControl.control.TransitionToNewRoom(GameControl.control.lastSaveRoom));
	}

	public void ReturnToLevelStart(){
		Time.timeScale = 1;
		DeathCanvas.gameObject.SetActive(false);
		StartCoroutine(GameControl.control.TransitionToNewRoom(AreaControl.startingRoom));
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