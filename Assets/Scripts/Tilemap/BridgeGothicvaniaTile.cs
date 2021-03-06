﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BridgeGothicvaniaTile : Tile {

[SerializeField] private Sprite preview;
	[SerializeField] private Sprite [] bridgeTiles;
	#if UNITY_EDITOR
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

		if (!isGround(tilemap, position)) {
			tileData.colliderType = ColliderType.None;
		}

		tileData.colliderType = ColliderType.Grid;
		tileData.sprite = preview;

		if (composition[7] == 'N' && composition[11] == 'G' && composition[12] == 'G' && composition[16] == 'N') {
			/*
				04		09		13		18		23
				03		08		[12]	17		22
				02		07		X		16		21
				01		06		[11]	15		20
				00		05		10		14		19 
			*/
			//between two tiles with nothing above or below
			tileData.sprite = bridgeTiles[13];
		}
	}

	#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/BridgeGothicvaniaTile")]
	public static void CreateBridgeGothicVaniaTile() {
		string path = EditorUtility.SaveFilePanelInProject ("Save BridgeGothicVaniaTile", "New BridgeGothicVaniaTile", "asset", "Save BridgeGothicVaniaTile", "Assets");
		if (path == "")
			return;
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BridgeGothicvaniaTile>(), path);
	}
	#endif

	#endif
}

