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
	SerializedProperty guiStyleObjectProp;
	SerializedProperty guiStylePrefabProp;
	SerializedProperty depthProp;
	SerializedProperty colorProp;
	SerializedProperty autoXPositionProp;
	SerializedProperty textProp;
	SerializedProperty imageProp;
	
    public void OnEnable () {
    	positionProp = serializedObject.FindProperty("position");
		visibleProp = serializedObject.FindProperty("visible");
		guiStyleObjectProp = serializedObject.FindProperty("guiStyleObject");
		guiStylePrefabProp = serializedObject.FindProperty("guiStylePrefab");
		depthProp = serializedObject.FindProperty("depth");
		colorProp = serializedObject.FindProperty("color");
		autoXPositionProp = serializedObject.FindProperty("autoXPosition");
		textProp = serializedObject.FindProperty("text");
		imageProp = serializedObject.FindProperty("image");
	}
	
	public override void OnInspectorGUI() {
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update();
		bool allowSceneObjects = !EditorUtility.IsPersistent (target);
		
		// Show the custom GUI controls
		Type filterType = typeof(GameObject);
		if (allowSceneObjects)
		{
			guiStylePrefabProp.objectReferenceValue = EditorGUILayout.ObjectField("GUI Style", guiStylePrefabProp.objectReferenceValue, typeof(GUIStylePrefab), allowSceneObjects);
		}
		else
		{
			EditorGUILayout.PropertyField(guiStyleObjectProp);
		}
		EditorGUILayout.PropertyField(depthProp);
		EditorGUILayout.PropertyField(colorProp);
		EditorGUILayout.PropertyField(autoXPositionProp);
		
		Vector2 position = positionProp.vector2Value;
		// If AutoXPosition == None, show position sliders
		if (autoXPositionProp.enumValueIndex == 0)
		{
			position.x = EditorGUILayout.Slider("X", position.x, -20, 120);
		}
		position.y = EditorGUILayout.Slider("Y", position.y, -20, 120);
		positionProp.vector2Value = position;
		
		EditorGUILayout.PropertyField(visibleProp);
		textProp.stringValue = EditorGUILayout.TextField("Text", textProp.stringValue);
		imageProp.objectReferenceValue = EditorGUILayout.ObjectField("Texture", imageProp.objectReferenceValue, typeof(Texture2D), allowSceneObjects);
		
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		serializedObject.ApplyModifiedProperties();
	}
}

