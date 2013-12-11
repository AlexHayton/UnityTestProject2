using UnityEngine;

namespace TestProject
{
	/// <summary>
	/// Use this interface for all 
	/// </summary>
	public interface IUsable
	{
	
		bool CanBeUsedBy(GameObject user);
		void OnUse(GameObject user);
	
	}

}