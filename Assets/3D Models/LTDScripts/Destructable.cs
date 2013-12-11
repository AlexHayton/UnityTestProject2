using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Destructable : MonoBehaviour 
{
	
	public int Health = 100;
	public List<GameObject> FX;
	public int dmgIncrement;
	
	public GameObject TempReplace;
	private GameObject replacement;
	
	void Start () 
	{
		//Temporary//
		TempReplace = Resources.Load("Pfb_Wall_1_Generic") as GameObject;	
		dmgIncrement = -(Mathf.RoundToInt(Health/FX.Count));		
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Bullet")
		{
			foreach (GameObject effect in FX)
			{
				if(effect.GetComponent<ParticleEmitter>())
				{
					effect.particleEmitter.emit = true;
				}
				else
				{
					effect.particleSystem.Play();
				}
			}
			
			if(Health <= 0)
			{
				foreach (GameObject effect in FX)
				{
					if(effect.GetComponent<ParticleEmitter>())
					{
						effect.particleEmitter.emit = true;
					}
					else
					{
						effect.particleSystem.Play();
					}
				}
				
				//instantiates damaged tile and parents it in exact position/hierarchy of previous tile//
				replacement = Instantiate(TempReplace, gameObject.transform.position, gameObject.transform.rotation) as GameObject; //replaces with broken pipes tile//
				replacement.transform.parent = transform.parent;
				
				//destroys old tile//
				Destroy(gameObject);
			}
		}
		
		//any form of sufficient damage will cause wall to explode//

	}
}