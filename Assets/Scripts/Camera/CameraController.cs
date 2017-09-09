using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
	Vector3 topRightEdgeScreen;
	Vector3 downLeftEdgeScreen;
	Plane cameraPositionFixPlane;
	
	
	private Vector3 velocity;
	[SerializeField] private GameObject player;
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
	// Update is called once per frame
	void FixedUpdate () {
		
		if(!playerCont.lockCameraX)
			posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);

		posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY) + .05f;

	    transform.position = new Vector3(posX, posY, -UnityDepth.instance.focusZ);
	}
	
	/*/
	void Update()
	{
		Camera.main.camera.
		topRightEdgeScreen = Camera.main.WorldToScreenPoint(topRightEdge);
		downLeftEdgeScreen = Camera.main.WorldToScreenPoint(downLeftEdge);

		Debug.Log(downLeftEdgeScreen + "  " + Screen.height);
	
		// Is the camera out of the map bounds?
		if(topRightEdgeScreen.x < Screen.width || topRightEdgeScreen.y < Screen.height || downLeftEdgeScreen.x > 0 || downLeftEdgeScreen.y>0){
			//smack a big plane at the camera position that covers more than the screen is showing
			cameraPositionFixPlane = new Plane(Vector3.forward*10, Camera.main.transform.position);

			//move the top right edge back so its inside the screen again
			Vector3 topRightEdgeScreenFixed = Camera.main.ScreenToWorldPoint(new Vector3(Mathf.Max(Screen.width, topRightEdgeScreen.x), Mathf.Max(Screen.height,topRightEdgeScreen.y), topRightEdgeScreen.z));
			//now we know the offset the camera should move at distance z to fix the top right edge
			Vector3 topRightOffsetAtDistance = topRightEdgeScreenFixed-topRightEdge;

			//this time for the down left edge
			Vector3 downLeftEdgeScreenFixed = Camera.main.ScreenToWorldPoint(new Vector3(Mathf.Min(0, downLeftEdgeScreen.x), Mathf.Min(0, downLeftEdgeScreen.y), downLeftEdgeScreen.z));
			//now we know the offset the camera should move at distance z to fix the down left edge
			Vector3 downLeftOffsetAtDistance = downLeftEdgeScreenFixed-downLeftEdge;

			Debug.Log ("offset: "+downLeftOffsetAtDistance);


			//where is the center of the screen translated at given distance
			Vector3 cameraCenterAtDistance = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2.0f, Screen.height/2.0f, topRightEdge.z));
			//now lets offset the center of the screen with the offset we found
			Vector3 cameraCenterAtDistanceFixed = new Vector3(cameraCenterAtDistance.x-topRightOffsetAtDistance.x-downLeftOffsetAtDistance.x, cameraCenterAtDistance.y-topRightOffsetAtDistance.y-downLeftOffsetAtDistance.y, cameraCenterAtDistance.z);

			//here we generate a ray at the camera center at distance pointing back to the camera
			Ray rayFromFixedDistanceToCameraPlane = new Ray(cameraCenterAtDistanceFixed, -Camera.main.transform.forward);

			//this is where the magic happens, lets raycast back to the plane i smacked infront of the  camera
			float d;
			cameraPositionFixPlane.Raycast(rayFromFixedDistanceToCameraPlane, out d);

			//where did the raycast hit the camera plane?
			Vector3 planeHitPoint = rayFromFixedDistanceToCameraPlane.GetPoint(d);

			//position camera at the hitpoint we found
			Camera.main.transform.position = new Vector3(planeHitPoint.x, planeHitPoint.y, Camera.main.transform.position.z);

	}*/
	
	
}
