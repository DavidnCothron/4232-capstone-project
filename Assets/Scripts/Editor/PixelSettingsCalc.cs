using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PixelSettingsCalc : EditorWindow {

	bool groupEnabled;
	int layerNum = 1;
	int centerToEdge = 8;
	int PPU = 32;
	int displayPPU, displayZ;
	string calculateZPPU = "Calculate";
	float screenResWidth, screenResHeight;
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
		GUILayout.TextArea ("PPU: " + layerInfo.Key + " Z-distance: " + layerInfo.Value);


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
