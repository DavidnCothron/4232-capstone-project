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

	void Start(){
		topRight = camera.ViewportToWorldPoint (new Vector3 (1, 1, 16.875f));
		bottomRight = camera.ViewportToWorldPoint (new Vector3 (1, 0, 16.875f));
		topLeft = camera.ViewportToWorldPoint (new Vector3 (0, 1, 16.875f));
		bottomLeft = camera.ViewportToWorldPoint (new Vector3 (0, 0, 16.875f));
	}

	// Update is called once per frame
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
