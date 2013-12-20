using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class FadeOutHandler : MonoBehaviour {

		bool fadingOut = false;
		private float fadeOutEndTime = 0;
		private float fadeOutBeginTime = 0;
		private float originalAlpha = 1;
		public float fadeOutTime = 1;

		public void QueueDestroy(float destroyTime)
		{
			fadingOut = true;
			fadeOutBeginTime = fadeOutEndTime - fadeOutTime;
			fadeOutEndTime = Time.time + destroyTime + fadeOutTime;
			originalAlpha = this.renderer.material.color.a;
			Destroy (gameObject, destroyTime + fadeOutTime);
		}
		
		void Update()
		{
			if (fadingOut && Time.time > fadeOutEndTime - fadeOutTime)
			{
				float fadeFactor = (fadeOutEndTime - Time.time) / fadeOutTime;
				Color oldColor = renderer.material.color;
				Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, originalAlpha * fadeFactor);
				renderer.material.color = newColor;
			}
		}
	}
}
