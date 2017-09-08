using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityDepth : MonoBehaviour {

	public float unityDepth, focusZ, PPU, screenResHeight, screenResWidth; //Screen.currentResolution.height - Screen.currentResolution.Width
	public Dictionary<int,float> layerAndPPU;
	public layer[] layers;
	public Matrix4x4 viewProjection = new Matrix4x4 ();

	private static UnityDepth u_Instance = null;
	public static UnityDepth instance {
		get {
			if (u_Instance == null) {
				u_Instance = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UnityDepth> ();
			}
			if (u_Instance == null) {
				u_Instance = GameObject.FindGameObjectWithTag ("MainCamera").AddComponent (typeof(UnityDepth)) as UnityDepth;
			}
			if (u_Instance == null) {
				GameObject obj = new GameObject ("UnityDepth");
				u_Instance = obj.AddComponent (typeof(UnityDepth)) as UnityDepth;
				Debug.Log ("No main camera exists in scene; UnityDepth was generated automatically.");
			}
			return u_Instance;
		}
	}


	//SPRITE WORLD SPACE = -16.875 + zDistance
	void Awake () {
		viewProjection = Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix;
		FindUnityDepth ();

		layers = new layer[128];

		//layerAndPPU = new Dictionary<int, float> ();
		for (int i = 0; i < 128; i++) {
			float zDistance = PPUzDistance (i);
			layers [i] = new layer (i, zDistance, 0f);
			//Debug.Log ("zDistance, PPU: " + layers[i].zDimension + ", " + layers[i].pixelsPerUnit);
			//layerAndPPU.Add (i, zDistance);
		}

//		foreach (KeyValuePair<int,float> entry in layerAndPPU) {
//			Debug.Log ("zDistance, PPU: " + entry.Key + ", " + entry.Value);
//		}
	}

	public void FindUnityDepth(){
		if (PPU == 0) 
			PPU = 32;

		//Done with screen width
		//1920.0f can be replaced with Screen.width
		float screenWidth = screenResWidth / PPU / 2.0f;
		unityDepth = viewProjection [0] * screenWidth;

		focusZ = unityDepth;

		//If done with shifting FOV:
		//focusZ = Camera.main.transform.position.z + unityDepth;

		//Done with screen height
		//1080.0f can be replaced by Screen.height
		float screenHeight = screenResHeight / PPU / 2.0f;
		float unityDepth2 = viewProjection [5] * screenHeight;

		//Done using fieldOfView and screenHeight
		float distance = screenHeight / Mathf.Tan (Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
		//Debug.Log ("Calculated using fov = " + distance);
	}

	void OnApplicationQuit(){
		u_Instance = null;
	}

	public float zDistancePPU(float layerZ){
		float width = layerZ / viewProjection [0];
		float layerPPU = screenResWidth / (2 * width);
		return layerPPU;
	}

	public float PPUzDistance(float pixels){
		float screenWidth = screenResWidth / pixels / 2f;
		return viewProjection [0] * screenWidth;
	}

	public class layer {
		public int pixelsPerUnit;
		public float zDimension;
		public float pixelsOnScreen;

		public layer(){}

		public layer(int PPU, float z, float POS){
			pixelsPerUnit = PPU;
			zDimension = z;
			pixelsOnScreen = POS;
		}
	}


	/*
	 * This class is a singleton. 
	 * It moves the camera away from the world space based on a PPU (Pixels Per Unit) value.
	 * This is done to give the main camera a pixel-perfect image while in perspective mode.
	 * 
	 * A Pixel-perfect image in orthographic mode is given by orthoCam.orthographicSize = Screen.height / PPU / 2f;
	 * 
	 * I've chosen a Field of View of 90 because the -Z value that the camera is placed from the
	 * scene when running this script is the exact same value for a pixel perfect image in orthographic mode.
	 * That is, when PPU = 32, the orthographic size should be 16.875.
	 * At 90 FOV and PPU = 32, the -z distance is exactly 16.875.
	 * 
	 * In the Dev_PlayerController scene, I chose a PPU of 374 because it closely matched the way
	 * the scene looked before, and I didn't want to upset any dev testing.
	 * 
	 * I'm currently looking at using a PPU of 32 for scale, though.
	 * 
	 * //matrix are linear unidimensional array - column major notation like this:
	//
	// |	0	4	8	12	|
	// |	1	5	9	13	|
	// |	2	6	10	14	|
	// |	3	7	11	15	|
	//
	// The first [0,0] element of the matrix (0) is the x scale - effects scale in x direction
	// [1,1] element of the matrix (5) is the y scale
	// [2,2] element of the matrix (10) is the z scale
	*/
}
