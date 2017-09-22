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

	private List<string> keys;
	private GameObject[] enemies;
	[SerializeField] private List<AstarNode> nodes, totalOccupiedNodes;
	//[SerializeField] private List<GameObject> entities;

	//Maybe, instead of using a dictionary, use a class, so that each
	[SerializeField] private Dictionary<string, List<AstarNode>> occupiedDict;


	void Start(){
		totalOccupiedNodes = new List<AstarNode> ();

		occupiedDict = new Dictionary<string, List<AstarNode>> ();
		occupiedDict.Add ("Player", new List<AstarNode> ()); // has to be here for some reaason
		//List to hold all nodes in the scene 
		nodes = new List<AstarNode> ();
		minNodeDistance = 2;

		createGrid ();
		createNodes ();
		this.setNeighbors (nodes);

		//Identifies all enemies using AStar in the scene and adds an entry to the occupiedDict dictionary
		//Their keys are not a tag, unlike the "Player" tag for the player character; instead
		//their keys are generated at run-time using createNewGuidID() in the AstarMaster.cs singleton.
		identifyAndAddAstarEnemies();
		//Also add a "Player" entry in the dictionary.


		//for iteration through dictionary of entities in scene
		keys = new List<string> (occupiedDict.Keys);

		//AstarMaster.instance.showRoomNodes(nodes);
	}

	void Update() {
		//totalOccupiedNodes = AstarMaster.instance.maintainTotalOccupiedNodes(nodes, occupiedDict, totalOccupiedNodes, keys);

//		foreach (string s in keys) {
//			occupiedDict[s] = AstarMaster.instance.getOccupiedNodes (nodes, s);
//		}

		//For debugging purposes, color all nodes currently occupied by entities IN RED. MUST HAVE "AstarMaster.instance.showRoomNodes(nodes);" at end of Start().
		//AstarMaster.instance.colorRoomNodes (nodes, totalOccupiedNodes);
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
				RaycastHit2D hit = Physics2D.CircleCast(rayStart, 1f, direction);

				//Check to see if Vector at nodeGrid[k,h] is in a pathable position
				if (hit == null || (hit != null && (hit.collider == null || hit.collider.tag == "Player" || hit.collider.tag == "enemy"))) {
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
						//Giving each node a GameObject for testing HERE
						nodes [nodes.Count - 1].setParameters (k, h, nodeGrid[k,h]);
						nodes [nodes.Count - 1].setNodeObj (GameObject.Instantiate (nodeObject, nodes [nodes.Count - 1].getLocation (), Quaternion.identity));
						nodes [nodes.Count - 1].getObject ().SetActive (false);
						nodes [nodes.Count - 1].getObject ().layer = 2;
					}
				}

			}
		}
	}

	public void setNeighbors(List<AstarNode> nodeList){
		foreach (AstarNode node in nodeList) {
			int row = node.getRow ();
			int col = node.getCol ();

			AstarNode neighbor;
			for (int i = -1; i < 2; i++){
				for (int j = -1; j < 2; j++) {
					neighbor = nodeList.Find (n => n.getRow () == (row + i) && n.getCol () == (col + j));
					if (neighbor != null && !neighbor.compareTo(node)) {
						//might have to remove this check and ensure that node distance is always less than the minimum thickness of an object
						RaycastHit2D hit = Physics2D.Raycast (neighbor.getLocation (), node.getLocation (), Vector3.Distance(node.getLocation(), neighbor.getLocation()) );
						if (hit == null || (hit != null && (hit.collider == null || hit.collider.tag == "Player" || hit.collider.tag == "enemy"))) 
							node.getList ().Add (neighbor);
					}
				}
			}
		}
	}

	/// <summary>
	/// Identifies the entities in a scene associated with this AstarController that are using AStar, gives them a unique ID, and adds them to the occupiedDict Dictionary.
	/// </summary>
	void identifyAndAddAstarEnemies() {
		if (enemies == null) 
			enemies = GameObject.FindGameObjectsWithTag ("enemy");
		foreach (GameObject enemy in enemies) {
			AstarUser u = enemy.GetComponent<AstarUser> ();
			if (u == null) //if object tagged "enemy" does not have an AstarUser script, do not treat it as an Astar entity.
				continue;
			u.setGuidID (AstarMaster.instance.createNewGuidID ());
			occupiedDict.Add (u.getGuidID (), new List<AstarNode> ());
			//This has to be enabled after EVERYTHING has been
			u.enabled = true;
		}

	}

	/// <summary>
	/// Gets the target's occupied nodes. The target is identified by a string, which is used as a key for the occupiedDict Dictionary.
	/// </summary>
	/// <returns>The target's occupied nodes.</returns>
	/// <param name="targetTag">Target tag.</param>
	public List<AstarNode> getOccupiedNodeList (string targetTag) {
		return occupiedDict [targetTag];
	}

	public List<AstarNode> getRoomNodeListClone(){
		List<AstarNode> tempList = new List<AstarNode>();
//		foreach (AstarNode n in nodes)
//			tempList.Add (new AstarNode(n.getRow(), n.getCol(), n.getLocation()));
		tempList = nodes.ConvertAll(node => new AstarNode(node.getRow(), node.getCol(), node.getLocation(), node.getObject(), node.getList()));
		return tempList;
	}

	public void setHCosts(List<AstarNode> rn, AstarNode goal) {
		foreach (AstarNode n in rn) {
			n.setH ((Mathf.Abs (n.getRow () - goal.getRow ()) + Mathf.Abs (n.getCol () - goal.getCol ())) * 10);
		}
	}

	public List<string> getKeys(){
		return keys;
	}

	public Dictionary<string, List<AstarNode>> getDict(){
		return occupiedDict;
	}
//	void showNodeNeighbors(int nodeNum) {
//		Debug.Log (nodes [nodeNum].toString ());
//		GameObject obj = GameObject.Instantiate (nodeObject, nodes[nodeNum].getLocation(), Quaternion.identity);
//		obj.GetComponent<SpriteRenderer> ().color = Color.red;
//		obj.layer = 2;
//		foreach (AstarNode n in nodes[nodeNum].getList()) {
//			Debug.Log (n.toString ());
//			obj = GameObject.Instantiate (nodeObject, n.getLocation (), Quaternion.identity);
//			obj.layer = 2;
//		}
//	}
}
