using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//MUST BE TURNED OFF IN INSPECTOR PRIOR TO PLAY
public class AstarUser : MonoBehaviour {
	//Needs: A list of target's occupied nodes. Target's position. A duplicate list of every node in the room (for h, g, f).
	private List<AstarNode> targetOccupied, thisEntityOccupied, roomNodes;
	[SerializeField]private AstarController controller;
	private Dictionary<string, List<AstarNode>> occupiedDict;
	[SerializeField]private string guidID;
	private AstarNode startNode, targetNode;
	private string target = "Player";
	private List<string> keys;


	void OnEnable () {
		roomNodes = controller.getRoomNodeListClone ();
		occupiedDict = controller.getDict ();
		keys = new List<string>(occupiedDict.Keys);
		AstarMaster.instance.showRoomNodes (roomNodes);
		//AstarMaster.instance.showNodeNeighbors (roomNodes, 150);
	}

	void Update () {
		AstarMaster.instance.colorRoomNodes (roomNodes);
		foreach (string s in keys) {
			occupiedDict [s] = AstarMaster.instance.getOccupiedNodes (roomNodes, s);
		}
		targetNode = setGoalNode (targetNode); //SETTING H COSTS IN THIS FUNCTION
		startNode = setStartNode (startNode);

	}

	public void setGuidID(string s) {
		guidID = s;
	}

	public void setClosestNode (AstarNode n) {
		startNode = n;
	}

	public AstarNode getClosestNode() {
		return startNode;
	}

	public string getGuidID() {
		return guidID;
	}

	AstarNode setGoalNode(AstarNode tNode) {
		AstarNode temp;
		if (occupiedDict [target].Count != 0) 
			temp = occupiedDict [target] [Mathf.RoundToInt (occupiedDict [target].Count - 1)];
		else 
			return tNode;
		if (temp != null) {
			if (tNode != null && temp.compareTo (tNode)) 
				return temp;
			else if (tNode != null)
				tNode.setGoal (false);
			temp.setGoal (true);
		}
		//Only setHCosts if goal node has changed
		controller.setHCosts (roomNodes, temp);
		return temp;
	}

	AstarNode setStartNode(AstarNode sNode){
		AstarNode temp;
		if (occupiedDict [guidID].Count != 0)
			temp = occupiedDict [guidID] [Mathf.RoundToInt (occupiedDict [guidID].Count - 1)];
		else
			return sNode;
		if (temp != null) {
			if (sNode != null && temp.compareTo (sNode)) 
				return temp;
			else if (sNode != null) 
				sNode.setStart (false);
			temp.setStart (true);
		}
		return temp;
	}
		
}
