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
	private bool inRoom;

	//Smooth Damp variables
	private Vector3 velocity;
	private float posX;
	private float posY;
	[SerializeField] private float smoothTimeY = .03f;
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

	private RoomController currentRoomController;
	private LayerMask room;
	private Vector3[] roomCorners;
	[SerializeField] private string currentRoomID;
	[SerializeField] private Image cameraFadeImage;
	[SerializeField] private float fadeSpeed;
	private RectTransform boundingTransform;

	private IEnumerator checkForExitCo;

	#region detectRoom
		private Vector3 rayStart;
		private RaycastHit2D roomHit;
	#endregion

	//initializes Pixel Per Unit for the camera
	//initializes camera/player and their controllers
	void Awake(){
		room = (1<<LayerMask.NameToLayer("RoomBackground"));
		roomCorners = new Vector3[4];

		if (UnityDepth.instance.PPU == 0f) {
			UnityDepth.instance.PPU = 32;
		}

		camera = this.GetComponent<Camera> ();
		player = GameObject.FindWithTag ("Player");

		playerCont = player.GetComponent<PlayerController>();
		cameraCont = this.GetComponent<CameraController>();
		cameraFadeImage = GameObject.Find("cameraFadeImage").GetComponent<Image>();

		if (cameraFadeImage != null){
			cameraFadeImage.rectTransform.localScale = new Vector2 (Screen.width, Screen.height);
			setActiveFadeImage(false);
		}
	}

	void Start(){
		camera.transform.SetPositionAndRotation(new Vector3(0f,0f,-UnityDepth.instance.unityDepth2), Quaternion.identity);
	}

	void FixedUpdate () {
		detectRoom ();
		//basically lerps the camera closer to its target position over time
		posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y + 1.5f, ref velocity.y, smoothTimeY) + .05f;

		//calculates the distance the camera should be from the bounding walls and sets that as the new position
		transform.position = new Vector3(
			/*x*/	Mathf.Clamp(posX, wallBoundBL.x + Mathf.Abs(this.transform.position.x - cameraCont.VpBottomLeft.x), wallBoundTR.x - Mathf.Abs(this.transform.position.x - cameraCont.VpTopRight.x)),
			/*y*/	Mathf.Clamp(posY, wallBoundBL.y + Mathf.Abs(this.transform.position.y - cameraCont.VpBottomLeft.y), wallBoundTR.y - Mathf.Abs(this.transform.position.y - cameraCont.VpTopRight.y)),
			/*z*/	camera.transform.position.z
		);

		//gets the worldpoint coordinates of the camera viewport at a particular z distance
		VpTopRight = camera.ViewportToWorldPoint (new Vector3 (1, 1, Mathf.Abs(camera.transform.position.z)));
		VpBottomRight = camera.ViewportToWorldPoint (new Vector3 (1, 0, Mathf.Abs(camera.transform.position.z)));
		VpTopLeft = camera.ViewportToWorldPoint (new Vector3 (0, 1, Mathf.Abs(camera.transform.position.z)));
		VpBottomLeft = camera.ViewportToWorldPoint (new Vector3 (0, 0, Mathf.Abs(camera.transform.position.z)));
	}

	public void setActiveFadeImage(bool value){
		cameraFadeImage.gameObject.SetActive(value);
	}

	//calculates the four corners of a room based on the bounding walls of that room
	void getRoomCorners(){
		//wallBoundBL = new Vector3((BottomWall.transform.position.x - (BottomWall.transform.localScale.x)/2), (LeftWall.transform.position.y - (LeftWall.transform.localScale.x)/2), 0);
		//wallBoundTR = new Vector3((TopWall.transform.position.x + (TopWall.transform.localScale.x)/2), (RightWall.transform.position.y + (RightWall.transform.localScale.x)/2), 0);

		wallBoundBL = new Vector3((-(roomCorners[3].x - roomCorners[0].x)/2),-((roomCorners[1].y - roomCorners[0].y)/2), 0f) + boundingTransform.position;
		wallBoundTR = new Vector3(((roomCorners[2].x - roomCorners[1].x)/2), ((roomCorners[2].y - roomCorners[3].y)/2), 0f) + boundingTransform.position;
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

	/// <summary>
	/// Detects the room where the player currently is and updates bounding walls for the camera.
	/// </summary>
	void detectRoom() {
		//Cast ray at player with a LayerMask named "RoomBackground."
		rayStart = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z - 5f);
		roomHit = Physics2D.Raycast (rayStart, Vector3.forward, 10f, room);

		if (roomHit  && roomHit.collider != null)
		{
			currentRoomController = roomHit.collider.GetComponentInParent<RoomController>();
			boundingTransform = roomHit.collider.gameObject.GetComponent<RectTransform>();
			inRoom = true;
			
			if (currentRoomController.getRoomID () != currentRoomID) 
			{
				boundingTransform.GetWorldCorners(roomCorners);
				getRoomCorners ();

				//As it is current set, 'RoomBackground' object must be directly childed to the 'Room' object.
				GameControl.control.setCurrentRoom(currentRoomController);
				currentRoomID = currentRoomController.getRoomID ();

				if (checkForExitCo != null) StopCoroutine(checkForExitCo);
				checkForExitCo = currentRoomController.checkForPlayerExit();
				StartCoroutine(checkForExitCo);
			}
		} else 
		{
			inRoom = false;
		}
	}

	/// <summary>
	/// Sets the bounding walls of the newly detected room.
	/// </summary>
	/// <param name="BoundingWalls">Bounding walls.</param>
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

	/// <summary>
	/// Fades an image covering the camera viewport to black.
	/// </summary>
	/// <returns>The to black.</returns>
	public IEnumerator fadeToBlack() {
		setActiveFadeImage(true);
		cameraFadeImage.enabled = true;
		while (true) {
			cameraFadeImage.color = Color.Lerp (cameraFadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
			if (cameraFadeImage.color.a >= 0.97f) {
				cameraFadeImage.color = Color.black;
				yield break;
			} else
				yield return null;
		}
		yield return new WaitForSeconds (GameControl.control.getRoomTransTime ());
	}

	///<summary>
	/// Method that sets the cameraFadeImage to black. Used for area transitioning in
	///</summary>
	public void setToBlack(){
		setActiveFadeImage(true);
		cameraFadeImage.color = Color.black;
	}

	/// <summary>
	/// Fades an image covering the camera viewport to clear.
	/// </summary>
	/// <returns>The to clear.</returns>
	public IEnumerator fadeToClear() {
		setActiveFadeImage(true);
		cameraFadeImage.enabled = true;
		while (true) {
			cameraFadeImage.color = Color.Lerp (cameraFadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);
			if (cameraFadeImage.color.a <= 0.03f) {
				cameraFadeImage.color = Color.clear;
				setActiveFadeImage(false);
				yield break;				
			}
			else
				yield return null;
		}
	}

	public bool getInRoom() {
		return inRoom;
	}
}
