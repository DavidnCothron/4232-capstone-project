using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteSettings : MonoBehaviour {

	private KeyValuePair<int,float> layerInfo;
	private Sprite sprite;
	private SpriteLayer layer;
	public int layerNum;
	public Texture2D texture;

	//Attach to sprite to find the appropriate z-depth and pixels-per-unit for a given sprite.
	//When you change layer number in the editor, this will run.
	//0 is the player layer. numbers > 0 = closer to camera. numbers < 0 = further away from camera.
	void OnValidate() {
		layerNum = layerNum;
		layerInfo = UnityDepth.instance.getLayer (layerNum);
		if (layerInfo.Key > 0) {
			sprite = GetComponent<SpriteRenderer> ().sprite;
			texture = UnityDepth.instance.textureFromSprite (sprite);
			createSprite ();
			transform.position = new Vector3(transform.position.x, transform.position.y, layerInfo.Value);
			GetComponent<SpriteRenderer> ().sprite = sprite;

		}
	}

	void createSprite(){
		//Cannot change the Rect starting point. As such, this should be used during run-time to get the proper values for z.
		sprite = Sprite.Create (texture, new Rect (0f, 0f, (float)texture.width, (float)texture.height), new Vector2 (0, 0), layerInfo.Key, 0, SpriteMeshType.FullRect);
		sprite.texture.filterMode = FilterMode.Point;

	}

	Vector3 getWorldPositionBehindSprite(){
		Ray ray = Camera.main.ScreenPointToRay (transform.position);
		Plane xy = new Plane (Vector3.forward, new Vector3 (0, 0, layerInfo.Value)); 
		float distance;
		xy.Raycast (ray, out distance);
		Debug.Log (ray.GetPoint(distance));
		return ray.GetPoint (distance);
	}

}
