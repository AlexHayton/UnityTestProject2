using UnityEngine;
using System.Collections;
using System;

public class XPHandler : MonoBehaviour {
	
	public float xp = 0;
	public float xpPerLevel = 1000;
	public float xpScalarPerLevel = 1.5f;

	// Use this for initialization
	void Start () {
		this.xp = 0;
	}
	
	public float GetXp() {	
		return this.xp;
	}
	
	public void AddXp(float xpToAdd)
	{
		this.xp += xpToAdd;
	}
	
	public float GetXpForLevel(int level)
	{
		return (float) (xpPerLevel * xpScalarPerLevel * level);
	}
	
	public int GetNextLevel()
	{
		return this.GetLevel() + 1;
	}
	
	public int GetLevel()
	{
		float lvlFloat = (float)(this.GetXp() / (xpPerLevel * xpScalarPerLevel));
		return (int)Math.Floor(lvlFloat);
	}
	
	public float GetXpForThisLevel()
	{
		return this.GetXpForLevel(this.GetLevel());
	}
	
	public float GetXpForNextLevel()
	{
		return this.GetXpForLevel(this.GetNextLevel());
	}
	
	public float GetXpTillNextLevelScalar()
	{
		float thisLevel = this.GetXpForThisLevel();
		float nextLevel = this.GetXpForNextLevel();
		float currentXp = this.GetXp();
		return (currentXp - thisLevel) / nextLevel;
	}

}
