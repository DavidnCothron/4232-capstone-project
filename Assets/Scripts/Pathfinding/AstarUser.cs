using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//MUST BE TURNED OFF IN INSPECTOR PRIOR TO PLAY
public class AstarUser : MonoBehaviour {
	//Needs: A list of target's occupied nodes. Target's position. A duplicate list of every node in the room (for h, g, f).
	private List<AstarNode> targetOccupied, thisEntityOccupied, roomNodes;
	[SerializeField]private AstarController controller;
	private Dictionary<string, List<AstarNode>> occupiedDict;
	private string guidID;
	private AstarNode closestNode, targetNode;
	private string target = "Player";
	private List<string> keys;


	void OnEnable () {
		roomNodes = controller.getRoomNodeListClone ();
		occupiedDict = controller.getDict ();
		keys = new List<string>(occupiedDict.Keys);
		//AstarMaster.instance.showRoomNodes (roomNodes);
		//AstarMaster.instance.showNodeNeighbors (roomNodes, 150);
	}

	void Update () {
		//AstarMaster.instance.colorRoomNodes (roomNodes);
		foreach (string s in keys) {
			occupiedDict [s] = AstarMaster.instance.getOccupiedNodes (roomNodes, s);
		}

	}

	public void setGuidID(string s) {
		guidID = s;
	}

	public void setClosestNode (AstarNode n) {
		closestNode = n;
	}

	public AstarNode getClosestNode() {
		return closestNode;
	}

	public string getGuidID() {
		return guidID;
	}

	void setGoalNode() {
		if (targetOccupied.Count != 0) {
			targetNode = targetOccupied [0];
			if (targetNode.getObject () != null) {
				targetNode.getObject ().GetComponent<SpriteRenderer> ().color = Color.cyan;
			}
		}
	}
		
}
