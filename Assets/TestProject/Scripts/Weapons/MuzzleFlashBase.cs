using System;
using UnityEngine;
using System.Collections.Generic;

namespace TestProject
{
	public class MuzzleFlashBase : MonoBehaviour
	{
		IList<Renderer> childRenderers = null;
		IList<Light> childLights = null;
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

			float varyScale = defaultScale * VaryScaleByPercent / 100.0f;
			Vector3 localScale = transform.localScale;
			localScale.x = UnityEngine.Random.Range (defaultScale - varyScale, defaultScale + varyScale);
			transform.localScale = localScale;

			foreach(Renderer renderer in childRenderers)
			{
				renderer.enabled = true;
			}

			foreach(Light light in childLights)
			{
				light.enabled = true;
			}
		}
	}
}

