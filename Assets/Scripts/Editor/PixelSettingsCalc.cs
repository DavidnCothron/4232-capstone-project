using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PixelSettingsCalc : EditorWindow {

	bool groupEnabled;
	int layerNum = 1;
	int centerToEdge = 8;
	int PPU = 32;
	int squarePixelSubdivision, pixelsInTile;
	string calculateZPPU = "Calculate Z and PPU";
	string calculateTilemapScale = "Calculate Tilemap Scale";
	float screenResWidth, screenResHeight, tilemapScale, gridSize, unitsPerPixel;
	//Matrix4x4 viewProjection = Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix;
	double viewProjection_0 = 2.099278;
	KeyValuePair<int, float> layerInfo = new KeyValuePair<int, float>();
	Dictionary <int, float> layerAndPPU = new Dictionary <int,float>();

	[MenuItem ("Window/Pixel Settings")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(PixelSettingsCalc));
	}

	void OnGUI() {
		GUILayout.Label ("Calculate Z-distance for Layer and PPU of Sprites", EditorStyles.boldLabel);
		GUILayout.TextArea("Negative Layer numbers indicate layers behind the ground layer, positive Layer numbers indicate layers in front of the ground layer.");
		screenResWidth = EditorGUILayout.FloatField("Screen Resolution Width", screenResWidth);
		screenResHeight = EditorGUILayout.FloatField("Screen Resolution Height", screenResHeight);
		PPU = EditorGUILayout.IntField("Pixels Per Unit", PPU);
		layerNum = EditorGUILayout.IntField("Layer Number", layerNum);

		if (GUILayout.Button(calculateZPPU)) {
			PPUzDistance();
			layerInfo = getLayer(layerNum);
			//GUILayout.Label("PPU: " + layerInfo.Key + " Z-depth: " + layerInfo.Value);
		}

		GUILayout.TextArea ("PPU: " + layerInfo.Key + "\nZ-distance: " + layerInfo.Value);
		GUILayout.Label("Grid Settings Calculator", EditorStyles.boldLabel);
		squarePixelSubdivision = EditorGUILayout.IntField("Number of desired pixels per tile: ", squarePixelSubdivision);
		pixelsInTile = EditorGUILayout.IntField("Number of actual pixels per tile", pixelsInTile);
		
		if (pixelsInTile == 0) pixelsInTile = 1;

		if (GUILayout.Button(calculateTilemapScale)) {
			if (pixelsInTile == 0) pixelsInTile = 1;
			tilemapScale = (float)squarePixelSubdivision/(float)pixelsInTile;
			unitsPerPixel = (float)PPU/(float)pixelsInTile;
			gridSize = tilemapScale/unitsPerPixel;
		}

		GUILayout.TextArea("Tilemap Scale: " + tilemapScale + "\nGrid Size: " + gridSize);


	}

	void PPUzDistance() {
		layerAndPPU.Clear();
		for (int i = 0; i < 128; i++) {
			float screenWidth = screenResWidth / i / centerToEdge;
			layerAndPPU.Add(i, (float)(viewProjection_0 * screenWidth));
		}
	}

	KeyValuePair<int, float> getLayer(int ln) {
		float unityDepth_GUI = (float)(viewProjection_0 * (screenResWidth/PPU/centerToEdge));
		int newPPU = Mathf.Abs(layerNum + PPU);
		if (layerAndPPU.ContainsKey(newPPU)){
			return new KeyValuePair<int, float> (newPPU, layerAndPPU[newPPU] - unityDepth_GUI);
		}
		return new KeyValuePair<int, float>(0,0f);
	}
}
