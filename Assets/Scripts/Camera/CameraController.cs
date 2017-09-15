using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Camera camera;
	public Vector3 topRight;
	public Vector3 bottomRight;
	public Vector3 topLeft;
	public Vector3 bottomLeft;


	void Awake(){
		UnityDepth.instance.FindUnityDepth ();
		if (UnityDepth.instance.PPU == null) {
			UnityDepth.instance.PPU = 32;
		}
		camera = Camera.FindObjectOfType (typeof(Camera)) as Camera;
	}


	void FixedUpdate () {
		topRight = camera.ViewportToWorldPoint (new Vector3 (1, 1, 16.875f));
		bottomRight = camera.ViewportToWorldPoint (new Vector3 (1, 0, 16.875f));
		topLeft = camera.ViewportToWorldPoint (new Vector3 (0, 1, 16.875f));
		bottomLeft = camera.ViewportToWorldPoint (new Vector3 (0, 0, 16.875f));
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (topRight, .5f);
		Gizmos.DrawSphere (bottomRight, .5f);
		Gizmos.DrawSphere (topLeft, .5f);
		Gizmos.DrawSphere (bottomLeft, .5f);
	}

}
