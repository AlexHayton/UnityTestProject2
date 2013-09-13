using UnityEngine;
using System.Collections;
	
public abstract class GUIBase : MonoBehaviour {
	
	// Position and scale are in screen percent
	public Vector2 position;
	public Vector2 scale;
	public bool visible;
	public GUIContent content;
	public GUIStyle style;
	
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
		return this.visible;
	}
	
	protected void SetIsVisible(bool visible)
	{
		this.visible = visible;
	}
	
	public Vector3 GetScale()
	{
		return this.scale;
	}
	
	// In percentage of screen!
	public void SetScale(Vector3 scale)
	{
		this.scale = scale;
	}
	
	public GUIContent GetContent()
	{
		return this.content;
	}
	
	public GUIStyle GetStyle()
	{
		return this.style;
	}
	
	public float GetPixelWidth()
	{
		return Screen.width * this.scale.x / 100.0f;
	}
	
	public float GetPixelHeight()
	{
		return Screen.height * this.scale.y / 100.0f;
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
				Debug.Log ("MouseOver " + this.content.text);
				return true;
			}
		}
		
		return false;
	}
}


