using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

	//player(with controller) and camera(with controller) objects 
	[SerializeField] private GameObject player;
	[SerializeField] private Camera camera; 
	[SerializeField] private CameraController cameraCont;
	[SerializeField] private PlayerController playerCont;

	//Smooth Damp variables
	private Vector3 velocity;
	private float posX;
	private float posY;
	[SerializeField] private float smoothTimeY = 0.2f;
	[SerializeField] private float smoothTimeX = 0.2f;

	//Viewport Points
	public Vector3 VpTopRight; 
	public Vector3 VpBottomRight;
	public Vector3 VpTopLeft;
	public Vector3 VpBottomLeft;

	//Reference to walls of a given room
	[SerializeField] private GameObject LeftWall;
	[SerializeField] private GameObject RightWall;
	[SerializeField] private GameObject TopWall;
	[SerializeField] private GameObject BottomWall;

	//get bounds from above walls to initialize these *note- only topRight and Bottom left points
	//are needed in order to calculate the bounds of a rectangular room
	[SerializeField] private Vector3 wallBoundBL;
	[SerializeField] private Vector3 wallBoundTR;

	[SerializeField] private string currentRoomID;
	[SerializeField] private Image cameraFadeImage;
	[SerializeField] private float fadeSpeed;

	//initializes Pixel Per Unit for the camera
	//initializes camera/player and their controllers
	void Awake(){
		UnityDepth.instance.FindUnityDepth ();
		if (UnityDepth.instance.PPU == null) {
			UnityDepth.instance.PPU = 32;
		}
		camera = this.GetComponent<Camera> ();
		player = GameObject.FindWithTag ("Player");

		playerCont = player.GetComponent<PlayerController>();
		cameraCont = this.GetComponent<CameraController>();

		//If the cameraFadeImage has been set for the camera
		if (cameraFadeImage != null)
			cameraFadeImage.rectTransform.localScale = new Vector2 (Screen.width, Screen.height);
	}

	//gets the four corners of the room on start *note this will need to change in the
	//future to incoperate player moving from room to room
	void Start(){
		getRoomCorners ();
	}

	void FixedUpdate () {
		detectRoom ();
		//basically lerps the camera closer to its target position over time
		posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY) + .05f;

		//calculates the distance the camera should be from the bounding walls and sets that as the new position
		transform.position = new Vector3(
			/*x*/	Mathf.Clamp(posX, wallBoundBL.x + Mathf.Abs(this.transform.position.x - cameraCont.VpBottomLeft.x), wallBoundTR.x - Mathf.Abs(this.transform.position.x - cameraCont.VpTopRight.x)),
			/*y*/	Mathf.Clamp(posY, wallBoundBL.y + Mathf.Abs(this.transform.position.y - cameraCont.VpBottomLeft.y), wallBoundTR.y - Mathf.Abs(this.transform.position.y - cameraCont.VpTopRight.y)),
			/*z*/	camera.transform.position.z
		);

		//gets the worldpoint coordinates of the camera viewport at a particular z distance
		//the camera current lies at -16.875z in world space so in order to get 0z (where the main game/sprite layers are)
		//the z coordinate in the VieportToWorldPoint asks for a point 16.875 units in front of the camera
		VpTopRight = camera.ViewportToWorldPoint (new Vector3 (1, 1, Mathf.Abs(camera.transform.position.z)));
		VpBottomRight = camera.ViewportToWorldPoint (new Vector3 (1, 0, Mathf.Abs(camera.transform.position.z)));
		VpTopLeft = camera.ViewportToWorldPoint (new Vector3 (0, 1, Mathf.Abs(camera.transform.position.z)));
		VpBottomLeft = camera.ViewportToWorldPoint (new Vector3 (0, 0, Mathf.Abs(camera.transform.position.z)));
	}

	//calculates the four corners of a room based on the bounding walls of that room
	void getRoomCorners(){
		wallBoundBL = new Vector3((BottomWall.transform.position.x - (BottomWall.transform.localScale.x)/2), (LeftWall.transform.position.y - (LeftWall.transform.localScale.x)/2), 0);
		wallBoundTR = new Vector3((TopWall.transform.position.x + (TopWall.transform.localScale.x)/2), (RightWall.transform.position.y + (RightWall.transform.localScale.x)/2), 0);
	}

	//draws points of the viewport corners in world space 
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (VpTopRight, .5f);
		Gizmos.DrawSphere (VpBottomRight, .5f);
		Gizmos.DrawSphere (VpTopLeft, .5f);
		Gizmos.DrawSphere (VpBottomLeft, .5f);

		Gizmos.color = Color.green;
		Gizmos.DrawSphere (wallBoundBL, .5f);
		Gizmos.DrawSphere (wallBoundTR, .5f);
	}

	//Detects the room where the player currently is.
	void detectRoom() {
		Vector3 rayStart = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z - 5f);
		LayerMask room = (1 << LayerMask.NameToLayer ("RoomBackground"));
		RaycastHit2D hit = Physics2D.Raycast (rayStart, Vector3.forward, 10f, room);
		if (hit != null && hit.collider != null) {
			if (hit.collider.GetComponentInParent<RoomController> ().getRoomID () != currentRoomID) {
				GameObject currentRoom = hit.collider.gameObject.transform.parent.gameObject;
				currentRoomID = currentRoom.GetComponent<RoomController> ().getRoomID ();
				GameObject[] objs = GameControl.control.GetChildGameObjects (currentRoom);
				GameObject obj = GameControl.control.FindGameObjectFromArray (objs, "CameraBounds");
				objs = GameControl.control.GetChildGameObjects (obj);
				if (objs.Length == 4)
					setBoundingWalls (objs);
			}
		}
	}

	//Set the bounding walls from the currently detected room
	void setBoundingWalls (GameObject[] BoundingWalls) {
		foreach (GameObject g in BoundingWalls) {
			if (g.CompareTag ("LeftBoundingWall"))
				LeftWall = g;
			if (g.CompareTag ("RightBoundingWall"))
				RightWall = g;
			if (g.CompareTag ("TopBoundingWall"))
				TopWall = g;
			if (g.CompareTag ("BottomBoundingWall"))
				BottomWall = g;
		}
	}

	//Coroutine to fade the screen to black
	public IEnumerator fadeToBlack() {
		cameraFadeImage.enabled = true;
		while (true) {
			cameraFadeImage.color = Color.Lerp (cameraFadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
			if (cameraFadeImage.color.a >= 0.95f)
				yield break;
			else
				yield return null;
		}
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
	}

	//Coroutine to fade the screen to clear
	public IEnumerator fadeToClear() {
		cameraFadeImage.enabled = true;
		while (true) {
			cameraFadeImage.color = Color.Lerp (cameraFadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);
			if (cameraFadeImage.color.a <= 0.05f)
				yield break;
			else
				yield return null;
		}
	}
}
