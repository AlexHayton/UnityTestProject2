using UnityEngine;
using System.Collections;
using System;
	
[ExecuteInEditMode()]
public abstract class GUIBase : MonoBehaviour {
	
	// Position and scale are in screen percent
	public Vector2 position;
	public bool visible = true;
	public GameObject guiStyleObject;
	public GUIStylePrefab guiStylePrefab;
	public int depth = 100;
	public Color color = new Color(1, 1, 1, 1);
	public AutoXPosition autoXPosition;
	
	public virtual void Start()
	{
		// Register ourselves
		GUIHandler handler = PlayerUtility.GetLocalPlayer().GetComponent<GUIHandler>();
		if (handler)
		{
			handler.RegisterGUI(this);
		}
	}
	
	public virtual bool SelfTest()
	{
		bool fail = false;
		
		SelfTestUtility.NotNull(ref fail, this, "guiStyleObject");
		//SelfTestUtility.HasComponent<GUIStylePrefab>(ref fail, this.guiStyleObject);
		
		return fail;
	}
	
	protected void OnBaseGUI()
	{
		// Try and allocate the style prefab on first GUI call.
		if (this.guiStylePrefab == null)
		{
			this.SelfTest();
			this.guiStylePrefab = this.guiStyleObject.GetComponent<GUIStylePrefab>();
		}
		
		// Set the auto X position.
		switch (autoXPosition)
		{
			case AutoXPosition.Centre:
				this.position.x = 50.0f - this.GetScale().x/2.0f;
				break;
			case AutoXPosition.Left:
				this.position.x = 0;
				break;
			case AutoXPosition.Right:
				this.position.x = 100.0f- this.GetScale().x;
				break;
			case AutoXPosition.None:
			default:
				break;
		}
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
	
	protected virtual bool GetIsVisible()
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
		return this.guiStylePrefab.GetStyle();
	}
	
	public float GetBasePixelWidth()
	{
		return Screen.width * this.guiStylePrefab.scale.x / 100.0f;
	}
	
	public virtual float GetPixelWidth()
	{
		return this.GetBasePixelWidth();
	}
	
	public float GetBasePixelHeight()
	{
		return Screen.height * this.guiStylePrefab.scale.y / 100.0f;
	}
	
	public virtual float GetPixelHeight()
	{
		return this.GetBasePixelHeight();
	}
	
	public virtual Texture2D GetBackground()
	{
		return this.GetStyle().normal.background;
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
	
	public virtual int GetDepth()
	{
		return this.depth;
	}
	
	public void SetDepth(int depth)
	{
		this.depth = depth;
	}
	
	public virtual Color GetColor()
	{
		return this.color;
	}
	
	public void SetColor(Color color)
	{
		this.color = color;
	}
	
	/// <summary>
	/// Sets the depth layer in Unity and checks visibility.
	/// Pass this your render function.
 	/// GUI.depth is modified before any other calls to get the depth right.
	/// </summary>
	protected virtual void RenderGUI(Action f)
	{
		// Only render if we're visible and the current event type is Repaint
		if (this.guiStylePrefab == null)
		{
			Debug.Log (gameObject.name + ": guiStylePrefab is null!");
		}
		else if (this.GetIsVisible() /*&& Event.current.type == EventType.Repaint*/)
		{
			GUI.color = this.color;
			GUI.depth = this.depth;
			f();
		}
	}
}


