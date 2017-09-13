using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarController : MonoBehaviour {
	[SerializeField] private SpriteRenderer room;
	private Vector3[] minMax;
	private Vector3[,] nodeGrid;
	private AstarNode[,] nodes;
	private int length, height, minNodeDistance;
	private float xInc, yInc;
	public GameObject node;
	private bool hitsNotNull, hitsBoundaries;

	private bool hitsNotAstarTest; // temporary condition for testing algorithm

	// Use this for initialization
	void Start(){
		minNodeDistance = 2;
		minMax = SpriteCoordinatesLocalToWorld (room);
		//Debug.Log ((minMax [1].x - minMax [0].x));

		length = (int)((minMax [1].x - minMax [0].x)/minNodeDistance);//getEvenInt(Mathf.FloorToInt (minMax [1].x - minMax [0].x))
		height = (int)((minMax [1].y - minMax [0].y)/minNodeDistance); //divided by minNodeDistance
		nodeGrid = new Vector3[length,height];
		nodes = new AstarNode[length, height];

		//For traversal
		Vector3 min, max;
		min = minMax [0]; //new vector3 with rounded floats like length/height
		max = minMax [1];

		int i = 0;
		int j = 0;

		//Debug.Log ("Length: " + length + " height: " + height);
		//Debug.Log ("Max.x - min.x: " + (max.x - min.x));
		//Debug.Log ("Max.y - min.y: " + (max.y - min.y));

		//populate a 2d array with vector3 locations for node placement
		while (min.x < max.x) {
			while (min.y < max.y) {
				nodeGrid [i, j] = new Vector3 (min.x + 1.25f, min.y + 1.25f, 0f);
				j++;
				min.y += minNodeDistance;

			}
			min.x += minNodeDistance;
			min.y = minMax [0].y;
			i++;
			j = 0;
		}


		for (int k = 0; k < nodeGrid.GetLength (0); k++) {
			for (int h = 0; h < nodeGrid.GetLength (1); h++) {
				if (nodeGrid [k, h].x == 0 && nodeGrid [k, h].y == 0) {
					Debug.Log ("NODE INSTANTIATED AT (0,0); NODE GRID TOO LARGE FOR # OF NODES");
					continue;
				}
				//GameObject obj = GameObject.Instantiate (node, nodeGrid [k, h], Quaternion.identity);
				//Debug.DrawRay(new Vector3 (nodeGrid[k,h].x, nodeGrid[k,h].y,
					//-UnityDepth.instance.unityDepth), Vector3.forward * UnityDepth.instance.unityDepth, Color.red, 30, false);
				Vector3 rayStart = new Vector3 (nodeGrid [k, h].x, nodeGrid [k, h].y, -UnityDepth.instance.unityDepth);
				Vector3 direction = (Vector3.forward * (UnityDepth.instance.unityDepth));
				RaycastHit2D hit = Physics2D.Raycast (rayStart, direction);

				//Check to see if Vector at nodeGrid[k,h] is in a pathable position
				if (hit == null || (hit != null && (hit.collider == null || hit.collider.tag == "Player"))) {
					//If vector is in pathable position, check to see if it's in current room
					RaycastHit2D hitUp = Physics2D.Raycast (nodeGrid [k, h], Vector3.up, 100);
					RaycastHit2D hitDown = Physics2D.Raycast (nodeGrid [k, h], Vector3.down, 100);
					RaycastHit2D hitRight = Physics2D.Raycast (nodeGrid [k, h], Vector3.right, 100);
					RaycastHit2D hitLeft = Physics2D.Raycast (nodeGrid [k, h], Vector3.left, 100);
					hitsNotNull = hitsBoundaries = false;
					//Will have to add condition to check if the hit is in the currently occupied room
					//Condition: Each raycast from position hits something
					if (hitUp != null && hitDown != null && hitRight != null && hitLeft != null)
						hitsNotNull = true;
					//Condition: Each hit's collider is not null
					if (hitsNotNull && (hitUp.collider != null && hitDown.collider != null && hitRight.collider != null && hitLeft.collider != null))
						hitsBoundaries = true;
					//Temporary Condition: hit's gameobject is not an Astar Node test object
//					if (hitsBoundaries && (hitUp.transform.tag != "testAstar" && hitDown.transform.tag != "testAstar"
//						&& hitRight.transform.tag != "testAstar" && hitLeft.transform.tag != "testAstar"))
//						hitsNotAstarTest = true;
					
					//If the above conditions are true, the node is un the current room.
					if (hitsNotNull && hitsBoundaries) {
						GameObject obj = GameObject.Instantiate (node, nodeGrid [k, h], Quaternion.identity);
						obj.tag = "testAstar";
						obj.layer = 2;
					}
				}

			}
		}
	}

	public static Vector3[] SpriteCoordinatesLocalToWorld(SpriteRenderer sp){
		//Vector3 pos = sp.transform.position;
		//array [0] = pos + sp.bounds.min;
		//array [1] = pos + sp.bounds.max;
		Vector3[] array = new Vector3[2];
		array[0] = sp.bounds.min;
		array [1] = sp.bounds.max;
		array [0] = new Vector3 (Mathf.Floor (array [0].x), Mathf.Floor (array [0].y), 0f);
		array[1] = new Vector3 (Mathf.Ceil(array[1].x), Mathf.Ceil(array[1].y), 0f);

		//check to ensure even numbers on vector floats. reduce using turnary statement
		if (array [0].x % 2 != 0) 
			array [0].x--;
		
		if (array [0].y % 2 != 0) 
			array [0].y--;
		
		if (array [1].x % 2 != 0) 
			array [1].x++;
		
		if (array [1].y % 2 != 0) 
			array [1].y++;
		

		return array;
	}		
}
