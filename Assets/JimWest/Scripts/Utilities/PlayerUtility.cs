using UnityEngine;
using System.Collections;

public class PlayerUtility {
	
	public static GameObject GetPlayer() {
		return GameObject.FindWithTag("Player");
	}
}