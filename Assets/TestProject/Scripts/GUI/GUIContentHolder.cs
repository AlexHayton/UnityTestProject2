using UnityEngine;
using System.Collections;
using System;
	
[ExecuteInEditMode()]
public class GUIContentHolder : GUIBase {
	
	public string text;
	public Texture2D image;
	
	public GUIContent GetContent()
	{
		GUIContent content = new GUIContent();
		content.text = this.text;
		content.image = this.image;
		return content;
	}
	
	public override void OnGUI()
	{			
		base.OnGUI();
		
		this.RenderGUI(delegate() 
		{
			GUI.Box(new Rect(
				this.GetLeft(),
				this.GetTop(), 
				this.GetPixelWidth(),
				this.GetPixelHeight()), 
				this.GetContent(), 
				this.GetStyle());
		});
	}
}

