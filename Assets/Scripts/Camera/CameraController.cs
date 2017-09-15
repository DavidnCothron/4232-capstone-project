using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	//player(and controller) and camera(and controller) objects 
	[SerializeField] private GameObject player;
	private Camera camera; 
	private CameraController cameraCont;
	private PlayerController playerCont;

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

	//initializes Pixel Per Unit for the camera
	//initializes camera/player and their controllers
	void Awake(){
		UnityDepth.instance.FindUnityDepth ();
		if (UnityDepth.instance.PPU == null) {
			UnityDepth.instance.PPU = 32;
		}
		camera = Camera.FindObjectOfType (typeof(Camera)) as Camera;
		player = GameObject.FindWithTag ("Player");

		playerCont = player.GetComponent<PlayerController>();
		cameraCont = camera.GetComponent<CameraController>();
	}

	//gets the four corners of the room on start *note this will need to change in the
	//future to incoperate player moving from room to room
	void Start(){
		getRoomCorners ();
	}

	void FixedUpdate () {
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
		VpTopRight = camera.ViewportToWorldPoint (new Vector3 (1, 1, 16.875f));
		VpBottomRight = camera.ViewportToWorldPoint (new Vector3 (1, 0, 16.875f));
		VpTopLeft = camera.ViewportToWorldPoint (new Vector3 (0, 1, 16.875f));
		VpBottomLeft = camera.ViewportToWorldPoint (new Vector3 (0, 0, 16.875f));
	}

	//calculates the four corners of a room based on the bounding walls of that room
	void getRoomCorners(){
		wallBoundBL = new Vector3((BottomWall.transform.position.x - (BottomWall.transform.localScale.x)/2), (LeftWall.transform.position.y-(LeftWall.transform.localScale.x)/2), 0);
		wallBoundTR = new Vector3((TopWall.transform.position.x + (TopWall.transform.localScale.x)/2), (RightWall.transform.position.y+(RightWall.transform.localScale.x)/2), 0);
	}

	//draws points of the viewport corners in world space 
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (VpTopRight, .5f);
		Gizmos.DrawSphere (VpBottomRight, .5f);
		Gizmos.DrawSphere (VpTopLeft, .5f);
		Gizmos.DrawSphere (VpBottomLeft, .5f);
	}

}
