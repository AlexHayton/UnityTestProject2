using System;
public class MeleeWeapon : Weapon  {

	public override bool OnPrimaryAttack()
	{
		return true;
	}

	protected void CheckHit() {
		throw new System.Exception("Not implemented");
	}

}
