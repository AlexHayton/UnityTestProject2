using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TestProject
{
	public class UseTarget : MonoBehaviour, IUsable {

		public List<IUsable> UseComponents = new List<IUsable>();
		public Texture2D useIconTexture;
		public Texture2D useButtonTexture;
		
		public void RegisterUsable(IUsable component)
		{
			UseComponents.Add(component);
		}
		
		public bool CanBeUsedBy(GameObject user)
    	{
        	bool canUse = false;
        	foreach (IUsable usable in UseComponents)
        	{
				canUse = usable.CanBeUsedBy(user);
        		if (canUse)
        		{
        			break;
        		}
        	}
        	
        	return canUse;
    	}
    	
    	public void OnUse(GameObject user)
    	{
    		foreach (IUsable usable in UseComponents)
        	{
				if (usable.CanBeUsedBy(user))
        		{
        			usable.OnUse(user);
        		}
        	}
    	}
    	
    	public Texture2D GetUseIcon()
    	{
    		return this.useIconTexture;
    	}
    	
    	public Texture2D GetUseButton()
    	{
    		return this.useButtonTexture;
    	}
    }
}