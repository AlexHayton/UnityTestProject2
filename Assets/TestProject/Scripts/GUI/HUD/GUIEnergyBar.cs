using UnityEngine;
using System.Collections;
using System;
using TestProject;
	
[ExecuteInEditMode()]
public class GUIEnergyBar : GUIHorizontalBar {

	EnergyHandler handler = null;

	public override void Start()
	{
		base.Start();
		owner = PlayerUtility.GetLocalPlayer ();
		handler = owner.GetComponent<EnergyHandler>();
	}
	
	public override float GetFullScalar()
	{
		if (Application.isPlaying)
		{
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
		float maxEnergy = handler.GetMaxEnergy();
		float energy = handler.GetEnergy();
		
		maxEnergy = Mathf.Ceil (maxEnergy);
		energy = Mathf.Ceil (energy);
		return (energy + "/" + maxEnergy);
	}
}

