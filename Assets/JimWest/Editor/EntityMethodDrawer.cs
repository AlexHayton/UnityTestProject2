using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


[CustomPropertyDrawer (typeof (EntityMethodPair))]
public class EntityMethodDrawer : PropertyDrawer {
	
	private float extraHeight = 20.0f; 

    public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {		
	
        SerializedProperty target = prop.FindPropertyRelative ("target");
        SerializedProperty method = prop.FindPropertyRelative ("method");
		SerializedProperty index = 	prop.FindPropertyRelative ("index");
		
		EditorGUI.BeginProperty (pos, label, prop) ;
		
		//EditorGUI.BeginChangeCheck();
		// make it a bit more right
		EditorGUI.indentLevel ++;
		
		// original object entity field
		EditorGUI.PropertyField (pos, target, true);		
		//obj = (Entity)EditorGUI.ObjectField (new Rect(pos.x, pos.y, pos.width, pos.height - extraHeight), "Target", obj, typeof(Entity));
				
		// 2nd line
		pos.y += extraHeight-3;
		pos.height = extraHeight;	
		
		// get the methods
		string[] methods = {Entity.noFunctionString};			
		Entity tempEnt = (Entity)target.objectReferenceValue;

		if (tempEnt != null){
			methods = EntityUtility.GetMethodList (tempEnt);
		}		
		
		// to save the choosen index so it will not be always 0
		//int tempIndex = Mathf.Min ((int)index.intValue, methods.Length) ;				

		// drop down list for the methods
		index.intValue = EditorGUI.Popup(pos,"Method", index.intValue, methods);	
		method.stringValue = methods[index.intValue];			
	
		EditorGUI.EndProperty ();
				
    }
	
	
	
	// make it a bit higher so it will look good
	public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) {
		

		return base.GetPropertyHeight(prop, label) + extraHeight;
		
	}
	
}
