using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Linq;

public class EntityUtility {		
			
	internal static string[] ignoreMethods = new string[] {"Start", "Update"};
	
	public static Entity GetEntity(GameObject entity){
		return (Entity)entity.GetComponent<Entity>();
	}
	
	public static Entity GetEntity(Component entity){
		return (Entity)entity.GetComponent<Entity>();
	}	
	
			// returns callable methods, for triggers etc
	public static string[] GetMethodList(Entity ent) {
		
		string [] tempMethods =				
	        ent.GetType()
	        .GetMethods(BindingFlags.Instance| BindingFlags.Public | BindingFlags.NonPublic |  BindingFlags.FlattenHierarchy)			
			.Where(m => m.GetCustomAttributes(typeof(Callable), false).Length > 0)
	        .Where(x => !ignoreMethods.Any(n => n == x.Name)) // Don't list methods in the ignoreMethods array (so we can exclude Unity specific methods, etc.)
	        .Select(x => x.Name)
	        .ToArray();
				
		if (tempMethods.Length > 0) {
			// sort the list					
			Array.Sort<string> (tempMethods);
		}
		
		return tempMethods;
	}
		

	
}
