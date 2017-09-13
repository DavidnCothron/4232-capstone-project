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
	[SerializeField] private GameObject LeftWall;
	[SerializeField] private GameObject RightWall;
	[SerializeField] private GameObject TopWall;
	[SerializeField] private GameObject BottomWall;

	[SerializeField] private Vector3 boundBL;
	[SerializeField] private Vector3 boundTL; 
	[SerializeField] private Vector3 boundBR;
	[SerializeField] private Vector3 boundTR;
	
	// [SerializeField] private Collider2D boundColliderBL;
	// [SerializeField] private Collider2D boundColliderTL;
	// [SerializeField] private Collider2D boundColliderBR;
	// [SerializeField] private Collider2D boundColliderTR;


	void Awake(){
		UnityDepth.instance.FindUnityDepth ();
		if (UnityDepth.instance.PPU == null) {
			UnityDepth.instance.PPU = 32;
		}

		playerCont = player.GetComponent<PlayerController>();
		cameraCont = camera.GetComponent<CameraController>();
	}

	 void Start(){		
	// 	boundColliderBL.transform.position = cameraCont.bottomLeft;
	// 	boundColliderTL.transform.position  = cameraCont.topLeft;
	// 	boundColliderBR.transform.position  = cameraCont.bottomRight;
	// 	boundColliderTR.transform.position  = cameraCont.topRight;
		getRoomCorners();
	 }
	
	void FixedUpdate () {
		
		posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY) + .05f;
		transform.position = new Vector3(posX, posY, 0);


		// boundColliderBL.transform.position = cameraCont.bottomLeft;
		// boundColliderTL.transform.position  = cameraCont.topLeft;
		// boundColliderBR.transform.position  = cameraCont.bottomRight;
		// boundColliderTR.transform.position  = cameraCont.topRight;

	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.green;
		Gizmos.DrawSphere (boundBL, .5f);
		Gizmos.DrawSphere (boundTL, .5f);
		Gizmos.DrawSphere (boundBR, .5f);
		Gizmos.DrawSphere (boundTR, .5f);
	}

	void getRoomCorners(){
		boundBL = new Vector3((BottomWall.transform.position.x - (BottomWall.transform.localScale.x)/2), (LeftWall.transform.position.y-(LeftWall.transform.localScale.x)/2), 0);
		boundTL = new Vector3((TopWall.transform.position.x - (BottomWall.transform.localScale.x)/2), (LeftWall.transform.position.y+(LeftWall.transform.localScale.x)/2), 0);
		boundBR = new Vector3((BottomWall.transform.position.x + (BottomWall.transform.localScale.x)/2), (RightWall.transform.position.y-(RightWall.transform.localScale.x)/2), 0);
		boundTR = new Vector3((TopWall.transform.position.x + (TopWall.transform.localScale.x)/2), (RightWall.transform.position.y+(RightWall.transform.localScale.x)/2), 0);
	}

}
