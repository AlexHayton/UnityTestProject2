using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class DestroyAfterTime : MonoBehaviour {

		public float destroyAfterTime = 1;

		void Start()
		{
			FadeOutHandler fadeOut = GetComponent<FadeOutHandler>();
			if (fadeOut)
			{
				fadeOut.QueueDestroy(destroyAfterTime);
			}
			else
			{
				Destroy (gameObject, destroyAfterTime);
			}
		}
	}
}
