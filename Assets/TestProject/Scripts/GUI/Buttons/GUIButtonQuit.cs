using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode()]
public class GUIButtonQuit : GUIButton {
	
	public override void OnLeftButtonUp()
	{
		this.Quit();
	}
	
	public void Quit()
	{   
#if !UNITY_EDITOR
			Application.Quit();
#else
		try
		{
			EditorApplication.ExecuteMenuItem("Edit/Play");
		}
		catch (Exception)
		{
			Application.Quit();
		}
#endif
	}
	
}