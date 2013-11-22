using System;
using UnityEngine;
using System.Collections.Generic;

namespace TestProject
{
	public class MuzzleFlashBase : MonoBehaviour
	{
		IList<Renderer> childRenderers = null;
		IList<Light> childLights = null;
		float hideTime = Time.time;
		bool visible = false;

		void Start()
		{
			childRenderers = gameObject.GetComponentsInChildren<Renderer>();
			childLights = gameObject.GetComponentsInChildren<Light>();
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

