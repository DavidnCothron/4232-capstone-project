using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarController : MonoBehaviour {
	[SerializeField] private GameObject player;
	[SerializeField] private SpriteRenderer room;
	private Vector3[] minMax;
	private Vector3[,] nodeGrid;
	private Vector3 playerPosition;
	private int length, height, minNodeDistance;
	private float xInc, yInc;
	public GameObject nodeObject;
	private bool hitsNotNull, hitsBoundaries, hitDistance;

	[SerializeField] private List<AstarNode> nodes;
	//[SerializeField] private List<GameObject> entities;
	[SerializeField] private Dictionary<string, List<AstarNode>> occupiedDict;

	void Start(){
		
		//Add an entry to 'occupied' for each enemy/player in the scene
		//This will have to eventually be in a loop to get all entities in a scene
		occupiedDict = new Dictionary<string, List<AstarNode>> ();
		occupiedDict.Add ("Player", new List<AstarNode> ());

		//List to hold all nodes in the scene
		nodes = new List<AstarNode> ();
		minNodeDistance = 2;

		createGrid ();
		createNodes ();
		setNeighbors ();
		showRoomNodes (nodes);
	}

	void Update() {
		//might be better in fixed update?
		occupiedDict["Player"] = AstarMaster.instance.getOccupiedNodes(nodes, "Player");
		colorOccupiedNodes (occupiedDict["Player"]);
	}

	void FixedUpdate() {

	}

	public static Vector3[] SpriteCoordinatesLocalToWorld(SpriteRenderer sp) {
		//Array holds the absolute minimum and maximum Vector3 values
		Vector3[] array = new Vector3[2];
		array[0] = sp.bounds.min;
		array [1] = sp.bounds.max;
		array [0] = new Vector3 (Mathf.Floor (array [0].x), Mathf.Floor (array [0].y), 0f);
		array[1] = new Vector3 (Mathf.Ceil(array[1].x), Mathf.Ceil(array[1].y), 0f);

		//check to ensure even numbers on vector floats.
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

	void createGrid() {

		minMax = SpriteCoordinatesLocalToWorld (room);

		length = (int)((minMax [1].x - minMax [0].x)/minNodeDistance);//getEvenInt(Mathf.FloorToInt (minMax [1].x - minMax [0].x))
		height = (int)((minMax [1].y - minMax [0].y)/minNodeDistance); //divided by minNodeDistance
		nodeGrid = new Vector3[length,height];

		//For traversal
		Vector3 min, max;
		min = minMax [0];
		max = minMax [1];

		int i = 0;
		int j = 0;

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
	}

	void createNodes(){
		for (int k = 0; k < nodeGrid.GetLength (0); k++) {
			for (int h = 0; h < nodeGrid.GetLength (1); h++) {
				if (nodeGrid [k, h].x == 0 && nodeGrid [k, h].y == 0) {
					Debug.Log ("NODE INSTANTIATED AT (0,0); NODE GRID TOO LARGE FOR # OF NODES");
					continue;
				}

				Vector3 rayStart = new Vector3 (nodeGrid [k, h].x, nodeGrid [k, h].y, -UnityDepth.instance.unityDepth);
				Vector3 direction = (Vector3.forward * (UnityDepth.instance.unityDepth));
				RaycastHit2D hit = Physics2D.CircleCast(rayStart, 1.25f, direction);

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

					//If the above conditions are true, the node is un the current room.
					if (hitsNotNull && hitsBoundaries) {
						nodes.Add(new AstarNode());
						nodes [nodes.Count - 1].setParameters (k, h, nodeGrid[k,h]);
					}
				}

			}
		}
	}

	void setNeighbors(){
		foreach (AstarNode node in nodes) {
			int row = node.getRow ();
			int col = node.getCol ();

			AstarNode neighbor;
			for (int i = -1; i < 2; i++){
				for (int j = -1; j < 2; j++) {
					neighbor = nodes.Find (n => n.getRow () == (row + i) && n.getCol () == (col + j));
					if (neighbor != null && neighbor != node) {
						//might have to remove this check and ensure that node distance is always less than the minimum thickness of an object
						RaycastHit2D hit = Physics2D.Raycast (neighbor.getLocation (), node.getLocation (), Vector3.Distance(node.getLocation(), neighbor.getLocation()) );
						if (hit == null || (hit != null && (hit.collider == null || hit.collider.tag == "Player"))) 
							node.getList ().Add (neighbor);
					}
				}
			}
		}
	}

	void setHCostsNodeToPlayer() {
		foreach (AstarNode n in nodes) {
			
		}
	}

	void showNodeNeighbors(int nodeNum) {
		Debug.Log (nodes [nodeNum].toString ());
		GameObject obj = GameObject.Instantiate (nodeObject, nodes[nodeNum].getLocation(), Quaternion.identity);
		obj.GetComponent<SpriteRenderer> ().color = Color.red;
		obj.layer = 2;
		foreach (AstarNode n in nodes[nodeNum].getList()) {
			Debug.Log (n.toString ());
			obj = GameObject.Instantiate (nodeObject, n.getLocation (), Quaternion.identity);
			obj.layer = 2;
		}
	}

	void showRoomNodes(List<AstarNode> rn) {
		GameObject obj;
		foreach (AstarNode n in rn) {
			obj = GameObject.Instantiate (nodeObject, n.getLocation (), Quaternion.identity);
			obj.layer = 2;
			n.setNodeObj (obj);
		}
	}

	void colorOccupiedNodes(List<AstarNode> occuNodes){
		foreach (AstarNode n in nodes) {
			if (occuNodes.Contains (n))
				n.getObject ().GetComponent<SpriteRenderer> ().color = Color.red;
			else
				n.getObject ().GetComponent<SpriteRenderer> ().color = Color.green;
		}
	}
}
