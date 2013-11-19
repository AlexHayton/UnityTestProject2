using UnityEngine;
using System.Collections;
	
[ExecuteInEditMode()]
public class GUIStylePrefab : MonoBehaviour, ISelfTest {
	public Texture2D normalBackground;
	public Color normalTextColor = new Color(1,1,1,1);
	public Texture2D hoverBackground;
	public Color hoverTextColor = new Color(1,1,1,1);
	public Texture2D activeBackground;
	public Color activeTextColor = new Color(1,1,1,1);
	
	public Font font;
	public int fontSize;
	public FontStyle fontStyle;
	public TextAnchor alignment;

	public ImagePosition imagePosition = ImagePosition.ImageAbove;
	
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
		
		SelfTestUtility.GreaterThanZero(ref fail, this, "scale");
		
		return fail;
	}
	
	private void UpdateLastScale()
	{
		this.lastScale.x = this.scale.x;
		this.lastScale.y = this.scale.y;
	}
	
	private GUIStyle m_ComputedGUIStyle = null;
	public GUIStyle GetStyle()
	{
		if (m_ComputedGUIStyle == null)
		{
			m_ComputedGUIStyle = new GUIStyle();
		
			m_ComputedGUIStyle.normal.background = this.normalBackground;
			m_ComputedGUIStyle.normal.textColor = this.normalTextColor;
			m_ComputedGUIStyle.hover.background = this.hoverBackground;
			m_ComputedGUIStyle.hover.textColor = this.hoverTextColor;
			m_ComputedGUIStyle.active.background = this.activeBackground;
			m_ComputedGUIStyle.active.textColor = this.activeTextColor;
			m_ComputedGUIStyle.font = this.font;
			m_ComputedGUIStyle.fontSize = this.fontSize;
			m_ComputedGUIStyle.fontStyle = this.fontStyle;
			m_ComputedGUIStyle.alignment = this.alignment;
			m_ComputedGUIStyle.imagePosition = this.imagePosition;
		}
		
		return m_ComputedGUIStyle;
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
