using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarController : MonoBehaviour {
	#region data
	//Object References
	[SerializeField] private GameObject player;
	[SerializeField] private SpriteRenderer room;
	public GameObject nodeObject;
	private GameObject[] enemies;
	[SerializeField]private GameObject enemyContainer;

	//Vectors
	private Vector3[] minMax;
	private Vector3[,] nodeGrid;
	private Vector3 playerPosition;

	//Basic datatypes
	private int length, height, minNodeDistance;
	private float xInc, yInc;
	private bool hitsNotNull, hitsBoundaries, hitDistance;
	LayerMask Background;

	//Lists and Dictionaries
	private List<string> keys;
	[SerializeField] private List<AstarNode> nodes, totalOccupiedNodes;
	[SerializeField] private Dictionary<string, List<AstarNode>> occupiedDict;
	#endregion
	#region initialization
	void Start(){
		//Layermask that ignores the background layer and collider
		Background = ~(1 << LayerMask.NameToLayer ("RoomBackground") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("IgnoreAstarGridBuild"));
		StartCoroutine (tempStart ());
	}
	IEnumerator tempStart() {
		yield return new WaitForSeconds (1f);
		room = GameControl.control.getCurrentRoom ().getRoomExtents ();
		totalOccupiedNodes = new List<AstarNode> ();
		occupiedDict = new Dictionary<string, List<AstarNode>> ();
		occupiedDict.Add ("Player", new List<AstarNode> ()); // has to be here for some reaason

		//List to hold all nodes in the scene 
		nodes = new List<AstarNode> ();
		minNodeDistance = 2;

		//Creates a 2D array of Vector3s that hold the location of node in a room.
		createGrid ();
		//Populates a list (nodes) that holds all pathable nodes in a room.
		createNodes ();
		//Sets the neighbors of every node in the list (nodes)
		//this.setNeighbors (nodes);

		//Identifies all enemies using AStar in the scene and adds an entry to the occupiedDict dictionary
		//Their keys are not a tag, unlike the "Player" tag for the player character; instead
		//their keys are generated at run-time using createNewGuidID() in the AstarMaster.cs singleton.
		identifyAndAddAstarEnemies();

		//for iteration through dictionary of entities in scene
		keys = new List<string> (occupiedDict.Keys);

		//AstarMaster.instance.showRoomNodes(nodes);
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

				Vector3 rayStart = new Vector3 (nodeGrid [k, h].x, nodeGrid [k, h].y, -UnityDepth.instance.unityDepth2);
				Vector3 direction = (Vector3.forward * (UnityDepth.instance.unityDepth));
				RaycastHit2D hit = Physics2D.CircleCast(rayStart, 1.75f, direction, 1f, Background);
				//Check to see if Vector at nodeGrid[k,h] is in a pathable position
				if (hit == null || (hit != null && (hit.collider == null || hit.collider.tag == "Player" || hit.collider.tag == "Enemy"))) {
					//If vector is in pathable position, check to see if it's in current room
					RaycastHit2D hitUp = Physics2D.Raycast (nodeGrid [k, h], Vector3.up, 20);
					RaycastHit2D hitDown = Physics2D.Raycast (nodeGrid [k, h], Vector3.down, 20);
					RaycastHit2D hitRight = Physics2D.Raycast (nodeGrid [k, h], Vector3.right, 20);
					RaycastHit2D hitLeft = Physics2D.Raycast (nodeGrid [k, h], Vector3.left, 20);
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
	/// <summary>
	/// Identifies the entities in a scene associated with this AstarController that are using AStar, gives them a unique ID, and adds them to the occupiedDict Dictionary.
	/// </summary>
	void identifyAndAddAstarEnemies() {
		
		if (enemies == null) {
			enemies = GameControl.control.GetChildGameObjects (enemyContainer);
			//enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		}
		foreach (GameObject enemy in enemies) {
			AstarUser u = enemy.GetComponent<AstarUser> ();
			if (u == null) //if object tagged "enemy" does not have an AstarUser script, do not treat it as an Astar entity.
				continue;
			u.setGuidID (GameControl.control.createGUID ());
			occupiedDict.Add (u.getGuidID (), new List<AstarNode> ());
			//This has to be enabled after EVERYTHING has been
			u.enabled = true;
		}

	}
	#endregion
	#region getters
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
		tempList = nodes.ConvertAll(node => new AstarNode(node.getRow(), node.getCol(), node.getLocation(), node.getObject(), node.getList()));
		return tempList;
	}

	public List<string> getKeys(){
		return keys;
	}

	public Dictionary<string, List<AstarNode>> getDict(){
		return occupiedDict;
	}
	#endregion
	#region setters
	public void setHCosts(List<AstarNode> rn, AstarNode goal) {
		foreach (AstarNode n in rn) {
			n.setH ((Mathf.Abs (n.getRow () - goal.getRow ()) + Mathf.Abs (n.getCol () - goal.getCol ())) * 10);
		}
	}
	#endregion

	#region not_used
	void Update() {
		//totalOccupiedNodes = AstarMaster.instance.maintainTotalOccupiedNodes(nodes, occupiedDict, totalOccupiedNodes, keys);
		//foreach (string s in keys) {
		//	occupiedDict[s] = AstarMaster.instance.getOccupiedNodes (nodes, s);
		//}
		//For debugging purposes, color all nodes currently occupied by entities IN RED. MUST HAVE "AstarMaster.instance.showRoomNodes(nodes);" at end of Start().
		//AstarMaster.instance.colorRoomNodes (nodes, totalOccupiedNodes);
	}
	#endregion
}
