using UnityEngine;
using TestProject;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class DestructibleEffect
{
	public float triggerHealthPercentage = 99;
	public GameEffect effect;
}

// Use this to auto-play any animations and auto-destroy when done
[RequireComponent(typeof(HealthHandler))]
public class Destructible : MonoBehaviour {
	
	public List<DestructibleEffect> destructibleTriggers = new List<DestructibleEffect>();
	public float lastHealthPercentage = 100;
	public DestructibleEffect onDestroyEffect;
	private HealthHandler healthHandler = null;
	private MeshFilter meshFilter = null;
	private MeshRenderer meshRenderer = null;

	void Start()
	{
		healthHandler = GetComponent<HealthHandler>();
		meshFilter = this.GetComponent<MeshFilter>();
		meshRenderer = this.GetComponent<MeshRenderer>();
	}
	
	public void OnTakeDamage()
	{
		// Just get the triggers that we need to trigger.
		IEnumerable<DestructibleEffect> triggers = destructibleTriggers.Where(t => t.triggerHealthPercentage > healthHandler.GetHealthPercentage() && t.triggerHealthPercentage <= lastHealthPercentage).OrderByDescending(t => t.triggerHealthPercentage);
		
		foreach(DestructibleEffect eff in triggers)
		{
			Debug.Log (this.name + " passed Destructible threshold of " + eff.triggerHealthPercentage);
			PlaySoundEffects(eff.effect);
			SpawnDamagePrefab(eff.effect);
			SwapRenderModel(eff.effect);
			PlayParticleEffects(eff.effect);
		}
	}
	
	private void PlaySoundEffects(GameEffect effect)
	{
		if (effect.playSoundEffect)
		{
			AudioSource.PlayClipAtPoint(effect.playSoundEffect, transform.position);
		}
		
		if (effect.loopSoundEffect)
		{
			AudioSource.PlayClipAtPoint(effect.loopSoundEffect, transform.position);
		}
	}
	
	private void SpawnDamagePrefab(GameEffect effect)
	{
		if (effect.spawnPrefab)
		{
			Instantiate(effect.spawnPrefab, transform.position, transform.rotation);
		}
	}
	
	private void SwapRenderModel(GameEffect effect)
	{
		if (this.meshFilter != null && effect.swapToMesh != null)
		{
			this.meshFilter.mesh = effect.swapToMesh;
		}

		if (this.meshRenderer != null && effect.swapToMaterials != null && effect.swapToMaterials.Count > 0)
		{
			this.meshRenderer.materials = effect.swapToMaterials.ToArray();
		}
	}
	
	private void PlayParticleEffects(GameEffect effect)
	{
		if (effect.emitParticle)
		{
			effect.emitParticle.Emit(1);
		}
		
		if (effect.loopParticle)
		{
			effect.loopParticle.Play();
		}
	}
}
