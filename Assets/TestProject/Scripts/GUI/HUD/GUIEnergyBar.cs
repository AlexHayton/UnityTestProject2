using UnityEngine;
using System.Collections;
using System;
	
public abstract class GUIHealthBar : GUIHorizontalBar {

	private GameObject player;

	public override void Start()
	{
		base.Start();
		player = PlayerUtility.GetPlayer ();
	}
	
	public override float GetPercentageFull()
	{
		EnergyHandler handler = player.GetComponent<EnergyHandler>();
		float maxEnergy = handler.GetMaxEnergy();
		float energy = handler.GetEnergy();
		
		if (energy > 0) {
			 return maxEnergy / energy;
		} else {
			 return 0;
		}
	}	
	
	public override string GetText()
	{
		EnergyHandler handler = player.GetComponent<EnergyHandler>();
		float maxEnergy = handler.GetMaxEnergy();
		float energy = handler.GetEnergy();
		
		maxEnergy = Mathf.Ceil (maxEnergy);
		energy = Mathf.Ceil (energy);
		return ( energy + "/" + maxEnergy);
	}
}

