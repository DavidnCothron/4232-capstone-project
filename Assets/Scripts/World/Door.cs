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
	[SerializeField] private bool isDoorUp, isDoorDown;
	[SerializeField] private int currentAreaID;
	[SerializeField] private int nextAreaID;
	[SerializeField] private int currentAccessPointID;
	[SerializeField] private int nextAccessPointID;//need to init tuples
	private AreaTransTuple currentArea;
	private AreaTransTuple nextArea;
	[SerializeField] string sceneToLoad;
	private LayerMask ignore;
	[SerializeField] private CameraController controller;

	private static IEnumerator roomTransCo, checkExitCo, movePlayerCo, pushPlayerCo;
	
	void Awake() {
		ignore = ~(1<<LayerMask.NameToLayer("RoomBackground")|1<<LayerMask.NameToLayer("Enemy"));
	}

	void Start() {
		controller = Camera.main.GetComponent<CameraController>();
	}

	void FixedUpdate() {
		if(isDoorDown) {
			Debug.DrawRay(this.transform.position + new Vector3 (0f, 2.5f, 0f), transform.right * 5f, Color.red);
			Debug.DrawRay(this.transform.position + new Vector3 (0f, 2.5f, 0f), -transform.right * 5f, Color.blue);
		}
	}

	void DisableCoroutines() {
		if (roomTransCo != null) {
			StopCoroutine(roomTransCo);
		}
		if (checkExitCo != null) {
			StopCoroutine(checkExitCo);
		}
		if (movePlayerCo != null) {
			StopCoroutine(movePlayerCo);
		}
		if (pushPlayerCo != null) {
			StopCoroutine(pushPlayerCo);
		}
	}

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
		else if (c.tag == "Player" && !c.GetComponent<PlayerPlatformerController>().haltInput) {
			if (roomTransCo != null) StopCoroutine(roomTransCo);
			roomTransCo = roomTransition(c);
			StartCoroutine (roomTransCo);
		}
	}

	/// <summary>
	/// Handles the transition out of an area. Called from doors with 'switchAreaTrigger' equal to true in OnTriggerEnter2D
	/// </summary>
	/// <returns>The transition in</returns>
	/// <param name="c">C.</param>
	IEnumerator areaTransitionOut(Collider2D c){
		GameObject player = c.gameObject;
		//Set body type to kinematic to ensure smooth transition (doesn't look right yet)
		var ppc = c.GetComponent<PlayerPlatformerController> ();
		ppc.haltInput = true;
		//ppc.SetGravity(0f);
		c.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		initAreaTuple();
		GameControl.control.fadeImage ("black");
		yield return StartCoroutine (GameControl.control.movePlayer (player, playerSpawn.transform.position));
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
		GameObject player = c.gameObject;
		var ppc = c.GetComponent<PlayerPlatformerController> ();
		ppc.haltInput = true;
		c.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		GameControl.control.fadeImage ("startBlack");

		c.transform.position = this.getSpawn ().transform.position;
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		GameControl.control.fadeImage ("");
		yield return StartCoroutine (GameControl.control.movePlayer(player, this.getDestination ().transform.position));

		//Resets the RigidbodyType2D to Dynamic and returns input control to the player
		c.GetComponent<PlayerPlatformerController> ().haltInput = false;
		//ppc.SetGravity();
	}

	/// <summary>
	/// Handles the transition between rooms when the player hits a 'Door' trigger
	/// </summary>
	/// <returns>The transition.</returns>
	/// <param name="c">C.</param>
	IEnumerator roomTransition(Collider2D c) {
		GameObject player = c.gameObject;
		var ppc = c.GetComponent<PlayerPlatformerController> ();
		ppc.setPhaseTime(0f);
		//ppc.setPhasing(false);
		ppc.haltInput = true;
		int jumpDir = 1; //-1 for left, 1 for right
		bool mustFlipSprite = false;

		if (isDoorDown || isDoorUp) { 
			ppc.SetGravity(0f);

			if (isDoorDown) {
				ppc.SetVelocityOverride(new Vector2(ppc.getVelocity().x, -ppc.getMaxYVelocity()+3));
			}

			if (isDoorUp) {
				ppc.SetVelocityOverride(new Vector2(ppc.getVelocity().x, ppc.getMaxYVelocity()-3));
				RaycastHit2D hitRight = Physics2D.Raycast(other.transform.position + new Vector3(0f,2.5f,0f), transform.right, 5f, ignore);
				RaycastHit2D hitLeft = Physics2D.Raycast(other.transform.position + new Vector3(0f,2.5f,0f), -transform.right, 5f, ignore);
				
				//Debug.Log(ppc.getDirection());
				switch(ppc.getDirection()) {
					case 1:
						if (!hitRight) {
							jumpDir = 1;
						} else {
							jumpDir = -1;
							Debug.Log(hitRight.collider.tag);
							mustFlipSprite = true;
							ppc.setDirection(-1);
						}
						break;
					case -1:
						if (!hitLeft) {
							jumpDir = -1;
						} else {
							jumpDir = 1;
							Debug.Log(hitLeft.collider.tag);
							mustFlipSprite = true;
							ppc.setDirection(1);
						}
						break;
					default:
						Debug.Log("This should never happen. Game detected player face direction as neither left or right");
						break;
				}
			} 
		}

		//fade to black > move player into door > move player behind other door > fade to clear > move player out of other door
		GameControl.control.fadeImage ("black");

		//We need to stop the player velocity if the player is no longer in a room;
		if(checkExitCo != null) StopCoroutine(checkExitCo);
		checkExitCo = checkForRoomExitAndStop(ppc);
		StartCoroutine(checkExitCo);

		if (movePlayerCo != null) StopCoroutine(movePlayerCo);
		movePlayerCo = GameControl.control.movePlayer(player, playerSpawn.transform.position);
		yield return StartCoroutine (movePlayerCo);

		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		c.transform.position = other.getSpawn ().transform.position;
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());

		GameControl.control.fadeImage ("");

		if (!isDoorUp) {
			if (movePlayerCo != null) StopCoroutine(movePlayerCo);
			movePlayerCo = GameControl.control.movePlayer(player, other.getDestination ().transform.position, ppc);
			yield return StartCoroutine (movePlayerCo);
		}

		if (isDoorDown || isDoorUp) {

			ppc.SetGravity(4f);
			if (isDoorUp) {
				if (movePlayerCo != null) StopCoroutine(movePlayerCo);
				if (mustFlipSprite) ppc.flipSprite();
				ppc.SetVelocityOverride(new Vector2(0f, ppc.getMaxYVelocity()-2f));
				if (pushPlayerCo != null) StopCoroutine(pushPlayerCo);
				//other.transform.position.x is added to the target to convert the target to world space.
				pushPlayerCo = GameControl.control.pushPlayer(ppc, Mathf.Sign(jumpDir)*(ppc.maxSpeed*2) + other.transform.position.x);
				yield return StartCoroutine(pushPlayerCo); 
			}
			
			
		}
		c.GetComponent<PlayerPlatformerController> ().haltInput = false;
	}

	IEnumerator checkForRoomExitAndStop(PlayerPlatformerController ppc) {
		while (controller.getInRoom()) {
			yield return new WaitForFixedUpdate();
		}
		ppc.SetVelocityOverride(new Vector2(0f,0f));
		yield return null;
	}
	
}
