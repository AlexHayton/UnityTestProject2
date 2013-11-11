using System;
using UnityEngine;

[System.Serializable]
public class ActivationMask
{	
	
	public int value = 0;
	public ActivationType mask;
	
	// bit enum (0 is nothing, -1 is all, bitmask interpretation from unity)
	public enum ActivationType {
		AfterSpawn = 0x1,
		SeeEnemy = 0x2,
		HearEnemy = 0x4,
		GettingAttacked = 0x8,		
	}
	
	// bitwise check if flag is set
	public bool IsSet(ActivationType flag) {
        return (((int)this.mask & 1 << (int)flag) > 0);
    }
  

}

