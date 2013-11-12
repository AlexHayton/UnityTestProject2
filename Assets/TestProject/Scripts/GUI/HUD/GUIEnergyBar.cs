using UnityEngine;
using System.Collections;
using System;
	
[ExecuteInEditMode()]
public class GUIEnergyBar : GUIHorizontalBar {

	private GameObject player;

	public override void Start()
	{
		base.Start();
		player = PlayerUtility.GetLocalPlayer ();
	}
	
	public override float GetFullScalar()
	{
		if (Application.isPlaying)
		{
			EnergyHandler handler = player.GetComponent<EnergyHandler>();
			float maxEnergy = handler.GetMaxEnergy();
			float energy = handler.GetEnergy();
			
			if (energy > 0) {
				 return energy / maxEnergy;
			} else {
				 return 0;
			}
		}
		else
		{
			return 1.0f;
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

