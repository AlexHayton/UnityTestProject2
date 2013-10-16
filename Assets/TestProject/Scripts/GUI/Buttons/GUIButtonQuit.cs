using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

[ExecuteInEditMode()]
public class GUIButtonQuit : GUIButton {
	
	public override void OnLeftButtonUp()
	{
		this.Quit();
	}
	
	public void Quit()
	{
		try
		{
			EditorApplication.ExecuteMenuItem("Edit/Play");
		}
		catch (Exception)
		{
			Application.Quit();
		}
	}
	
}