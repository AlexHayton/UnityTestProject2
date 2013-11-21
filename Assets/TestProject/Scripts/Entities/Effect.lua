using UnityEngine;
using TestProject;

// Use this to auto-play any animations and auto-destroy when done
public class AutoEffect : MonoBehaviour {
	
	void Start()
	{
		EffectUtility.PlayEffects(this);
		EffectUtility.DestroyWhenFinished(this);
	}
}
