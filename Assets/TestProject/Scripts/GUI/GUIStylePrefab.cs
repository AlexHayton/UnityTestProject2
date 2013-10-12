using UnityEngine;
using System.Collections;
	
[ExecuteInEditMode()]
public class GUIStylePrefab : MonoBehaviour, ISelfTest {
	public GUIStyle style;
	public Vector2 scale = new Vector2(10, 10);
	public bool lockScaleRatio = true;
	private Vector2 lastScale;
	
	public GUIStylePrefab()
	{
		this.UpdateLastScale();
	}
	
	public void Start()
	{
		this.UpdateLastScale();
	}
	
	public bool SelfTest()
	{
		bool fail = false;
		
		SelfTestUtility.GreaterThanZero(ref fail, "scale.x", this.scale.x);
		SelfTestUtility.GreaterThanZero(ref fail, "scale.y", this.scale.y);
		
		return fail;
	}
	
	private void UpdateLastScale()
	{
		this.lastScale.x = this.scale.x;
		this.lastScale.y = this.scale.y;
	}
	
	public void OnGUI()
	{
		// Assume that only one value could have changed
		if (!lastScale.Equals(scale))
		{
			if (lockScaleRatio)
			{
				// Assume that only one value could be changed at a time.
				// Set the other value so that the ratio between the two is preserved.
				if ((this.lastScale.x != this.scale.x) && (this.lastScale.y != this.scale.y))
				{
					// Don't do anything in this case. Just set the values.
				}
				else if (this.lastScale.x != this.scale.x)
				{
					// Set the y scale so that the ratio is preserved.
					this.scale.y = this.scale.x * (this.lastScale.y / this.lastScale.x);
				}
				else if (this.lastScale.y != this.scale.y)
				{
					// Set the x scale so that the ratio is preserved.
					this.scale.x = this.scale.y * (this.lastScale.x / this.lastScale.y);
				}
			}
		}
					
		this.UpdateLastScale();
	}
}
