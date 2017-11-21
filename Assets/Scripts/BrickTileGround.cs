using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class BrickTileGround : Tile {
	[SerializeField] private Sprite[] top_outer, top_inner, bottom_outer, bottom_inner, left_outer, left_inner, right_outer, right_inner, inner, inner_transparent_background,
									top_outer_transparent_background, top_inner_transparent_background, bottom_outer_transparent_background, bottom_inner_transparent_background,
									left_outer_transparent_background, left_inner_transparent_background, right_outer_transparent_background, right_inner_transparent_background;
	[SerializeField] private Sprite preview;

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
		

		tileData.colliderType = ColliderType.Grid;
		tileData.sprite = top_outer_transparent_background[5];

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
		*/
		
		#region one_tile_height
		
		if (composition[7] == 'G' && composition[11] == 'N' && composition[12] == 'N' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		(12)	17		22
				02		[07]	X		[16]	21
				01		06		(11)	15		20
				00		05		10		14		19 
			*/
			//between two tiles with nothing above or below
			tileData.sprite = top_outer_transparent_background[1];
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
			tileData.sprite = top_outer_transparent_background[3];
		}
		if (composition[7] == 'G' && composition[11] == 'N' && composition[12] == 'N' && composition[16] == 'N') {
			/*
				04		09		13		18		23
				03		08		(12)	17		22
				02		[07]	X		(16)	21
				01		06		(11)	15		20
				00		05		10		14		19 
			*/
			//on the left of a one_tile_height platform
			tileData.sprite = top_outer_transparent_background[4];
		}
		#endregion
		#region top_edge
		if (composition[7] == 'N' && composition[11] == 'G' && composition[12] == 'N' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		(12)	17		22
				02		(07)	X		[16]	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//on the top left with a tile below and to the right
			tileData.sprite = top_outer[0];
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
			tileData.sprite = top_outer[1];
		}
		if (composition[7] == 'G' && composition[11] == 'G' && composition[12] == 'N' && composition[16] == 'N') {
			/*
				04		09		13		18		23
				03		08		(12)	17		22
				02		[07]	X		(16)	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//on the top right with a tile below and to the left
			tileData.sprite = top_outer[2];
		}
		#endregion
		#region left_edge
		if (composition[7] == 'N' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		(07)	X		[16]	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//on the left edge of a block
			tileData.sprite = left_outer[(int)Random.Range(0,4)];
		}
		#endregion
		#region right_edge
		if (composition[7] == 'G' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'N') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		[07]	X		(16)	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//on the right edge of a block
			tileData.sprite = right_outer[(int)Random.Range(0,4)];
		}
		#endregion
		#region bottom_edge
		if (composition[7] == 'N' && composition[11] == 'N' && composition[12] == 'G' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		(07)	X		[16]	21
				01		06		(11)	15		20
				00		05		10		14		19 
			*/
			//on the left, bottom corner of a block
			tileData.sprite = bottom_outer[5];
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
			tileData.sprite = bottom_outer[4];
		}
		if (composition[7] == 'G' && composition[11] == 'N' && composition[12] == 'G' && composition[16] == 'N') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		[07]	X		(16)	21
				01		06		(11)	15		20
				00		05		10		14		19 
			*/
			//on the bottom, right corner of a block
			tileData.sprite = bottom_outer[3];
		}
		#endregion
		#region inner
		if (composition[7] == 'G' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'G') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		[07]	X		[16]	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//tiles on all sides
			tileData.sprite = inner[0]; //All black tile (base case)
			int num = (int)Random.Range(0,9);
			if (num <= 2) {
				tileData.sprite = inner[(int)Random.Range(1,3)];
			}
			if (num == 3) {
				//find four inner
			}
		}
		#endregion
		#region inner_edge
		if (composition[7] == 'G' && composition[8] == 'N' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'G' && composition[17] == 'G') {
			/*
				04		09		13		18		23
				03		(08)	[12]	[17]	22
				02		[07]	X		[16]	21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//Inner top-left corner (platform extends to the left)
			tileData.sprite = top_inner[5];
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
			tileData.sprite = top_inner[4];
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
			tileData.sprite = bottom_inner[0];
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
			tileData.sprite = bottom_inner[1];
		}
		#endregion
	}

	private bool isGround(ITilemap tilemap, Vector3Int position) {
		return tilemap.GetTile (position) == this;
	}

	private void findFourInner(ITilemap tilemap, Vector3Int position) {
		Vector4 choices = new Vector4(1f, 1f, 1f, 1f);
		for (int x = -1; x <= 1; x += 2) {
			for (int y = -1; y <= 1; y += 2) {
				if (isGround (tilemap, new Vector3Int (position.x + x, position.y + y, position.z))) {
					//if a diagonal tile is a ground tile, check its surrounding tiles
					Vector3Int nPos = new Vector3Int (position.x + x, position.y + y, position.z);
				}
			}
		}
	}

	#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/BrickTileGround")]
	public static void CreateBrickTileGroundTile() {
		string path = EditorUtility.SaveFilePanelInProject ("Save BricktileGround", "New BricktileGround", "asset", "Save BricktileGround", "Assets");
		if (path == "")
			return;
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BrickTileGround>(), path);
	}
	#endif
}
