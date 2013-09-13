using UnityEngine;
using System.Collections;
	
public abstract class GUIBase : MonoBehaviour {

	public float top;
	public float left;
	public bool visible;
	private bool hovering;
	public Vector3 scale;
	public Texture normalTexture;
	public Texture hoveringTexture;
	
	// Use this for initialization
	public virtual void Start () {
	}
	
	public virtual float GetTop()
	{
		return this.top;
	}
	
	protected void SetTop(float top)
	{
		this.top = top;
	}
	
	public virtual float GetLeft()
	{
		return left;
	}
	
	protected void SetLeft(float left)
	{
		this.left = left;
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
	
	public void SetScale(Vector3 scale)
	{
		this.scale = scale;
	}
	
	public virtual Bounds GetBounds()
	{
		Vector3 center = new Vector3(this.GetLeft() + this.scale.x / 2.0f, this.GetTop() + this.scale.y / 2.0f, 0);
		Vector3 size = new Vector3(this.scale.x / 2.0f, this.scale.y / 2.0f, 1.0f);
		return new UnityEngine.Bounds(center, size);
	}
	
	protected virtual bool GetIsHovering()
	{
		if (Screen.showCursor && !Screen.lockCursor)
		{			
			if (this.GetBounds().Contains(Input.mousePosition))
			{
				return true;
			}
		}
		
		return false;
	}
	
	protected Texture GetNormalTexture()
	{
		return this.normalTexture;
	}
	
	protected Texture GetHoveringTexture()
	{
		return this.hoveringTexture;
	}
}


