using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Door : MonoBehaviour {
	[SerializeField] private GameObject cameraPivot;
	[SerializeField] private GameObject playerSpawn;
	[SerializeField] private GameObject playerDestination;
	[SerializeField] private Door other;
	[SerializeField] private bool switchAreaTrigger;
	[SerializeField] private int currentAreaID;
	[SerializeField] private int nextAreaID;
	[SerializeField] private int currentAccessPointID;
	[SerializeField] private int nextAccessPointID;//need to init tuples
	private AreaTransTuple currentArea;
	private AreaTransTuple nextArea;
	[SerializeField] string sceneToLoad;
	
	

	/// <summary>
	/// Gets the camera pivot associated with this door.
	/// </summary>
	/// <returns>The pivot.</returns>
	public GameObject getPivot(){
		return cameraPivot;
	}

	/// <summary>
	/// Gets the spawn associated with this door. The spawn is placed directly behind the door.
	/// </summary>
	/// <returns>The spawn.</returns>
	public GameObject getSpawn() {
		return playerSpawn;
	}

	/// <summary>
	/// Gets the player destination. The destination is placed in front of the door.
	/// </summary>
	/// <returns>The destination.</returns>
	public GameObject getDestination() {
		return playerDestination;
	}

	public void initAreaTuple(){
		currentArea.areaID = currentAreaID;
		currentArea.accessPointID = currentAccessPointID;
		nextArea.areaID = nextAreaID;
		nextArea.accessPointID = nextAccessPointID;
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.tag == "Player" && !c.GetComponent<PlayerPlatformerController>().haltInput && switchAreaTrigger){
			StartCoroutine(areaTransitionOut(c));			
		}	
		else if (c.tag == "Player" && !c.GetComponent<PlayerPlatformerController>().haltInput)
			StartCoroutine (roomTransition (c));
	}

	/// <summary>
	/// Handles the transition out of an area. Called from doors with 'switchAreaTrigger' equal to true in OnTriggerEnter2D
	/// </summary>
	/// <returns>The transition in</returns>
	/// <param name="c">C.</param>
	IEnumerator areaTransitionOut(Collider2D c){
		//Set body type to kinematic to ensure smooth transition (doesn't look right yet)
		c.GetComponent<PlayerPlatformerController> ().haltInput = true;
		c.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		initAreaTuple();
		GameControl.control.fadeImage ("black");
		yield return StartCoroutine (movePlayer (c, playerSpawn.transform.position));
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		GameControl.control.TransitionAreas(currentArea, nextArea);
		SceneManager.LoadScene(sceneToLoad);//swap this for a configurable option for any door
	}

	/// <summary>
	/// Handles the transition into a new area. Called from AreaControl for a given scene/area. 
	/// </summary>
	/// <returns>The transition in</returns>
	/// <param name="c">C.</param>
	public IEnumerator areaTransitionIn(Collider2D c){
		c.GetComponent<PlayerPlatformerController> ().haltInput = true;
		c.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		GameControl.control.fadeImage ("startBlack");

		c.transform.position = this.getSpawn ().transform.position;
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		GameControl.control.fadeImage ("");
		yield return StartCoroutine (movePlayer (c, this.getDestination ().transform.position));

		//Resets the RigidbodyType2D to Dynamic and returns input control to the player
		c.GetComponent<PlayerPlatformerController> ().haltInput = false;
	}

	/// <summary>
	/// Handles the transition between rooms when the player hits a 'Door' trigger
	/// </summary>
	/// <returns>The transition.</returns>
	/// <param name="c">C.</param>
	IEnumerator roomTransition(Collider2D c) {
		//Set body type to kinematic to ensure smooth transition (doesn't look right yet)
		c.GetComponent<PlayerPlatformerController> ().haltInput = true;
		c.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		//fade to black > move player into door > move player behind other door > fade to clear > move player out of other door
		GameControl.control.fadeImage ("black");
		yield return StartCoroutine (movePlayer (c, playerSpawn.transform.position));
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		c.transform.position = other.getSpawn ().transform.position;
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		GameControl.control.fadeImage ("");
		yield return StartCoroutine (movePlayer (c, other.getDestination ().transform.position));

		//Resets the RigidbodyType2D to Dynamic and returns input control to the player
		c.GetComponent<PlayerPlatformerController> ().haltInput = false;
	}

	/// <summary>
	/// Coroutine called from roomTransition coroutine that physically moves the player
	/// from one room to another using Kinematic Movement
	/// </summary>
	/// <returns>The player.</returns>
	/// <param name="c">C.</param>
	/// <param name="t">T.</param>
	IEnumerator movePlayer(Collider2D c , Vector3 t) {
		KinematicArrive.KinematicSteering steering;
		while (true) {
			c.GetComponent<KinematicArrive> ().setTarget (new Vector3(t.x, c.transform.position.y, t.z));
			steering = c.GetComponent<KinematicArrive> ().getSteering ();
			c.GetComponent<KinematicArrive> ().setOrientations (steering);
			if (c.GetComponent<KinematicArrive> ().getArrived ())
				yield break;
			else
				yield return null;
		}
	}
}
