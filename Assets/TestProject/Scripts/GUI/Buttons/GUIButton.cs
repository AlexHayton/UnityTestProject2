using UnityEngine;
using System.Collections;
using System;
	
public abstract class GUIButton : GUIContentHolder {
	
	public override void OnGUI()
	{			
		base.OnGUI();
		
		if (Application.isPlaying)
		{
			// Handle clicks
			if (this.GetIsMouseOver())
			{
				if (Event.current != null && Event.current.isMouse)
				{
					switch (Event.current.button)
					{
					case 0: 
						this.HandleClickButton(Event.current, this.OnLeftButtonDown, this.OnLeftButtonUp);
						break;
						
					case 1:
						this.HandleClickButton(Event.current, this.OnRightButtonDown, this.OnRightButtonUp);;
						break;
					}
				}
			}
		}
	}    	
	
	protected void HandleClickButton(Event currentEvent, Action downFunc, Action upFunc)
	{
		switch (currentEvent.type)
		{
		case EventType.MouseDown:
			downFunc();
			break;
			
		case EventType.MouseUp:
			upFunc();
			break;
		}
	}
	
	public virtual void OnLeftButtonDown()
	{
	}
	
	public virtual void OnLeftButtonUp()
	{
	}
	
	public virtual void OnRightButtonDown()
	{
	}
	
	public virtual void OnRightButtonUp()
	{
	}
}

