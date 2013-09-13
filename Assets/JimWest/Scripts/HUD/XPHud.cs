using UnityEngine;
using System.Collections;

public class XPHud : BoxHud {	
	
	public const int XPBarHeight = 10;
	public const int XPBarOffset = 10;
	
	public XPHud():base()
	{
		this.SetTop(Screen.height - XPBarHeight);
		this.SetLeft(0);
	}
	
	public override Color GetColor()
	{
		return new Color(0,0.2f,1,1);
	}
	
	public override float GetWidth() {
		float baseWidth = Screen.width - (Screen.width/3);
		float newWidth = this.width;
		float xp = 0;
		XPHandler xpComponent = player.GetComponent<XPHandler>();
		if (xpComponent != null)
		{
			xp = xpComponent.GetXp();
		}
		
		if (xp > 0) {
			 newWidth = baseWidth * xp / 1000;
		} else {
			 newWidth = 0;
		}

		return newWidth; 
	}
	
	public override float GetHeight()
	{
		return 15;
	}
	
	public override string GetText (){
		float xp = 0;
		XPHandler xpComponent = player.GetComponent<XPHandler>();
		if (xpComponent != null)
		{
			xp = xpComponent.GetXp();
			xp = Mathf.Floor (xp);
		}
		return xp.ToString();
	}

}
