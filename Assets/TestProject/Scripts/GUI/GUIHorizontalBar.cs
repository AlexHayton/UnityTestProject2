using UnityEngine;
using System.Collections;
using System;
	
public abstract class GUIHorizontalBar : GUIContentHolder {
	
	private float cachedScalar = 1.0f;
	
	public bool showText = true;
	
	public abstract float GetFullScalar();
	
	public override float GetPixelWidth()
	{
		return base.GetPixelWidth() * cachedScalar;
	}
	
	public Rect GetSourceRect()
	{
		return new Rect(
			0,
			0,
			cachedScalar,
			1);
	}
	
	public override GUIContent GetContent()
	{
		GUIContent content = base.GetContent();
		content.text = this.GetText();
		content.image = this.GetImage();
		return content;
	}
	
	public override void OnGUI()
	{			
		this.OnBaseGUI();
		
		// Size and offset the texture based on %
		cachedScalar = this.GetFullScalar();
	
		this.RenderGUI(delegate()
		{
			if (Event.current.type == EventType.Repaint)
			{
				GUI.DrawTextureWithTexCoords(new Rect(
				this.GetLeft(),
				this.GetTop(), 
				this.GetPixelWidth(),
				this.GetPixelHeight()), 
				this.GetBackground(),
				this.GetSourceRect());
			}
		});
	}    	
}

