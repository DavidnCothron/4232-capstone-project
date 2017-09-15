using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContainer : MonoBehaviour {

	private Vector3 velocity;
	private CameraController cameraCont;
	private PlayerController playerCont;
	private float posX;
	private float posY;
	[SerializeField] private GameObject player;
	[SerializeField] private Camera camera;	
	[SerializeField] private float smoothTimeY = 0.2f;
	[SerializeField] private float smoothTimeX = 0.2f;
	//walls of the room, set in inspector
	[SerializeField] private GameObject LeftWall;
	[SerializeField] private GameObject RightWall;
	[SerializeField] private GameObject TopWall;
	[SerializeField] private GameObject BottomWall;

	//
	[SerializeField] private Vector3 boundBL;
	[SerializeField] private Vector3 boundTR;

	void Awake(){
		UnityDepth.instance.FindUnityDepth ();
		if (UnityDepth.instance.PPU == null) {
			UnityDepth.instance.PPU = 32;
		}

		playerCont = player.GetComponent<PlayerController>();
		cameraCont = camera.GetComponent<CameraController>();
	}

	 void Start(){
		getRoomCorners();
	 }
	
	void FixedUpdate () {
		posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY) + .05f;
		transform.position = new Vector3(
	/*x*/	Mathf.Clamp(posX, boundBL.x + Mathf.Abs(this.transform.position.x - cameraCont.bottomLeft.x), boundTR.x - Mathf.Abs(this.transform.position.x - cameraCont.topRight.x)),
	/*y*/	Mathf.Clamp(posY, boundBL.y + Mathf.Abs(this.transform.position.y - cameraCont.bottomLeft.y), boundTR.y - Mathf.Abs(this.transform.position.y - cameraCont.topRight.y)),
	/*z*/	0f
		);


	}

	void getRoomCorners(){
		boundBL = new Vector3((BottomWall.transform.position.x - (BottomWall.transform.localScale.x)/2), (LeftWall.transform.position.y-(LeftWall.transform.localScale.x)/2), 0);
		boundTR = new Vector3((TopWall.transform.position.x + (TopWall.transform.localScale.x)/2), (RightWall.transform.position.y+(RightWall.transform.localScale.x)/2), 0);
	}

}
