﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//MUST BE TURNED OFF IN INSPECTOR PRIOR TO PLAY
public class AstarUser : MonoBehaviour {
	//Needs: A list of target's occupied nodes. Target's position. A duplicate list of every node in the room (for h, g, f).
	private List<AstarNode> targetOccupied, thisEntityOccupied, roomNodes, openList, closedList, path;
	[SerializeField]private AstarController controller;
	private Dictionary<string, List<AstarNode>> occupiedDict;
	[SerializeField]private string guidID;
	private AstarNode startNode, targetNode;
	private string target = "Player";
	private List<string> keys;
	private bool foundGoal;


	void OnEnable () {
		roomNodes = controller.getRoomNodeListClone ();
		occupiedDict = controller.getDict ();
		keys = new List<string>(occupiedDict.Keys);
		AstarMaster.instance.showRoomNodes (roomNodes);
		openList = new List<AstarNode> ();
		closedList = new List<AstarNode> ();
		path = new List<AstarNode> ();
		//AstarMaster.instance.showNodeNeighbors (roomNodes, 150);
	}

	void Update () {
		AstarMaster.instance.colorRoomNodes (roomNodes);
		foreach (string s in keys) {
			occupiedDict [s] = AstarMaster.instance.getOccupiedNodes (roomNodes, s);
		}
		startNode = setStartNode (startNode);
		targetNode = setGoalNode (targetNode); //SETTING H COSTS AND SEARCHING IN/ FROM THIS FUNCTION
//		Debug.Log(roomNodes.Count);
//		Debug.Log(closedList.Count);
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
		//Probably reset values here
		controller.setHCosts (roomNodes, temp);
		foundGoal = false;
		resetNodes(roomNodes);
		closedList.Clear();
		openList.Clear ();
		path.Clear ();
		Debug.Log (startNode.getRow () + " " + startNode.getCol ());
		search (startNode);
		if (!foundGoal)
			print ("no path");
		else
			print ("path found");
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

	private int CompareFValues(AstarNode a, AstarNode b) {
		AstarNode temp;
		int aF = a.getF ();
		int bF = b.getF ();
		if (aF > bF)
			return -1;
		return 1;
	}

	void search(AstarNode currentNode) {
		if (currentNode.getGoal ()) {
			closedList.Add (currentNode);
			currentNode.setInClosedList (true);
			createPath(currentNode); //CREATE PATH FROM THIS NODE
			path.Reverse();
			foundGoal = true;
			return;
		}
		if (currentNode.getList ().Count == 0) {
			Debug.Log ("There are no neighbors of the current node");
			return;
		}

		//For each neighboring, pathable node of the current node, set G and F costs and add to openList
		foreach (AstarNode n in currentNode.getList()) {
			
			//if a node is already in the open list, determine if this route is faster
			if (n.getInOpenList ()) {
				int tempG;
				if (currentNode.getRow () == n.getRow () || currentNode.getCol () == n.getCol ())
					tempG = 10;
				else
					tempG = 14;
				//compare hypothetical f value of n with current f value of n
				//if predetermined path is faster than hypotherical path, do not change
				if (n.getF () < (n.getH () + currentNode.getG () + tempG))
					continue;
				else { //if new path is faster, change
					n.setParent(currentNode);
					n.setG (currentNode.getG () + tempG);
					n.setF ();
					continue;
				}
			}

			//if the node is in the closed list, continue
			if (n.getInClosedList ()) continue;

			//base case for an unchecked, pathable node
			n.setInOpenList(true);
			n.setParent (currentNode);
			if (currentNode.getRow () == n.getRow () || currentNode.getCol () == n.getCol ())
				n.setG (currentNode.getG () + 10);
			else
				n.setG (currentNode.getG () + 14);
			n.setF ();
			openList.Add (n);
		}
		//Sort openList so that first node has lowest F
		if (openList.Count == 0) {
			//Debug.Log ("CURRENT: " + currentNode.getRow () + " " + currentNode.getCol ());
			currentNode = null;
			//Debug.Log ("No items in openList");
			return; //If a path is not foundm return
		}
		openList.Sort(CompareFValues);
		openList.Reverse ();
		currentNode.setInOpenList (false);
		currentNode.setInClosedList (true);
		closedList.Add (currentNode);
		if (openList.Count != 0) {
			//Debug.Log ("CURRENT: " + currentNode.getRow () + " " + currentNode.getCol ());
			//openList[0].setParent(currentNode);
		}
		currentNode = openList [0];
		openList.RemoveAt (0);
		//print (openList.Count);
		search (currentNode);
	}

	public void createPath(AstarNode n){
		path.Add (n);
		n.setInClosedList (false);
		n.setOnPath (true);
		if (n.getParent () != null)
			createPath (n.getParent ());
		return;
	}

	public void resetNodes(List<AstarNode> list) {
		
		foreach (AstarNode n in list)
			n.reset ();
	}
}