using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour 
{
	
	public int Dmg = 0;
	public int DmgThresh = 10;
	public GameObject[] fx;
	public GameObject tempReplace;
	private GameObject replacement;
	
	void Start () 
	{
		fx = new GameObject[8];
		tempReplace = Resources.Load("Pfb_Wall_1_Generic") as GameObject;
		
		//assign children fx//
		fx[0] = transform.Find("FX_1").gameObject;
		fx[1] = transform.Find ("FX_2").gameObject;
		
		//disable effects on lvl start//
		fx[0].particleSystem.enableEmission = false;
		fx[1].particleSystem.enableEmission = false;
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Bullet")
		{
			Dmg = Dmg + 1;
			
			//Emits smoke or fuel effect//
			if(fx[0] != null)
			{
				fx[0].particleSystem.enableEmission = true;
			}
			if(fx[1] != null)
			{
				fx[1].particleSystem.enableEmission = true;
			}
		}
		
		//any form of sufficient damage will cause wall to explode//
		if(Dmg >= DmgThresh)
		{
			Dmg = 0;
			
			fx[0].particleSystem.enableEmission = false;
			fx[1].particleSystem.enableEmission = false;
//			fx[1].particleSystem.enableEmission = true; //emit explosion fx//
			
			//instantiates damaged tile and parents it in exact position/hierarchy of previous tile//
			replacement = Instantiate(tempReplace, gameObject.transform.position, gameObject.transform.rotation) as GameObject; //replaces with broken pipes tile//
			replacement.transform.parent = transform.parent;
			
			//destroys old tile//
			Destroy(gameObject);
		}
	}
}
