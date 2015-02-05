using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TileMap))]
public class TileMapInspector : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		TileMap myScript = (TileMap) target;

		Vector2 roomWidth = EditorGUILayout.Vector2Field ("Room Width", new Vector2 (myScript.roomWidthRange [0], myScript.roomWidthRange [1]));
		myScript.roomWidthRange [0] = (int)roomWidth.x;
		myScript.roomWidthRange [1] = (int)roomWidth.y;

		Vector2 roomHeight = EditorGUILayout.Vector2Field ("Room Height", new Vector2 (myScript.roomHeightRange [0], myScript.roomHeightRange [1]));
		myScript.roomHeightRange [0] = (int)roomHeight.x;
		myScript.roomHeightRange [1] = (int)roomHeight.y;
		
		if(GUILayout.Button("Recreate Map",  GUILayout.ExpandWidth(false)))
		{
			myScript.buildMap();
		}
	}
}
