using UnityEngine;
using System.Collections;
	
[ExecuteInEditMode()]
public abstract class GUIMenu : MonoBehaviour {
	
	public bool visible = false;
	private bool lastVisible = false;
	
	/// <summary>
	/// When the GUI flag changes, set all children to visible/otherwise
	/// </summary>
	public void OnGUI()
	{
		if (visible != lastVisible)
		{
			GUIBase[] allGui = this.GetComponentsInChildren<GUIBase>();
			foreach (GUIBase guiElement in allGui)
			{
				guiElement.menuVisible = this.visible;
			}
		}
	}
	
}

