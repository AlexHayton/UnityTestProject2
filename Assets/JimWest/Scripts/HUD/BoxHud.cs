using UnityEngine;
using System.Collections;

public abstract class BoxHud : Hud
{
	internal Rect rect;
	internal float width = Screen.width / 3;
	internal Texture2D texture;
	internal Color color;
	internal GUIStyle style;
	
	public override void Start () {		
		base.Start();	
		texture = new Texture2D(128, 128);
	}
	
	public virtual void OnGUI() {
		rect.width = GetWidth ();
		style = GUI.skin.box;
		GUI.backgroundColor = this.GetColor();
		style.normal.background = texture;
		rect = new Rect(left,top,GetWidth(), GetHeight());
		GUI.Box (rect , GetText(), style);
	}
		
	public virtual float GetWidth() {
		return width; 
	}
	
	public virtual float GetHeight() {
		return 20;
	}
	
	public virtual string GetText() {
		return ""; 
	}
	
	public abstract Color GetColor();

}


