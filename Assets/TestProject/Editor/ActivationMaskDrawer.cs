using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof (ActivationMask))]
public class ActivationMaskDrawer : PropertyDrawer {

	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {	
			
		SerializedProperty maskValue = prop.FindPropertyRelative ("value");
		SerializedProperty maskEnum = prop.FindPropertyRelative ("mask");
		
		EditorGUI.BeginProperty (pos, label, prop) ;
		pos = EditorGUI.PrefixLabel (pos, GUIUtility.GetControlID (FocusType.Passive), label);
		EditorGUI.indentLevel --;
		maskEnum.intValue = (int)(ActivationMask.ActivationType)EditorGUI.EnumMaskField(pos, (ActivationMask.ActivationType)maskEnum.intValue);			
		
		EditorGUI.EndProperty ();
				
    }
	
}