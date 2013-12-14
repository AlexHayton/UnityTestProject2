using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class DestroyAfterTime : MonoBehaviour {

		public float destroyAfterTime = 1;

		void Start()
		{
			Destroy (gameObject, destroyAfterTime);
		}
	}
}
