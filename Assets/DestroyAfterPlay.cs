using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DestroyAfterPlay : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (!this.audio.isPlaying) {
			Destroy (this.gameObject);
		}
	}
}
