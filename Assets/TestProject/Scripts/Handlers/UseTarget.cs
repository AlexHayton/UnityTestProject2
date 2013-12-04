using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class UseTarget : MonoBehaviour {

		public List<IUsable> UseComponents = new List<IUsable>();
		public Texture2D useIconTexture;
		public Texture2D useButtonTexture;
		
		public void RegisterUsable(IUsable component)
		{
			UseComponents.Add(component);
		}
		
		public bool CanUse()
    	{
        	bool canUse = false;
        	foreach (IUsable usable in UseComponents)
        	{
        		canUse = usable.GetCanBeUsed();
        		if (canUse)
        		{
        			break;
        		}
        	}
        	
        	return canUse;
    	}
    	
    	public void OnUse()
    	{
    		foreach (IUsable usable in UseComponents)
        	{
        		if (usable.GetCanBeUsed())
        		{
        			usable.OnUse();
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