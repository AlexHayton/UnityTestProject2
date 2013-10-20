using UnityEngine;
using System.Collections;
using System;
	
public abstract class GUIHorizontalBar : GUIContentHolder {
	
	private float cachedPercentage = 100.0f;
	
	public abstract float GetPercentageFull();
	
	public override float GetPixelWidth()
	{
		return base.GetPixelWidth() * cachedPercentage;
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

