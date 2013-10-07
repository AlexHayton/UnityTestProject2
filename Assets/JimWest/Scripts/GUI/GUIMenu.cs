using UnityEngine;
using System.Collections;
	
[ExecuteInEditMode()]
public class GUIMenu : MonoBehaviour {
	
	public bool visible = false;
	private bool lastVisible = false;
	
	public void Start()
	{
		this.lastVisible = this.visible;
		this.PopulateMenuVisible();
	}
	
	/// <summary>
	/// When the GUI flag changes, set all children to visible/otherwise
	/// </summary>
	public void OnGUI()
	{
		if (visible != lastVisible)
		{
			this.lastVisible = this.visible;
			this.PopulateMenuVisible();
		}
	}
	
	private void PopulateMenuVisible()
	{
		GUIBase[] allGui = this.GetComponentsInChildren<GUIBase>();
		foreach (GUIBase guiElement in allGui)
		{
			guiElement.MenuVisible = this.visible;
		}
	}
	
}

