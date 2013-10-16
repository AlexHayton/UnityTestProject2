using System;
using UnityEditor;
using UnityEngine;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CanEditMultipleObjects]
[CustomEditor(typeof(GUIStylePrefab))]
public class GUIStylePrefabEditor : Editor
{	
	SerializedProperty scaleProp;
	SerializedProperty lockScaleProp;
	SerializedProperty normalBackgroundProp;
	SerializedProperty normalTextColorProp;
	SerializedProperty hoverBackgroundProp;
	SerializedProperty hoverTextColorProp;
	SerializedProperty activeBackgroundProp;
	SerializedProperty activeTextColorProp;
	SerializedProperty fontProp;
	SerializedProperty fontSizeProp;
	SerializedProperty fontStyleProp;
	SerializedProperty alignmentProp;
	
    public void OnEnable () {
    	scaleProp = serializedObject.FindProperty("scale");
		lockScaleProp = serializedObject.FindProperty("lockScaleRatio");
		normalBackgroundProp = serializedObject.FindProperty("normalBackground");
		normalTextColorProp = serializedObject.FindProperty("normalTextColor");
		hoverBackgroundProp = serializedObject.FindProperty("hoverBackground");
		hoverTextColorProp = serializedObject.FindProperty("hoverTextColor");
		activeBackgroundProp = serializedObject.FindProperty("activeBackground");
		activeTextColorProp = serializedObject.FindProperty("activeTextColor");
		fontProp = serializedObject.FindProperty("font");
		fontSizeProp = serializedObject.FindProperty("fontSize");
		fontStyleProp = serializedObject.FindProperty("fontStyle");
		alignmentProp = serializedObject.FindProperty("alignment");
	}
	
	public override void OnInspectorGUI() {
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update();
		
		// Show the custom GUI controls
		normalBackgroundProp.objectReferenceValue = EditorGUILayout.ObjectField("Normal Background", normalBackgroundProp.objectReferenceValue, typeof(Texture2D), true);
		normalTextColorProp.colorValue = EditorGUILayout.ColorField("Normal Text Color", normalTextColorProp.colorValue);
		hoverBackgroundProp.objectReferenceValue = EditorGUILayout.ObjectField("Hover Background", hoverBackgroundProp.objectReferenceValue, typeof(Texture2D), true);
		hoverTextColorProp.colorValue = EditorGUILayout.ColorField("Hover Text Color", hoverTextColorProp.colorValue);
		activeBackgroundProp.objectReferenceValue = EditorGUILayout.ObjectField("Clicked Background", activeBackgroundProp.objectReferenceValue, typeof(Texture2D), true);
		activeTextColorProp.colorValue = EditorGUILayout.ColorField("Clicked Text Color", activeTextColorProp.colorValue);
		fontProp.objectReferenceValue = EditorGUILayout.ObjectField("Font", fontProp.objectReferenceValue, typeof(Font), true);
		fontSizeProp.intValue = EditorGUILayout.IntField("Font Size", fontSizeProp.intValue); 
		EditorGUILayout.PropertyField(fontStyleProp);
		EditorGUILayout.PropertyField(alignmentProp);
		
		Vector2 scale = scaleProp.vector2Value;
		scale.x = EditorGUILayout.Slider("X", scale.x, 0, 100);
		scale.y = EditorGUILayout.Slider("Y", scale.y, 0, 100);
		scaleProp.vector2Value = scale;
		
		lockScaleProp.boolValue = EditorGUILayout.Toggle("Lock Scale", lockScaleProp.boolValue);
		
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		serializedObject.ApplyModifiedProperties();
	}
}

