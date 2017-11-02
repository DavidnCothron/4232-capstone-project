using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class CaveTileGround : Tile {
	[SerializeField] private Sprite[] center, bottom, left, right;
	[SerializeField] private Sprite preview;

	public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
		for (int y = -1; y <= 1; y++) {
			for (int x = -1; x <= 1; x++) {
				Vector3Int nPos = new Vector3Int (position.x + x, position.y + y, position.z);
				if (isGround(tilemap, nPos)) {
					tilemap.RefreshTile (nPos);
				}
			}
		}
	}

	public override void GetTileData (Vector3Int position, ITilemap tilemap, ref TileData tileData) {
		string composition = string.Empty;
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x != 0 || y != 0) {
					if (isGround (tilemap, new Vector3Int (position.x + x, position.y + y, position.z))) {
						composition += 'G';
					} else {
						composition += 'N';
					}
				}
			}
		}
		tileData.sprite = preview;

		/*
		 * 3	5	8
		 * 2	X	7 => 12345678
		 * 1	4	6
		 */

		#region bottom_of_platform
		//Bottom of platform
		if (composition.Equals("NNNNNNGN") || composition.Equals("NNNNGNGG") || 
			composition.Equals("NNGNGNGG")) { // Only something to the right
			tileData.sprite = left[0];
		}
		if (composition.Equals("NGGNGNNN") || composition.Equals("NGGNGNNG")) { // Only something to the left
			tileData.sprite = right[0];
		}
		if (composition.Equals("NGNNGNGN") || composition.Equals("NGGNGNGG")) { //Bottom of platform not on edge
			tileData.sprite = center[(int)Random.Range(4,7)];
		}
			
		/*
			(2)	[4]	(7)
			[1]	X	[6]
			(0)	3	(5)
		*/

		if (composition[1] == 'G' && composition[4] == 'G' && composition[6] == 'G' && composition[3] == 'N' &&
			(composition[5] == 'G' || composition[0] == 'G')) {
			tileData.sprite = center[(int)Random.Range(4,7)];
		}

		/*
			2	4	(7)
			1	X	[6]
			0	(3)	(5)
		*/
		//does not work for all occasions
//		if (composition.Equals("NNNNNNGN") || composition.Equals("NNNNNNGG")) {
//			tileData.sprite = left[5];
//		}
		if (composition[0] == 'N' && composition[1] == 'N' && composition[2] == 'N' && composition[4] == 'N'&& composition[6] == 'G') {
			tileData.sprite = left[5];
		}

		/*
			(2)	[4]	([7]) <-----> have not decided on 7
			1	X	[6]
			(0)	3	(5)
		*/
		if (composition[1] == 'N' && composition[3] == 'N' && composition[4] == 'G' && composition[6] == 'G' /*&& composition[7] == 'G'*/){
			tileData.sprite = left[7];
		}


		/*
			([2])	[4]		([7]) <-------------> have not decided on 2 or 7
			[1]		X		6
			(0)		3		(5)
		*/

		if (composition[1] == 'G' && composition[3] ==  'N' && composition[4] == 'G' && composition[6] == 'N') {
			tileData.sprite = right[7];
		}

		/*
			(2)	[4]	(7)
			1	X	6
			(0)	3	(5)
		*/
		if (composition[1] == 'N' && composition[3] == 'N' && composition[6] == 'N' && composition[4] == 'G') {
			tileData.sprite = center[(int)Random.Range(4,7)];
		}
		#endregion
		#region top_of_platform
		//Top of platform
		/*
			2	4	(7)
			1	X	[6]
			(0)	[3]	(5)
		*/

		if(composition[1] == 'N' && composition[2] == 'N' && composition[3] == 'G' &&
			composition[4] == 'N' && composition[6] == 'G') {
			int index = (int)Random.Range(1,2);
			switch(index) {
			case 1:
				tileData.sprite = left [10];
				break;
			case 2:
				tileData.sprite = left [10];
				break;
			default:
				break;
			}
		}

//		if(composition.Equals("NNNGNGGN") || composition.Equals("NNNGNNGN") ||
//			composition.Equals("NNNGNGGG") || composition.Equals("NNNGNNGG") ||
//			composition.Equals("GNNGNGGN") || composition.Equals("GNNGNNGN") ||
//			composition.Equals("GNNGNGGG") || composition.Equals("GNNGNNGG")) { //Top left of platform
//			int index = (int)Random.Range(1,2);
//			switch(index) {
//			case 1:
//				tileData.sprite = left [10];
//				break;
//			case 2:
//				tileData.sprite = left [10];
//				break;
//			default:
//				break;
//			}
//		}
		/*
			(2)	4	7
			[1]	X	6
			(0)	[3]	(5)
		*/
		if (composition[1] == 'G' && composition[3] == 'G' && composition [4] == 'N' &&
			composition[6] == 'N' && composition[7] == 'N') {
			int index = (int)Random.Range(1,2);
			switch(index) {
			case 1:
				tileData.sprite = right [10];
				break;
			case 2:
				tileData.sprite = right [11];
				break;
			default:
				break;
			}
		}

		/*
			(2)	4	(7)
			1	X	6
			(0)	[3]	(5)
		*/

		if(composition[1] == 'N' && composition[3] == 'G' && composition [4] == 'N'
			&& composition[6] == 'N') {
			tileData.sprite = center[(int)Random.Range(15,16)];
		}

		//Center of platfrom
		//NOTHING ABOVE TILE IN 3,5,8 INDICES
		if (composition.Equals("GGNGNGGN") || composition.Equals("NGNGNGGN") || 
			composition.Equals("NGNGNNGN") || composition.Equals("GGNGNNGN")) {
			tileData.sprite = center [(int)Random.Range (0, 1)];
		}

		//ABOVE TILE IN 8th INDEX
		/*
			3	5	[8]
			[2]	X	[7]
			(1)	[4]	(6)
		*/

		if (composition.Equals("GGNGNGGG") || composition.Equals("NGNGNGGG") ||
			composition.Equals("NGNGNNGG") || composition.Equals("GGNGNNGG")) {
			tileData.sprite = center [(int)Random.Range (0, 1)];
		}

		//ABOVE TILE IN 3rd INDEX
		/*
			[3]	5	 8
			[2]	X	[7]
			(1)	[4]	(6)
		*/

		if (composition.Equals("GGGGNGGN") || composition.Equals("NGGGNGGN") ||
			composition.Equals("NGGGNNGN") || composition.Equals("GGGGNNGN")) {
			tileData.sprite = center [(int)Random.Range (0, 1)];
		}

		//ABOVE TILE IN 3rd AND 8th INDICES
		/*
			[3]	5	[8]
			[2]	X	[7]
			(1)	[4]	(6)
		*/

		if (composition.Equals("GGGGNGGG") || composition.Equals("NGGGNNGG") ||
			composition.Equals("GGGGNNGG") || composition.Equals("NGGGNGGG")) {
			tileData.sprite = center [(int)Random.Range (0, 1)];
		}


		#endregion
		#region one_tile_height
		//between tiles
		//BELOW TILE IN 1st and/or 6th INDICES
		/*
			3	5	8
			[2]	X	[7]
			(1)	4	(6)
		*/
		if (composition.Equals("NGNNNNGN") || composition.Equals("GGNNNGGN") || 
			composition.Equals ("GGNNNNGN") || composition.Equals("NGNNNGGN")) {
			tileData.sprite = center [(int)Random.Range (3, 4)];
		}

		//ABOVE TILE IN 3rd and/or 8th INDICES
		/*
			(3)	5	(8)
			[2]	X	[7]
			1	4	6
		*/
		if(composition.Equals("NGGNNNGG") || composition.Equals("NGNNNNGG") ||
			composition.Equals("NGGNNNGN")) {
			tileData.sprite = center [(int)Random.Range (3, 4)];
		}

		/*
			(3)	5	(8)
			[2]	X	[7]
			[1]	4	6
		*/
		if (composition.Equals("GGGNNNGG") || composition.Equals("GGNNNNGN") ||
			composition.Equals("GGGNNNGN") || composition.Equals("GGNNNNGG")) {
			tileData.sprite = center [(int)Random.Range (3, 4)];
		}

		/*
			(3)	5	(8)
			[2]	X	[7]
			1	4	[6]
		*/

		if (composition.Equals("NGGNNGGG") || composition.Equals("NGNNNGGN") ||
			composition.Equals("NGGNNGGN") || composition.Equals("NGNNNGGG")) {
			tileData.sprite = center [(int)Random.Range (3, 4)];
		}

		//rightmost tile
		if (composition.Equals("GGNNNNNN") || composition.Equals("NGNNNNNN")){
			tileData.sprite = right [6];
		}
		//leftmost tile
		if (composition.Equals("NNNNNGGN") || composition.Equals("NNNNNNGN")) {
			tileData.sprite = left[6];
		}
		#endregion
		#region side_of_platform
		/*
			(2)	[4]	(7)
			[1]	X	6
			(0)	[3]	(5)
		*/

		if(composition[1] == 'G' && composition[3] == 'G' && composition[4] == 'G' &&
			composition[6] == 'N') {
			tileData.sprite = right[(int)Random.Range(4,5)];
		}

		/*
			(2)	[4]	(7)
			1	X	[6]
			(0)	[3]	(5)
		*/

		if(composition[1] == 'N' && composition[3] == 'G' && composition[4] == 'G' && composition[6] == 'G') {
			int index = (int)Random.Range(1,2);
			switch(index) {
			case 1:
				tileData.sprite = left [3];
				break;
			case 2:
				tileData.sprite = left [4];
				break;
			default:
				break;
			}
		}

		/*
			2	4	7
			1	X	6
			0	3	5
		*/

		//if ()
		#endregion
		#region one_tile_width
		/*
			(2)	[4]	(7)
			1	X	6
			(0)	[3]	(5)
		*/
		if(composition[1] == 'N' && composition[3] == 'G' && composition[4] == 'G' && composition[6] == 'N') {
			int index = (int)Random.Range(1,2);
			switch(index) {
			case 1:
				tileData.sprite = center [12];
				break;
			case 2:
				tileData.sprite = center [13];
				break;
			default:
				break;
			}
		}


		#endregion
	}



	private bool isGround(ITilemap tilemap, Vector3Int position) {
		return tilemap.GetTile (position) == this;
	}

	#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/CaveTileGround")]
	public static void CreateCavetileGroundTile() {
		string path = EditorUtility.SaveFilePanelInProject ("Save CavetileGround", "New CavetileGround", "asset", "Save CavetileGround", "Assets");
		if (path == "")
			return;
		AssetDatabase.CreateAsset (ScriptableObject.CreateInstance<CaveTileGround> (), path);
	}
	#endif
}
