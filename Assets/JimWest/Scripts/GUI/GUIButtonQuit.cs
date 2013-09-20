using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class GUIButtonQuit : GUIButton {
	
	public override void OnLeftButtonUp()
	{
		this.Quit();
	}
	
	public void Quit()
	{
		Application.Quit();
	}
	
}