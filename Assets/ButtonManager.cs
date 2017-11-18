using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

	[SerializeField] public Transform PauseCanvas;
	[SerializeField] public Transform MainStartButton;
	[SerializeField] public Transform TutorialButton;
	[SerializeField] public Transform SongOneButton;
	[SerializeField] public Transform SongTwoButton;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			Time.timeScale = 0;
			PauseCanvas.gameObject.SetActive(true);
		}
	}

	public void ResumeGame()
	{
		PauseCanvas.gameObject.SetActive (false);
		Time.timeScale = 1;
	}

	public void QuitGame(string titleScreenName)
	{
		SceneManager.LoadScene (titleScreenName);
	}

	public void ReturnToSaveRoom(){
		
		//TODO need implement save room
		//SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void ExitToWindows()
	{
		Application.Quit ();
	}

	public void StartGameBtn(string newGameLevel)
	{
		MainStartButton.gameObject.SetActive (false);
		//TutorialButton.gameObject.SetActive(false);

		SongOneButton.gameObject.SetActive(true);
		SongTwoButton.gameObject.SetActive(true);
	}


	
	
}