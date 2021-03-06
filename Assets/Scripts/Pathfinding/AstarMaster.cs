﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class AstarMaster : MonoBehaviour {
	#region singleton
	private static AstarMaster a_Instance = null;
	public static AstarMaster instance {
		get {
			if (a_Instance == null)
				a_Instance = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AstarMaster> ();
			if (a_Instance == null) {
				a_Instance = GameObject.FindGameObjectWithTag ("MainCamera").AddComponent (typeof(AstarMaster)) as AstarMaster;
			}
			if (a_Instance == null) {
				GameObject obj = new GameObject ("AstarMaster");
				a_Instance = obj.AddComponent (typeof(AstarMaster)) as AstarMaster;
				Debug.Log ("No main camera exists in scene; AstarMaster was generated automatically on a new GameObject.");
			}
			return a_Instance;
		}
	}
	#endregion

	#region data
	public GameObject nodeObject;
	private Vector3 rayStart;
	LayerMask Background;
	#endregion

	void Awake() {
		//Ignore background layer
		Background = ~(1 << LayerMask.NameToLayer ("RoomBackground"));
	}
		
	public List<AstarNode> getOccupiedNodes(List<AstarNode> nodeList, string key){
		//for each node in room, if a circle-cast from a node hits the object with the appropriate string, then that node is being occupied by the obj.
		List <AstarNode> occupiedNodes = new List<AstarNode>();
		foreach (AstarNode n in nodeList) {
			rayStart = new Vector3 (n.getLocation ().x, n.getLocation ().y, UnityDepth.instance.unityDepth);
			RaycastHit2D hit = Physics2D.CircleCast (rayStart, .25f, (Vector3.forward * UnityDepth.instance.unityDepth), 1f, Background);
			if (hit != null && hit.collider != null) {
				if (hit.collider.tag == key) {
					n.setIsOccupied (true);
					occupiedNodes.Add (n);
				}
				AstarUser u = hit.collider.GetComponent<AstarUser> ();
				if (u != null && u.getGuidID () == key) {
					n.setIsOccupied (true);
					occupiedNodes.Add (n);
				}
			} else {
				n.setIsOccupied (false);
			}
		}
		return occupiedNodes;
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
						RaycastHit2D hit = Physics2D.Raycast (neighbor.getLocation (), node.getLocation (), Vector3.Distance(node.getLocation(), neighbor.getLocation()), Background );
						if (hit == null || (hit != null && (hit.collider == null || hit.collider.tag == "Player" || hit.collider.tag == "Enemy"))) 
							node.getList ().Add (neighbor);
					}
				}
			}
		}
	}

	public void setHCosts(List<AstarNode> list, AstarNode startNode, AstarNode endNode){

	}

	public void showRoomNodes(List<AstarNode> rn) {
		GameObject obj;
		foreach (AstarNode n in rn) {
//			obj = GameObject.Instantiate (nodeObject, n.getLocation (), Quaternion.identity);
//			obj.layer = 2;
//			n.setNodeObj (obj);
			n.getObject().SetActive(true);
		}
	}

	public void colorRoomNodes(List<AstarNode> rn){
		//Debug.Log (occuNodes.Count);
		if (rn.Count != 0) {
			foreach (AstarNode n in rn) {
				if (n.getIsOccupied ()) {
					if (n.getObject () != null) {
						n.getObject ().GetComponent<SpriteRenderer> ().color = Color.red;

					}
				} else {
					if (n.getObject () != null)
						n.getObject ().GetComponent<SpriteRenderer> ().color = Color.green;
				}
				if (n.getGoal ()) {
					//print ("goal");
					//print ("GOAL: " + n.getRow() + " " + n.getCol());
					n.getObject ().GetComponent<SpriteRenderer> ().color = Color.cyan;
				}
				if (n.getStart()) {
					//Debug.Log (n.getH());
					n.getObject().GetComponent<SpriteRenderer>().color = Color.red;
				}
				if (n.getInClosedList ()) {
					//Debug.Log ("closed");
					n.getObject ().GetComponent<SpriteRenderer> ().color = Color.black;
				}
				if (n.getInOpenList()) {
					n.getObject ().GetComponent<SpriteRenderer> ().color = Color.yellow;
				}
				if(n.getOnPath()) {
					n.getObject ().GetComponent<SpriteRenderer> ().color = Color.magenta;
				}
			}
		}
	}

	public string createNewGuidID(){
		Guid g = Guid.NewGuid ();
		string GuidString = Convert.ToBase64String (g.ToByteArray ());
		GuidString = GuidString.Replace ("=", "");
		GuidString = GuidString.Replace ("+", "");
		return GuidString;
	}

	public void showNodeNeighbors(List<AstarNode> rn, int nodeNum) {
		//Debug.Log (rn [nodeNum].toString ());
		GameObject obj = rn[nodeNum].getObject();
		if (obj != null) {
			obj.GetComponent<SpriteRenderer> ().color = Color.red;
			obj.layer = 2;
		}
		foreach (AstarNode n in rn[nodeNum].getList()) {
			//Debug.Log (n.toString ());
			if (n.getObject () == null) {
				obj = GameObject.Instantiate (nodeObject, n.getLocation (), Quaternion.identity);
				obj.layer = 2;
			} else
				obj = n.getObject ();
			obj.GetComponent<SpriteRenderer> ().color = Color.blue;
		}
	}

	public List<AstarNode> maintainTotalOccupiedNodes(List<AstarNode> rn, Dictionary<string, List<AstarNode>> dict, List<AstarNode> total, List<string> keys){
		if (total.Count != 0)
			total.Clear ();
		foreach (string s in keys) {
			dict[s] = AstarMaster.instance.getOccupiedNodes (rn, s);
			foreach (AstarNode n in dict[s])
				total.Add (n);
		}
		return total;
	}

	void OnApplicationQuit(){
		a_Instance = null;
	}

}
