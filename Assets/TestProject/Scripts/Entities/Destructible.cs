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
		// Just get the triggers that we need to trigger.
		IEnumerable<DestructibleTrigger> triggers = destructibleTriggers.Where(t => t.triggerHealthPercentage > newHealthPercentage && t.triggerHealthPercentage <= lastHealthPercentage).OrderBy(t => t.triggerHealthPercentage);
		
		foreach(DestructibleEffect effect in triggers)
		{
			PlaySoundEffects(effect);
			SpawnDamagePrefab(effect);
			SwapRenderModel(effect);
			PlayParticleEffects(effect);
		}
	}
	
	private void PlaySoundEffects(DestructibleEffect effect)
	{
		if (soundEffectToPlay)
		{
			PlaySoundAtPoint(soundEffectToPlay, transform.position);
		}
		
		if (soundEffectToLoop)
		{
			PlaySoundAtPoint(soundEffectToLoop, transform.position);
		}
	}
	
	private void SpawnDamagePrefab(DestructibleEffect effect)
	{
		if (spawnPrefab)
		{
			Instantiate(spawnPrefab, transform.position, transform.rotation);
		}
	}
	
	private void SwapRenderModel(DestructibleEffect effect)
	{
		if (swapToModel)
		{
			Instantiate(swapToModel, transform.position, transform.rotation);
		}
	}
	
	private void PlayParticleEffects(DestructibleEffect effect)
	{
		if (effectToPlay)
		{
			effectToPlay.Emit(1);
		}
		
		if (effectToLoop)
		{
			effectToLoop.enabled = true;
		}
	}
}
