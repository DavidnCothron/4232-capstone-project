using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarController : MonoBehaviour {
	[SerializeField] private SpriteRenderer room;
	[SerializeField] private Vector3 bounds;
	[SerializeField] private Vector3[] minMax;
	[SerializeField] private Vector3[][] nodeLocations;
	[SerializeField] private float length, height, minNodeDistanceX, minNodeDistanceY;
	public GameObject node;

	// Use this for initialization
	void Awake(){
		minMax = SpriteCoordinatesLocalToWorld (room);
		length = minMax [1].x - minMax [0].x;
		height = minMax [1].y - minMax [0].y;
		minNodeDistanceX = nodePlacementIncrement (length);
		minNodeDistanceY = nodePlacementIncrement (height);

		//bounds = new Vector3 (room.bounds.extents.x, room.bounds.extents.y, room.bounds.extents.z);
		//for (int i = Mathf.CeilToInt (minMax [0].x); i < Mathf.CeilToInt (minMax [1].x); i++)
			//for (int j = Mathf.CeilToInt (minMax [0].y); i < Mathf.CeilToInt (minMax [1].y); i++)
				//GameObject.Instantiate (node, new Vector3 (i, j, 0.1f), Quaternion.identity);

		//GameObject min = GameObject.Instantiate (node, minMax [0], Quaternion.identity);
		//GameObject max = GameObject.Instantiate (node, minMax [1], Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static Vector3[] SpriteCoordinatesLocalToWorld(SpriteRenderer sp){
		//Vector3 pos = sp.transform.position;
		Vector3[] array = new Vector3[2];
		//array [0] = pos + sp.bounds.min;
		//array [1] = pos + sp.bounds.max;
		array[0] = sp.bounds.min;
		array [1] = sp.bounds.max;
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
}
