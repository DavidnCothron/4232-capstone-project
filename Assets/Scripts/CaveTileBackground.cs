using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
public class CaveTileBackground : Tile {

	[SerializeField] private Sprite[] backgroundSprites;
	[SerializeField] private Sprite preview;

	private bool isBackground(ITilemap tilemap, Vector3Int position) {
		return tilemap.GetTile (position) == this;
	}

	public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
		for (int y = -1; y <= 1; y++) {
			for (int x = -1; x <= 1; x++) {
				Vector3Int nPos = new Vector3Int (position.x + x, position.y + y, position.z);
				if (isBackground(tilemap, nPos)) {
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
					if (isBackground (tilemap, new Vector3Int (position.x + x, position.y + y, position.z))) {
						composition += 'G';
					} else {
						composition += 'N';
					}
				}
			}
		}

		tileData.sprite = backgroundSprites[(int)Random.Range(0,3)];
	}

	#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/CaveTileBackground")]
	public static void CreateCavetileBackgroundTile() {
		string path = EditorUtility.SaveFilePanelInProject ("Save CavetileBackground", "New CavetileBackground", "asset", "Save CavetileBackground", "Assets");
		if (path == "")
			return;
		AssetDatabase.CreateAsset (ScriptableObject.CreateInstance<CaveTileBackground> (), path);
	}
	#endif
}
