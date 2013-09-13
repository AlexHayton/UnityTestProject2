using UnityEngine;
using System.Collections;

public class GameUtility {
		
	private static float tempTimeScale;

	public static void PauseGame() {			
		tempTimeScale = Time.timeScale;
		Time.timeScale = 0.0f; 			
	}
	
	public static void UnPauseGame() {
		if (tempTimeScale != 0) {
			Time.timeScale = tempTimeScale;
		} else {
			Time.timeScale = 1.0f;
		}
	}
	
}