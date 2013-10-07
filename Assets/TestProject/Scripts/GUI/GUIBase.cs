using UnityEngine;
using System.Collections;
	
[ExecuteInEditMode()]
public abstract class GUIBase : MonoBehaviour {
	
	// Position and scale are in screen percent
	public Vector2 position;
	public bool visible = true;
	public GUIStylePrefab guiStylePrefab;
	
	public virtual void Start()
	{
		this.SelfTest();
	}
	
	public bool SelfTest()
	{
		bool fail = false;
		
		SelfTestUtility.NotNull(ref fail, "guiStylePrefab", guiStylePrefab);
		
		return fail;
	}
	
	private bool menuVisible = true;
	public bool MenuVisible 
	{
		get
		{
			return menuVisible;
		}
		internal set
		{
			menuVisible = value;
		}
	}
		
	public virtual float GetTop()
	{
		return (float)Screen.height * (this.position.y / 100.0f);
	}
	
	// In percentage of screen
	protected void SetTop(float top)
	{
		this.position.y = top;
	}
	
	public virtual float GetLeft()
	{
		return (float)Screen.width * (this.position.x / 100.0f);
	}
	
	protected void SetLeft(float left)
	{
		this.position.x = left;
	}
	
	protected bool GetIsVisible()
	{
		return this.menuVisible && this.visible;
	}
	
	protected void SetIsVisible(bool visible)
	{
		this.visible = visible;
	}
	
	public Vector3 GetScale()
	{
		return this.guiStylePrefab.scale;
	}
	
	// In percentage of screen!
	public void SetScale(Vector3 scale)
	{
		this.guiStylePrefab.scale = scale;
	}
	
	public GUIStyle GetStyle()
	{
		return this.guiStylePrefab.style;
	}
	
	public float GetPixelWidth()
	{
		return Screen.width * this.guiStylePrefab.scale.x / 100.0f;
	}
	
	public float GetPixelHeight()
	{
		return Screen.height * this.guiStylePrefab.scale.y / 100.0f;
	}
	
	public virtual Bounds GetBounds()
	{
		Vector3 center = new Vector3(this.GetLeft() + this.GetPixelWidth() / 2.0f, this.GetTop() + this.GetPixelHeight() / 2.0f, 0);
		Vector3 size = new Vector3(this.GetPixelWidth(), this.GetPixelHeight(), 2.0f);
		return new UnityEngine.Bounds(center, size);
	}
	
	protected virtual bool GetIsMouseOver()
	{
		if (Screen.showCursor && !Screen.lockCursor)
		{			
			if (this.GetBounds().Contains(Event.current.mousePosition))
			{
				return true;
			}
		}
		
		return false;
	}
}


