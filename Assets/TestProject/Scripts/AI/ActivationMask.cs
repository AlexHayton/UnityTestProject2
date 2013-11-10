using System;
using UnityEngine;

[System.Serializable]
public class ActivationMask
{	
	
	public int value = 0;
	public ActivationType mask;
	
	public enum ActivationType {
		AfterSpawn = 0x1,
		SeeEnemy = 0x2,
		HearEnemy = 0x4,
		GettingAttacked = 0x8,		
	}

}

