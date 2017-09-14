using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarMaster : MonoBehaviour {

	private static AstarMaster a_Instance = null;
	public static AstarMaster instance {
		get {
			if (a_Instance == null)
				a_Instance = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AstarMaster> ();
			if (a_Instance == null)
				a_Instance = GameObject.FindGameObjectWithTag ("MainCamera").AddComponent (typeof(AstarMaster)) as AstarMaster;
			if (a_Instance == null) {
				GameObject obj = new GameObject ("AstarMaster");
				a_Instance = obj.AddComponent (typeof(AstarMaster)) as AstarMaster;
				Debug.Log ("No main camera exists in scene; AstarMaster was generated automatically on a new GameObject.");
			}
			return a_Instance;
		}
	}

	private Vector3 rayStart;
		
	void Awake() {
//		rayStart = new Vector3 (0f, 0f, -UnityDepth.instance.unityDepth);
	}

	void Update () {}

	public List<AstarNode> getOccupiedNodes(List<AstarNode> nodeList, string tag){
		//for each node in room, if a circle-cast from a node hits the object with the appropriate string, then that node is being occupied by the obj.
		List <AstarNode> occupiedNodes = new List<AstarNode>();
		foreach (AstarNode n in nodeList) {
			rayStart = new Vector3 (n.getLocation ().x, n.getLocation ().y, UnityDepth.instance.unityDepth);
			RaycastHit2D hit = Physics2D.CircleCast (rayStart, 2f, (Vector3.forward * UnityDepth.instance.unityDepth));
			if (hit != null && hit.collider != null) {
				if (hit.collider.tag == tag) 
					occupiedNodes.Add (n);
			}
		}
		return occupiedNodes;
	}

	void OnApplicationQuit(){
		a_Instance = null;
	}

	//ToDo: AstarUser script; get an object to constantly track towards the player.
}
