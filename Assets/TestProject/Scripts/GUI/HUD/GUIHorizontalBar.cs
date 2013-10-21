using UnityEngine;
using System.Collections;
using System;
	
public abstract class GUIHorizontalBar : GUIContentHolder {
	
	private float cachedPercentage = 100.0f;
	
	public abstract float GetPercentageFull();
	
	public override float GetPixelWidth()
	{
		return this.GetPixelWidth() * cachedPercentage;
	}
	
	public override int GetTextureWidth()
	{
		return (int)Math.Round (this.GetImage().width * cachedPercentage);
	}
	
	public Rect GetSourceRect()
	{
		return new Rect(
			0,
			0,
			this.GetTextureWidth(),
			this.GetTextureHeight());
	}
	
	public override GUIContent GetContent()
	{
		GUIContent content = base.GetContent();
		content.text = this.GetText();
		content.image = this.GetImage();
		//content.image = content.image.Crop(0, cachedPercentage);
		return content;
	}
	
	public override void OnGUI()
	{			
		// Size and offset the texture based on %
		cachedPercentage = this.GetPercentageFull();
	
		this.RenderGUI(delegate()
		{
			if (Event.current.type == EventType.Repaint)
			{
				GUI.DrawTextureWithTexCoords(new Rect(
				this.GetLeft(),
				this.GetTop(), 
				this.GetPixelWidth(),
				this.GetPixelHeight()), 
				this.GetImage(),
				this.GetSourceRect());
			}
		});
	}    	
}

