using UnityEngine;
using System.Collections;

namespace TestProject
{
	/// <summary>
	/// Sets a random rotation when the object spawns.
	/// </summary>
	public class RandomOrientation : MonoBehaviour {
		
		public bool xAxis = false;
		public bool yAxis = true;
		public bool zAxis = false;
		
		void Start()
		{
			Quaternion randomOrientation = Random.rotation;
			Vector3 randomEuler = randomOrientation.eulerAngles;

			if (!xAxis)
			{
				randomEuler.x = transform.eulerAngles.x;
			}

			if (!yAxis)
			{
				randomEuler.y = transform.eulerAngles.y;
			}

			if (!zAxis)
			{
				randomEuler.z = transform.eulerAngles.z;
			}

			randomOrientation.eulerAngles = randomEuler;
			transform.rotation = randomOrientation;
		}
	}
}
