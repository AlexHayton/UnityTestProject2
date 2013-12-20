using UnityEngine;
using System.Collections;
using TestProject;

[RequireComponent(typeof(AudioSource))]
public class DestroyAfterPlay : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (!this.audio.isPlaying) {
			EntityUtility.DestroyGameObject(gameObject);
		}
	}
}
