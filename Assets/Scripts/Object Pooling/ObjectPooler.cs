using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

	public GameObject objectType;
	public int poolSize;
	public bool canGrow;

	List<GameObject> pool;

	// Use this for initialization
	void Start () {

		pool = new List<GameObject> ();

		for (int i = 0; i < poolSize; i++)
		{
			GameObject obj = (GameObject)Instantiate (objectType);
			obj.SetActive (false);
			pool.Add (obj);
		}
		
	}

	public GameObject PopFromPool(){
		for (int i = 0; i < pool.Count; i++)
		{
			if (!pool [i].activeInHierarchy)
			{
				return pool [i];
			}
		}

		if (canGrow)
		{
			GameObject obj = (GameObject)Instantiate (objectType);
			pool.Add (obj);
			return obj;
		}

		return null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
