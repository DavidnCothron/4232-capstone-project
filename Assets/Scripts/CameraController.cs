using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Vector3 velocity;
	[SerializeField] private GameObject player;
	[SerializeField] private float smoothTimeY = 0.2f;
	[SerializeField] private float smoothTimeX = 0.2f;

	// Update is called once per frame
	void FixedUpdate () {
		float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY) + .05f;

		transform.position = new Vector3 (posX, posY, transform.position.z);
	}
}
