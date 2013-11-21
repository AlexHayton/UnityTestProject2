using UnityEngine;

// You can use these functions to instantiate and manage effects.
public static class EffectUtility
{
	// This version of the function gets angry if it is passed invalid parameters
	public static GameObject InstantiateEffectPrefab(GameObject effectPrefab, Vector3 position, Quaternion rotation)
	{
		if (effectPrefab == null)
			throw new ArgumentException("Tried to instantiate an effect prefab but it was null");
		
		return TryInstantiateEffectPrefab(effectPrefab, position, rotation);
	}
	
	// Create an effect at a location. Use this when you don't want the object to be tied to a particular GameObject.
	public static GameObject TryInstantiateEffectPrefab(GameObject effectPrefab, Vector3 position, Quaternion rotation)
	{
		if (effectPrefab != null)
		{
			GameObject gameObject = Instantiate(effectPrefab, transform, rotation);
			PlayEffects(gameObject);
			DestroyWhenFinished(gameObject);
			return gameObject;
		}
		else
		{
			return null;
		}
	}
	
	// This version of the function gets angry if passed invalid parameters
	public static GameObject InstantiateEffectPrefab(GameObject effectPrefab, GameObject parent)
	{
		if (effectPrefab == null)
			throw new ArgumentException("Tried to instantiate an effect prefab but it was null");
			
		if (parent == null)
			throw new ArgumentException("Tried to instantiate an effect prefab but its parent was null");
			
		return TryInstantiateEffectPrefab(effectPrefab, parent);
	}
		
	// Create an effect at relative to an existing object
	// If the parent object is destroyed, this one will be too!
	// This version of the function is tolerant of invalid parameters
	public static GameObject TryInstantiateEffectPrefab(GameObject effectPrefab, GameObject parent)
	{
		if (effectPrefab != null && parent != null)
		{
			GameObject gameObject = Instantiate(effectPrefab);
			gameObject.transform.parent = parent;
			PlayEffects(gameObject);
			DestroyWhenFinished(gameObject);
			return gameObject;
		}
		else
		{
			return null;
		}
	}
	
	public static void PlayEffects(GameObject gameObject)
	{
		if (gameObject.animation)
		{
			gameObject.animation.Play();
		}
			
		if (gameObject.soundSource)
		{
			gameObject.soundSource.Play();
		}
	}

	// Destroy the gameObject when effects are finished
	public static void DestroyWhenFinished(GameObject gameObject) {
	    float lifetime = 0.1f;
	    if (gameObject.animation)
	    {
	    	lifetime = Math.max(lifetime, gameObject.animation.length);
	    }
	    
	    if (gameObject.soundSource)
	    {
	    	lifetime = Math.max(lifetime, gameObject.soundSource.length);
	    }
	    
	    Destroy(gameObject, lifetime);
	}

}
