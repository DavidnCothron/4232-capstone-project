using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode {
	private int row, col, f, g, h, type;
	private Vector3 location;
	private bool start, goal, onPath, visited, inOpenList, inClosedList;
	private AstarNode parent;
	private GameObject nodeSquare;
	[SerializeField] private List<AstarNode> neighbors = new List<AstarNode> ();

	public void reset() {
		start = goal = onPath = visited = inOpenList = inClosedList = false;
		parent = null;
	}
		
	public void setParameters(int r, int c, Vector3 loc) {
		row = r;
		col = c;
		location = loc;
	}

	public void setNodeObj(GameObject obj){
		nodeSquare = obj;
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
		inOpenList = b;
	}

	public void setInClosedList (bool b) {
		inClosedList = b;
	}

	public void setLoaction (Vector3 loc) {
		location = loc;
	}

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

	public string toString(){
		return "Node: " + row + "_" + col;
	}
}
