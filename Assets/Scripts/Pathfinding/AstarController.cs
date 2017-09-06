using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarController : MonoBehaviour {
	[SerializeField] private SpriteRenderer room;
	[SerializeField] private Vector3[] minMax;
	[SerializeField] private Vector3[,] nodeGrid;
	[SerializeField] private int length, height, minNodeDistance;
	[SerializeField] private List<Vector3> nodeLocations = new List<Vector3>();
	private float xInc, yInc;
	public GameObject node;

	// Use this for initialization
	void Start(){
		minNodeDistance = 2;
		minMax = SpriteCoordinatesLocalToWorld (room);
		length = getEvenInt(Mathf.FloorToInt (minMax [1].x - minMax [0].x)/minNodeDistance);
		height = getEvenInt(Mathf.FloorToInt(minMax [1].y - minMax [0].y)/minNodeDistance);
		nodeGrid = new Vector3[length,height];

		Vector3 min, max;
		min = minMax [0];
		max = minMax [1];

		//Debug.Log ("Length, Height: " + length + ", " + height);
		//Debug.Log ((max.x - min.x) + ", " + (max.y - min.y));

		int i = 0;
		int j = 0;
		//populate a list with vector3 locations for node placement

		while (min.x < max.x -minNodeDistance) {

			//Debug.Log ("min.x, max.x: " + min.x + ", " + max.x);
			while (min.y < max.y - minNodeDistance) {
				
				//Debug.Log ("min.y, max.y: " + min.y + ", " + max.y);

				nodeGrid [i, j] = new Vector3 (min.x + 1.25f, min.y + 1.25f, 0f);
				//Debug.Log (j);
				j++;
				min.y += minNodeDistance;

			}
			min.x += minNodeDistance;
			min.y = minMax [0].y;
			//Debug.Log (i);
			i++;
			j = 0;
		}


		for (int k = 0; k < nodeGrid.GetLength (0); k++) {
			for (int h = 0; h < nodeGrid.GetLength (1); h++) {
				if (nodeGrid [k, h].x == 0 && nodeGrid [k, h].y == 0) {
					//Debug.Log (k + ", " + h);
					continue;
				}
				GameObject.Instantiate (node, nodeGrid [k, h], Quaternion.identity);
			}
		}
			

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static Vector3[] SpriteCoordinatesLocalToWorld(SpriteRenderer sp){
		//Vector3 pos = sp.transform.position;
		//array [0] = pos + sp.bounds.min;
		//array [1] = pos + sp.bounds.max;
		Vector3[] array = new Vector3[2];
		array[0] = sp.bounds.min;
		array [1] = sp.bounds.max;
		array [0] = new Vector3 (Mathf.Ceil (array [0].x), Mathf.Floor (array [0].y), 0f);
		array[1] = new Vector3 (Mathf.Floor(array[1].x), Mathf.Floor(array[1].y), 0f);

		if (array [0].x % 2 != 0) {
			array [0].x++;
		}
		if (array [0].y % 2 != 0) {
			array [0].y++;
		}
		if (array [1].x % 2 != 0) {
			array [1].x--;
		}
		if (array [1].y % 2 != 0) {
			array [1].y--;
		}

		Debug.Log ("MIN : " + array [0].x + ", " + array [0].y);
		Debug.Log ("MAX : " + array [1].x + ", " + array [1].y);

		return array;
	}

	public static float nodePlacementIncrement(float n){
		float min = .5f;
		int tries = 0;
		while (n % min != 0 && tries < 5) {
			min += .1f;
			tries++;
		}
		if (tries == 5)
			min = .5f;
		return min;
	}

	public static int getEvenInt(int n){
		if (n % 2 != 0) {
			n--;
		}

		return n;
	}
		
}
