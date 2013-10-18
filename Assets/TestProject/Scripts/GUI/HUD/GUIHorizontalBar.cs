using UnityEngine;
using System.Collections;
using System;
	
public abstract class GUIHorizontalBar : GUIContentHolder {
	
	public abstract float GetPercentageFull();
	private float cachedPercentage = 100.0f;
	
	public override float GetPixelWidth()
	{
		return base.GetPixelWidth() * cachedPercentage;
	}
	
	public override GUIContent GetContent()
	{
		GUIContent content = base.GetContent();
		content.image = content.image.Crop(0, cachedPercentage);
	}
	
	public override void OnGUI()
	{			
		// Size and offset the texture based on %
		cachedPercentage = this.PercentageFull();
	
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

