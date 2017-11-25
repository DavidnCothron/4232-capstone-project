using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class Adaptive2DTileGround : Tile {
	[SerializeField] private Sprite preview;
	[SerializeField] private Sprite [] top_left, top_center, top_right, bottom_left, bottom_center, bottom_right,
										left, right, slope_right_up, slope_left_up, slope_right_down, slope_left_down, inner, debris;
	public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
		for (int y = -2; y <= 2; y++) {
			for (int x = -2; x <= 2; x++) {
				Vector3Int nPos = new Vector3Int (position.x + x, position.y + y, position.z);
				if (isGround(tilemap, nPos)) {
					tilemap.RefreshTile (nPos);
				}
			}
		}

	}

	private bool isGround(ITilemap tilemap, Vector3Int position) {
		//Debug.Log(this.GetInstanceID());
		return tilemap.GetTile (position) == this;
	}

	public override void GetTileData (Vector3Int position, ITilemap tilemap, ref TileData tileData) {
		string composition = string.Empty;
		for (int x = -2; x <= 2; x++) {
			for (int y = -2; y <= 2; y++) {
				if (x != 0 || y != 0) {
					if (isGround (tilemap, new Vector3Int (position.x + x, position.y + y, position.z))) {
						composition += 'G';
					} else {
						composition += 'N';
					}
				}
			}
		}

		//If a tile has been deleted, remove its collider
		if (!isGround(tilemap, position)) {
			tileData.colliderType = ColliderType.None;
		} else {
			//Debug.Log(tilemap.GetTileFlags(position));
			//tileData.colliderType = this.colliderType;
			//this.colliderType = tileData.colliderType;
		}
		
		tileData.colliderType = ColliderType.Grid;
		tileData.sprite = preview;

		/*
			04	09	13	18	23
			03	08	12	17	22
			02	07	X	16	21 => ascending order for string representation
			01	06	11	15	20
			00	05	10	14	19 

			KEY:
				- Hard Brackets: tile is a Ground Tile.
				- Soft Brackets: tile is NULL
				- No Brackets: tile can be either NULL or a Ground Tile
				- X: this tile
		*/

		#region one_tile_height
		if (composition[7] == 'G' && composition[11] == 'N' && composition[12] == 'N' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		(12)	17		22
				02		[07]	X		[16]	21
				01		06		(11)		15		20
				00		05		10		14		19 
			*/
			//between two tiles with nothing above or below
			tileData.sprite = top_center[0];
		}
		if (composition[7] == 'N' && composition[11] == 'N' && composition[12] == 'N' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		(12)	17		22
				02		(07)	X		[16]	21
				01		06		(11)	15		20
				00		05		10		14		19 
			*/
			//on the left of a one_tile_height platform
			tileData.sprite = top_left[2];
		}
		if (composition[7] == 'G' && composition[11] == 'N' && composition[12] == 'N' && composition[16] == 'N') {
			/*
				04		09		13		18		23
				03		08		(12)	17		22
				02		[07]	X		(16)	21
				01		06		(11)	15		20
				00		05		10		14		19 
			*/
			//on the right of a one_tile_height platform
			tileData.sprite = top_right[2];
		}
		#endregion
		#region top
		if (composition[7] == 'N' && composition[11] == 'G' && composition[12] == 'N' && composition[15] == 'G' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		(12)	17		22
				02		(07)	X		[16]	21
				01		06		[11]	[15]	20
				00		05		10		14		19 
			*/
			//on the top left with a tile below and to the right
			tileData.sprite = top_left[0];
		}
		if (composition[7] == 'G' && composition[11] == 'G' && composition[12] == 'N' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		(12)	17		22
				02		[07]	X		[16]	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//on the top of a block with height > 1
			tileData.sprite = top_center[0];
		}
		if (composition[6] == 'G' && composition[7] == 'G' && composition[11] == 'G' && composition[12] == 'N' && composition[16] == 'N') {
			/* SAME TYPE OF TILE AS ONE BELOW
				04		09		13		18		23
				03		08		(12)	17		22
				02		[07]	X		(16)	21
				01		[06]	[11]	15		20
				00		05		10		14		19 
			*/
			//on the top right with a tile below and to the left
			tileData.sprite = top_right[0];
		}
		if (composition[8] == 'G' && composition[7] == 'G' && composition[11] == 'G' && composition[12] == 'N' && composition[16] == 'N') {
			/*
				04		09		13		18		23
				03		[08]	(12)	17		22
				02		[07]	X		(16)	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//on the top right with a tile below and to the left
			tileData.sprite = top_right[0];
		}
		#endregion
		#region bottom
		if (composition[7] == 'N' && composition[11] == 'N' && composition[12] == 'G' && composition[16] == 'G' && composition[17] == 'G') {
			/*
				04		09		13		18		23
				03		08		[12]	[17]	22
				02		(07)	X		[16]	21
				01		06		(11)	15		20
				00		05		10		14		19 
			*/
			//on the left, bottom corner of a block
			tileData.sprite = bottom_left[2];
		}
		if (composition[7] == 'G' && composition[11] == 'N' && composition[12] == 'G' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		[07]	X		[16]	21
				01		06		(11)	15		20
				00		05		10		14		19 
			*/
			//on the bottom edge of a block
			tileData.sprite = bottom_center[1];
		}
		if (composition[7] == 'G' && composition[8] == 'G' && composition[11] == 'N' && composition[12] == 'G' && composition[16] == 'N') {
			/*
				04		09		13		18		23
				03		[08]	[12]	17		22
				02		[07]	X		(16)	21
				01		06		(11)	15		20
				00		05		10		14		19 
			*/
			//on the bottom, right corner of a block
			tileData.sprite = bottom_right[2];
		}
		#endregion
		#region left
		if (composition[7] == 'N' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		(07)	X		[16]	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//on the left edge of a block
			tileData.sprite = left[(int)UnityEngine.Random.Range(0,5)];
		}
		#endregion
		#region right
		if (composition[7] == 'G' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'N') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		[07]	X		(16)	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//on the right edge of a block
			tileData.sprite = right[(int)UnityEngine.Random.Range(0,5)];
		}
		#endregion
		#region inner
		if (composition[6] == 'G' && composition[7] == 'G' && composition[8] == 'G' && composition[11] == 'G' && 
		composition[12] == 'G' && composition[15] == 'G' && composition[16] == 'G' && composition[17] == 'G') {
			/*
				04		09		13		18		23
				03		[08]	[12]	[17]	22
				02		[07]	X		[16]	21
				01		[06]	[11]	[15]	20
				00		05		10		14		19 
			*/
			//tiles on all sides
			tileData.sprite = inner[0]; //All black tile (base case)
			int num = (int)UnityEngine.Random.Range(0,9);
			if (num <= 1) {
				tileData.sprite = inner[(int)UnityEngine.Random.Range(1,6)];
			}
			if (num == 3) {
				//find four inner
			}
		}
		#endregion
		#region inner_corners
		if (composition[7] == 'G' && composition[8] == 'N' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'G' && composition[17] == 'G') {
			/*
				04		09		13		18		23
				03		(08)	[12]	[17]	22
				02		[07]	X		[16]	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//Inner top-left corner (platform extends to the left)
			tileData.sprite = top_left[3];
		}
		if (composition[7] == 'G' && composition[8] == 'G' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'G' && composition[17] == 'N') {
			/*
				04		09		13		18		23
				03		[08]	[12]	(17)	22
				02		[07]	X		[16]	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//Inner top-right corner (platform extends to the right)
			tileData.sprite = top_right[3];
		}
		if (composition[7] == 'G' && composition[11] == 'G' && composition[12] == 'G' && composition[15] == 'N' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		[07]	X		[16]	21
				01		06		[11]	(15)	20
				00		05		10		14		19 
			*/
			//Inner lower-right corner 
			tileData.sprite = bottom_right[1];
		}
		if (composition[6] == 'N' && composition[7] == 'G' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		[07]	X		[16]	21
				01		(06)	[11]	15		20
				00		05		10		14		19 
			*/
			//Inner lower-left corner
			tileData.sprite = bottom_left[1];
		}
		#endregion

	}


	#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/Adaptive2DTileGround")]
	public static void CreateAdaptive2DTileGroundTile() {
		string path = EditorUtility.SaveFilePanelInProject ("Save Adaptive2DtileGround", "New Adaptive2DtileGround", "asset", "Save Adaptive2DtileGround", "Assets");
		if (path == "")
			return;
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Adaptive2DTileGround>(), path);
	}
	#endif
}
