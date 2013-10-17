using System;
using UnityEditor;
using UnityEngine;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CanEditMultipleObjects]
[CustomEditor(typeof(GUIContentHolder))]
public class GUIContentEditor : Editor
{	
	SerializedProperty positionProp;
	SerializedProperty visibleProp;
	SerializedProperty guiStylePrefabProp;
	SerializedProperty depthProp;
	SerializedProperty colorProp;
	SerializedProperty autoXPositionProp;
	SerializedProperty contentProp;
	
    public void OnEnable () {
    	SerializedProperty positionProp = serializedObject.FindProperty("position");
		SerializedProperty visibleProp = serializedObject.FindProperty("visibleProp");
		SerializedProperty guiStylePrefabProp = serializedObject.FindProperty("guiStylePrefab");
		SerializedProperty depthProp = serializedObject.FindProperty("depth");
		SerializedProperty colorProp = serializedObject.FindProperty("color");
		SerializedProperty autoXPositionProp = serializedObject.FindProperty("autoXPosition");
		SerializedProperty contentProp = serializedObject.FindProperty("content");
	}
	
	public override void OnInspectorGUI() {
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update();
		
		// Show the custom GUI controls
		EditorGUILayout.PropertyField(guiStylePrefabProp);
		EditorGUILayout.PropertyField(depthProp);
		EditorGUILayout.PropertyField(colorProp);
		EditorGUILayout.PropertyField(autoXPositionProp);
		
		Vector2 position = positionProp.vector2Value;
		// If AutoXPosition == None, show position sliders
		if (autoXPositionProp.enumIndexValue == 0)
		{
			position.x = EditorGUILayout.Slider("X", scale.x, 0, 100);
		}
		position.y = EditorGUILayout.Slider("Y", scale.y, 0, 100);
		positionProp.vector2Value = position;
		
		EditorGUILayout.PropertyField(visibleProp);
		EditorGUILayout.PropertyField(contentProp);
		
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		serializedObject.ApplyModifiedProperties();
	}
}

