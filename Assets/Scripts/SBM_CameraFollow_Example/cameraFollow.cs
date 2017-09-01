using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	public GameObject player;
	public Camera MainCam;
	public float smoothTimeY; //set in inspector
	public float smoothTimeX; //set in inspector
	public float height, width; //set in inspector

	public bool bounds; //This is a flag on a room that indicates the camera is bounded to a certain area.
						//It was set by a trigger when the player entered a new room. Was almost always true.

	public Vector3 minCamPos; 
	public Vector3 maxCamPos;
	public Vector2 velocity;
	private float posX;
	private float posY;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		MainCam = Camera.main; 
	}
			

	void FixedUpdate() {
		/*if (Input.GetKey (KeyCode.D)) {
			
		}
		if (Input.GetKey (KeyCode.S)) {
			
		}
		if (Input.GetKey (KeyCode.A)) {
			
		}
		if (Input.GetKey (KeyCode.W)) {
			
		}*/

		posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
		transform.position = new Vector3 (posX, posY, transform.position.z);
		if (bounds) {
			transform.position = new Vector3 (posX, posY, transform.position.z);
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, minCamPos.x, maxCamPos.x),
				Mathf.Clamp (transform.position.y, minCamPos.y, maxCamPos.y),
				Mathf.Clamp (transform.position.z, minCamPos.z, maxCamPos.z));
		} 
	}

	public void GetDimensions(){ //This was for testing. I wanted to see in the inspector if the player window was at the right resolution.
		height = 2f * MainCam.orthographicSize;
		width = height * MainCam.aspect;
	}
		
}



/*

Ok,Here is the method (which was in the player controller) that sets the minCamPos and maxCamPos. IN THE ELSE-IF STATEMENT.

WARNING: DO NOT TRY TO UNDERSTAND THIS TOO MUCH. IT IS SO STUPID.

Basically, the way that game worked was that when a player entered a room, a trigger collidder on the floor tagged "CamFinder." 
This was the trigger to tell the game that the character had entered a new room, and so the camera needed to re-align.
The 'mask' of the room was a game-object that had a sprite attached to it (the 'rend'). The 'rend' was the exact size of the entire room,
and it served as the bounding box of the camera. 'camPiv' was an empty game-object in the middle of the room that the camera would initially lerp to.
This was for smooth screen transition. Once entering a new room, the camera would move to a specific spot before following the player (how it does in zelda or Binding of Isaac).

If 'camPiv' had the tag "CamPivot", the game knew that the room was a basic room and the camera didn't need to follow the player.
If 'camPiv' had the tag "CamFollow", the game knew that the room was big and needed the camera to follow the player.  

Anyway, what's important is the setting of the minCamPos and maxCamPos. Since there is no way to physically bind the camera to the room, the bounds of the camera had to be relative to
the physical size of the room in-game and the width/heighth of the camera. 

I DO NOT REMEMBER HOW I CAME UP WITH THESE VECTOR3s. Things like 'rend.bounds.extents.x' are built into the SpriteRenderer class, so they can be gotten from any sprite.

playerCamera.width and playerCamera.height were set somewhere else in the code by calling the getDimensions() method included in this cameraFollow script.

The '-75.425' value for the Z component of the Vector3s is because that was the Z value for the main camera in the inspector. If the Z value for the camera is -10, that value would be -10.

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.GetComponent<Collider2D>().tag == "CamFinder") {
			mask = coll.transform.parent.gameObject.transform.parent.gameObject.transform.Find ("Mask").gameObject;
			rend = mask.GetComponent<SpriteRenderer>();
			camPiv = coll.gameObject.transform.GetChild(0);
			if (camPiv.tag == "CamPivot") { 
				hasPivot = true;
				enablePlayerCamBool = false;
				playerCamera.enabled = false;
				playerCamera.bounds = false;
			} 
			
			else if (camPiv.tag == "CamFollow" && !inRoom) { 
				inRoom = true;
				playerCamera.minCamPos = new Vector3 ((-rend.bounds.extents.x + rend.transform.position.x)+playerCamera.width/2f, (-rend.bounds.extents.y + rend.transform.position.y)+playerCamera.height/2f, -72.452f);
				playerCamera.maxCamPos = new Vector3 ((rend.bounds.extents.x + rend.transform.position.x)-playerCamera.width/2f, (rend.bounds.extents.y + rend.transform.position.y)-playerCamera.height/2f, -72.452f);
				hasPivot = false;
				enablePlayerCamBool = true;
				enablePlayerCamTimer = .35f;
				playerCamera.bounds = true;
			}
			cameraTarget = new Vector3 (0f, 0f, 0f) + (camPiv.position);
		}
	}


*/
