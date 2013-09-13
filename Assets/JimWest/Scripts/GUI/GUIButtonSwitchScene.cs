using UnityEngine;
using System.Collections;
	
public class GUIButtonSwitchScene : GUIButton {
	
	public int scene;
	
	public override void OnLeftButtonUp()
	{
		this.SwitchScene();
	}
	
	public override void OnRightButtonUp()
	{
		this.SwitchScene();
	}
	
	public void SwitchScene()
	{
		Application.LoadLevel(scene);
	}
	
}