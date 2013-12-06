using UnityEngine;
using TestProject;

public struct DestructibleEffect
{
	int triggerHealthPercentage = 99;
	SoundEffect soundEffectToPlay = null;
	SoundEffect soundEffectToLoop = null;
	GameObject spawnPrefab = null;
	GameObject swapToModel = null;
	ParticleEffect effectToPlay = null;
	ParticleEffect effectToLoop = null;
}

// Use this to auto-play any animations and auto-destroy when done
[RequireComponent(typeof(HealthHandler))]
public class Destructible : MonoBehaviour {
	
	List<DestructibleEffect> destructibleTriggers = new List<DestructibleEffect>();
	float lastHealthPercentage = 100;
	
	public void HealthChanged(float newHealthPercent)
	{
		IEnumerable<DestructibleTrigger> triggers = destructibleTriggers.Where(t => t.triggerHealthPercentage > newHealthPercentage && t.triggerHealthPercentage <= lastHealthPercentage).OrderBy(t => t.triggerHealthPercentage);
		
		foreach(DestructibleEffect effect in triggers)
		{
		}
	}
}
