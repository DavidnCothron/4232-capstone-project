using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode {
	private int row, col, f, g, h, type;
	private Vector3 location;
	private bool start, goal, onPath, visited, inOpenList, inClosedList, isOccupied;
	private AstarNode parent;
	private GameObject nodeSquare;
	[SerializeField] private List<AstarNode> neighbors = new List<AstarNode> ();

	public void reset() {
		onPath = visited = inOpenList = inClosedList = false;
		parent = null;
	}

	#region constructors
	public AstarNode(){}
	public AstarNode(int r, int c, Vector3 loc) {
		row = r;
		col = c;
		location = loc;
	}
	public AstarNode(int r, int c, Vector3 loc, GameObject obj, List<AstarNode> neighborNodes){
		row = r;
		col = c;
		location = loc;
		nodeSquare = obj;
		neighbors = neighborNodes;
	}
	#endregion
	#region overides
	public bool compareTo(AstarNode other) {
		if (row == other.getRow () && col == other.getCol ())
			return true;
		return false;
	}

	public string toString(){
		return "Node: " + row + "_" + col + ", inOpenList: " + getInOpenList() + ", inClosedList: " + getInClosedList() + ", goal?: " + getGoal() + ", start?: " + getStart();
	}
	#endregion
	#region setters
	public void setParameters(int r, int c, Vector3 loc) {
		row = r;
		col = c;
		location = loc;
	}

	public void setNodeObj(GameObject obj){
		nodeSquare = obj;
	}

	public void setIsOccupied(bool b){
		isOccupied = b;
	}

	public void setF() {
		f = g + h;
	}

	public void setG(int val) {
		g = val;
	}

	public void setH(int val) {
		h = val;
	}

	public void setParent (AstarNode n) {
		parent = n;
	}

	public void setStart (bool s) {
		start = s;
	}

	public void setGoal (bool g) {
		goal = g;
	}

	public void setVisited (bool v) {
		visited = v;
	}

	public void setOnPath (bool p) {
		onPath = p;
	}

	public void setInOpenList (bool b) {
		//Debug.Log ("open");
		inOpenList = b;
	}

	public void setInClosedList (bool b) {
		inClosedList = b;
	}

	public void setLoaction (Vector3 loc) {
		location = loc;
	}
	#endregion
	#region getters
	public bool getInClosedList(){
		return inClosedList;
	}

	public bool getInOpenList(){
		return inOpenList;
	}

	public int getF(){
		return f;
	}

	public int getH(){
		return h;
	}

	public int getG(){
		return g;
	}

	public int getRow(){
		return row;
	}

	public int getCol(){
		return col;
	}

	public int getType(){
		return type;
	}

	public AstarNode getParent(){
		return parent;
	}

	public List<AstarNode> getList(){
		return neighbors;
	}

	public bool getGoal(){
		return goal;
	}

	public Vector3 getLocation() {
		return location;
	}

	public GameObject getObject() {
		return nodeSquare;
	}

	public bool getIsOccupied() {
		return isOccupied;
	}

	public bool getStart() {
		return start;
	}

	public bool getOnPath() {
		return onPath;
	}
	#endregion

}
