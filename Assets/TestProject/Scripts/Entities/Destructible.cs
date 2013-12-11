using UnityEngine;
using TestProject;
using System.Linq;
using System.Collections.Generic;

public struct DestructibleEffect
{
	public int triggerHealthPercentage;
	public AudioClip soundEffectToPlay;
	public AudioClip soundEffectToLoop;
	public GameObject spawnPrefab;
	public GameObject swapToModel;
	public ParticleSystem particleToEmit;
	public ParticleSystem particleToLoop;
}

// Use this to auto-play any animations and auto-destroy when done
[RequireComponent(typeof(HealthHandler))]
public class Destructible : MonoBehaviour {
	
	List<DestructibleEffect> destructibleTriggers = new List<DestructibleEffect>();
	float lastHealthPercentage = 100;
	
	public void HealthChanged(float newHealthPercentage)
	{
		// Just get the triggers that we need to trigger.
		IEnumerable<DestructibleEffect> triggers = destructibleTriggers.Where(t => t.triggerHealthPercentage > newHealthPercentage && t.triggerHealthPercentage <= lastHealthPercentage).OrderBy(t => t.triggerHealthPercentage);
		
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
		if (effect.soundEffectToPlay)
		{
			AudioSource.PlayClipAtPoint(effect.soundEffectToPlay, transform.position);
		}
		
		if (effect.soundEffectToLoop)
		{
			AudioSource.PlayClipAtPoint(effect.soundEffectToLoop, transform.position);
		}
	}
	
	private void SpawnDamagePrefab(DestructibleEffect effect)
	{
		if (effect.spawnPrefab)
		{
			Instantiate(effect.spawnPrefab, transform.position, transform.rotation);
		}
	}
	
	private void SwapRenderModel(DestructibleEffect effect)
	{
		if (effect.swapToModel)
		{
			//Instantiate(effect.swapToModel, transform.position, transform.rotation);
		}
	}
	
	private void PlayParticleEffects(DestructibleEffect effect)
	{
		if (effect.particleToEmit)
		{
			effect.particleToEmit.Emit(1);
		}
		
		if (effect.particleToLoop)
		{
			effect.particleToLoop.Play();
		}
	}
}
