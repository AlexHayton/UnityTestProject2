using System;
using UnityEngine;
using System.Collections.Generic;

namespace TestProject
{
	public class MuzzleFlashBase : MonoBehaviour
	{
		IList<Renderer> childRenderers = null;
		IList<Light> childLights = null;
		public List<Material> randomisedMaterials = new List<Material> ();
		private float defaultScale;
		public float VaryScaleByPercent = 0.0f;
		float hideTime = Time.time;
		bool visible = false;

		void Start()
		{
			childRenderers = gameObject.GetComponentsInChildren<Renderer>();
			childLights = gameObject.GetComponentsInChildren<Light>();
			defaultScale = transform.localScale.x;
			this.Hide();
		}

		void Update()
		{
			if (this.visible && Time.time > this.hideTime)
			{
				this.Hide();
			}
		}

		public void Hide()
		{
			visible = false;

			foreach(Renderer renderer in childRenderers)
			{
				renderer.enabled = false;
			}

			foreach(Light light in childLights)
			{
				light.enabled = false;
			}
		}

		public void Fire(float duration)
		{
			hideTime = Time.time + duration;
			visible = true;

			// Randomise the local scale
			float varyScale = defaultScale * VaryScaleByPercent / 100.0f;
			Vector3 localScale = transform.localScale;
			localScale.x = UnityEngine.Random.Range (defaultScale - varyScale, defaultScale + varyScale);
			transform.localScale = localScale;

			// Randomise the texture if we have some textures to choose from
			Material randomMaterial = null;
			if (randomisedMaterials.Count > 0) 
			{
				int randomMaterialIndex = UnityEngine.Random.Range(0, randomisedMaterials.Count - 1);
				randomMaterial = this.randomisedMaterials[randomMaterialIndex];
			}

			// Randomise the rotation
			transform.Rotate(new Vector3(UnityEngine.Random.Range (0, 360), 0, 0));

			foreach(Renderer renderer in childRenderers)
			{
				renderer.enabled = true;

				if (randomMaterial)
				{
					renderer.material = randomMaterial;
				}
			}

			foreach(Light light in childLights)
			{
				light.enabled = true;
			}
		}
	}
}

