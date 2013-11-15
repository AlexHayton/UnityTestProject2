using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

// This class parents GameObjects with their GUIs.
public class GUIHandler : MonoBehaviour
{
    public List<GUIBase> allGuis = new List<GUIBase>();
    Dictionary<Type, HashSet<GUIBase>> guisByType = new Dictionary<Type, HashSet<GUIBase>>();
    
    public void RegisterGUI<T>(T gui) where T : GUIBase
    {
    	Type GuiType = typeof(T);
    	this.RegisterGUI(GuiType, gui);
    }
    
    public void RegisterGUI(Type GuiType, GUIBase gui)
    {
    	if (!guisByType.ContainsKey(GuiType))
    	{
    		guisByType[GuiType] = new HashSet<GUIBase>();
    	}
    	HashSet<GUIBase> guiSet = guisByType[GuiType];
    	
    	guiSet.Add(gui);
    }

    public T GetGUI<T>()
    {
    	return GetGUIs<T>().FirstOrDefault();
    }
    
    public IList<T> GetGUIs<T>()
	{
    	Type GuiType = typeof(T);
    	if (guisByType.ContainsKey(typeof(T)))
    	{
			IEnumerable<T> guis = guisByType[GuiType].Cast<T>();
    		return guis.ToList();
    	}
    	else
    	{
    		return new List<T>();
    	}
    }

}
