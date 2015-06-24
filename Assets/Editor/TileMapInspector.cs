using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TileMap))]
public class TileMapInspector : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		TileMap myScript = (TileMap) target;

		if(GUILayout.Button("Recreate Map",  GUILayout.ExpandWidth(false)))
		{
			myScript.buildMap();
		}
	}
}
