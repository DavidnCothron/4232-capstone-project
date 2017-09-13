using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContainer : MonoBehaviour {

	private Vector3 velocity;
	[SerializeField] private GameObject player;
	[SerializeField] private Camera camera;
	private CameraController cameraCont;
	private PlayerController playerCont;
	[SerializeField] private float smoothTimeY = 0.2f;
	[SerializeField] private float smoothTimeX = 0.2f;
	private float posX;
	private float posY;
	[SerializeField] private Collider2D boundColliderBL;
	[SerializeField] private Collider2D boundColliderTL;
	[SerializeField] private Collider2D boundColliderBR;
	[SerializeField] private Collider2D boundColliderTR;


	void Awake(){
		UnityDepth.instance.FindUnityDepth ();
		if (UnityDepth.instance.PPU == null) {
			UnityDepth.instance.PPU = 32;
		}

		playerCont = player.GetComponent<PlayerController>();
		cameraCont = camera.GetComponent<CameraController>();
	}

	void Start(){		
		boundColliderBL.transform.position = cameraCont.bottomLeft;
		boundColliderTL.transform.position  = cameraCont.topLeft;
		boundColliderBR.transform.position  = cameraCont.bottomRight;
		boundColliderTR.transform.position  = cameraCont.topRight;
	}
	
	void FixedUpdate () {
		
		posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY) + .05f;
		transform.position = new Vector3(posX, posY, 0);

		boundColliderBL.transform.position = cameraCont.bottomLeft;
		boundColliderTL.transform.position  = cameraCont.topLeft;
		boundColliderBR.transform.position  = cameraCont.bottomRight;
		boundColliderTR.transform.position  = cameraCont.topRight;

	}

	void OnCollisionEnter2D(Collision2D other){
		Debug.Log ("hit");
//			if (other.gameObject.tag != "CameraBounds") {
//				Physics2D.IgnoreCollision (other.collider, boundColliderBL);
//				Physics2D.IgnoreCollision (other.collider, boundColliderTL);
//				Physics2D.IgnoreCollision (other.collider, boundColliderBR);
//				Physics2D.IgnoreCollision (other.collider, boundColliderTR);
//			}


	}

}
