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
	
	void Awake() {
		ignore = ~(1<<LayerMask.NameToLayer("RoomBackground"));
	}

	void Start() {
		controller = Camera.main.GetComponent<CameraController>();
	}

	void Update() {
		if(isDoorDown) {
			Debug.DrawRay(this.transform.position + new Vector3 (0f, 2f, 0f), transform.right * 2f, Color.red);
			Debug.DrawRay(this.transform.position + new Vector3 (0f, 2f, 0f), -transform.right * 2f, Color.blue);
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
		else if (c.tag == "Player" && !c.GetComponent<PlayerPlatformerController>().haltInput)
			StartCoroutine (roomTransition (c));
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
		ppc.haltInput = true;
		int jumpDir = 1; //-1 for left, 1 for right
		bool mustFlipSprite = false;

		if (isDoorDown || isDoorUp) { 
			ppc.SetGravity(0f);
			if (isDoorDown) {
				ppc.SetVelocityOverride(new Vector2(ppc.getVelocity().x, -ppc.getMaxYVelocity()));
			}
			if (isDoorUp) {
				RaycastHit2D hitRight = Physics2D.Raycast(other.transform.position + new Vector3(0f,2f,0f), transform.right, 2f, ignore);
				RaycastHit2D hitLeft = Physics2D.Raycast(other.transform.position + new Vector3(0f,2f,0f), -transform.right, 2f, ignore);
				switch(ppc.getDirection()) {
					case 1:
						if (!hitRight) {
							jumpDir = 1;
						} else {
							Debug.Log(hitRight.collider.tag);
							mustFlipSprite = true;
							ppc.setDirection(-1);
						}
						break;
					case -1:
						if (!hitLeft) {
							jumpDir = -1;
						} else {
							Debug.Log(hitLeft.collider.tag);
							mustFlipSprite = true;
							ppc.setDirection(1);
						}
						break;
					default:
						Debug.Log("This should never happen. Game detected player velocity.x as neither negative nor positive when moving through an upwards door.");
						break;
				}
			} 
		}

		//fade to black > move player into door > move player behind other door > fade to clear > move player out of other door
		GameControl.control.fadeImage ("black");
		yield return StartCoroutine (GameControl.control.movePlayer(player, playerSpawn.transform.position));
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		c.transform.position = other.getSpawn ().transform.position;
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
		GameControl.control.fadeImage ("");
		if (!isDoorUp) yield return StartCoroutine (GameControl.control.movePlayer(player, other.getDestination ().transform.position, ppc));

		if (isDoorDown || isDoorUp) {

			ppc.SetGravity(4f);
			if (isDoorUp) {
				if (mustFlipSprite) ppc.flipSprite();
				ppc.SetVelocityOverride(new Vector2(0f, ppc.getMaxYVelocity()));
				yield return StartCoroutine(GameControl.control.pushPlayer(ppc, Mathf.Sign(jumpDir)*ppc.maxSpeed)); 
				//StartCoroutine(GameControl.control.pushPlayer(ppc, Mathf.Sign(jumpDir)*ppc.maxSpeed));
			}
			
			
		}
		c.GetComponent<PlayerPlatformerController> ().haltInput = false;
	}


	
}
