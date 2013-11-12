using UnityEngine;
using System.Collections;
using System;
	
[ExecuteInEditMode()]
public class GUIXPBar : GUIHorizontalBar {

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
			XPHandler handler = player.GetComponent<XPHandler>();
			return handler.GetXpTillNextLevelScalar();
		}
		else
		{
			return 1.0f;
		}
	}	
	
	public override string GetText()
	{
		XPHandler handler = player.GetComponent<XPHandler>();
		float nextLevel = handler.GetXpForNextLevel();
		float currentXp = handler.GetXp();
		
		nextLevel = Mathf.Ceil (nextLevel);
		currentXp = Mathf.Ceil (currentXp);
		return ( currentXp + "/" + nextLevel );
	}
}

