using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContainer : MonoBehaviour {

	private Vector3 velocity;
	[SerializeField] private GameObject player;
	Camera camera;
	private PlayerController playerCont;
	[SerializeField] private float smoothTimeY = 0.2f;
	[SerializeField] private float smoothTimeX = 0.2f;
	private float posX;
	private float posY;


	void Awake(){
		UnityDepth.instance.FindUnityDepth ();
		if (UnityDepth.instance.PPU == null) {
			UnityDepth.instance.PPU = 32;
		}

		playerCont = player.GetComponent<PlayerController>();
	}
	
	void FixedUpdate () {
		posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY) + .05f;
		transform.position = new Vector3(posX, posY, 0);
	}

}
