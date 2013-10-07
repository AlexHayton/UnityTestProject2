using UnityEngine;
using System.Collections;
using System;
	
[ExecuteInEditMode()]
public class GUIContentHolder : GUIBase {
	
	public GUIContent content;
	
	public GUIContent GetContent()
	{
		return this.content;
	}
	
	public void OnGUI()
	{			
		if (this.GetIsVisible())
		{
			GUI.Box(new Rect(
				this.GetLeft(),
				this.GetTop(), 
				this.GetPixelWidth(),
				this.GetPixelHeight()), 
				this.GetContent(), 
				this.GetStyle());
		}
	}    	
}

