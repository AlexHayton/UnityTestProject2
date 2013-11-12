using UnityEngine;
using System.Collections;
using System.Linq;

// This class parents GameObjects with their GUIs.
public class GUIHandler : MonoBehaviour
{
    public List<GUIBase> allGuis = new List<GUIBase>();
    Dictionary<Type, Set<GUIBase>> guisByType = new Dictionary<Type, List<GUIBase>>();
    
    public void RegisterGUI<T>(T gui) where T : GUIBase
    {
    	Type GuiType = typeof(T);
    	if (!guisByType.ContainsKey(typeof<T>))
    	{
    		guisByType[GuiType] = new Set<GUIBase>();
    	}
    	Set<GUIBase> guiSet = guisByType[GuiType];
    	
    	guiSet.Add(gui);
    }

    public T GetGUI<T>()
    {
    	return GetGUIs<T>().FirstOrDefault();
    }
    
    public IEnumerable<T> GetGUIs<T>()
    	Type GuiType = typeof(T);
    	IEnumerable<T> guis = null;
    	if (guisByType.ContainsKey(typeof<T>))
    	{
    		return guisByType[GuiType];
    	}
    	else
    	{
    		return new List<T>();
    	}
    }

}
