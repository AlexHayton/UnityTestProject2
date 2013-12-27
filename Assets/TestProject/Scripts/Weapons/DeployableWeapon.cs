using System;
using UnityEngine;
using TestProject;
using System.Collections.Generic;
using System.Linq;

public class DeployableWeapon : Weapon  {
	public GameObject deployedPrefab;
	public GameObject ghostPrefab;
	private GameObject ghostDeploy;
	public float stickyRange;
	private bool snapped = false;
	
	public override void Start()
	{
		base.Start();

		ghostDeploy = Instantiate(ghostPrefab) as GameObject;
		ghostDeploy.transform.parent = this.transform;
	}

	public override bool OnPrimaryAttack() {
		Vector3 snapPoint;
		Quaternion snapRotation;

		bool canBePlaced = GetSnapPoint(out snapPoint, out snapRotation) && CanBePlaced();
		if (canBePlaced)
		{
			OnSuccessfulPlacement(snapPoint, snapRotation);
		}

		return canBePlaced;
	}
	
	public bool CanBePlaced()
	{
		return snapped && !IsInCooldown();
	}
	
	public void OnSuccessfulPlacement(Vector3 position, Quaternion rotation)
	{
		Instantiate(deployedPrefab);
		deployedPrefab.transform.position = position;
		deployedPrefab.transform.Rotate(rotation.eulerAngles);
	}
	
	public bool GetSnapPoint(out Vector3 snapPoint, out Quaternion rotation)
	{
		// Calculate position
		snapPoint = owner.transform.position;
		rotation = owner.transform.rotation;
		Vector3 mousePos = PlayerUtility.GetMouseOnPlane(transform, new Plane(Vector3.up, gripPoint.position));

		snapped = true;
		snapPoint = mousePos;
		rotation = Quaternion.identity;

		/*IEnumerable<Collider> hits = Physics.OverlapSphere(mousePos, stickyRange).Where(c => c.gameObject.isStatic);
		Collider target = hits.FirstOrDefault();
		if (target)
		{
			snapped = true;
			// Nudge the snap point up a bit to prevent tearing
			snapPoint = target.transform.position + (target.transform.forward * 0.2f);
			rotation = Quaternion.FromToRotation(target.transform.position, target.transform.position + target.transform.forward);
		}
		else
		{
			snapped = false;
		}*/
		
		return snapped;
	}
	
	public void LateUpdate()
	{
		snapped = false;
		if (!IsInCooldown())
		{
			Vector3 position;
			Quaternion rotation;
			GetSnapPoint(out position, out rotation);

			if (snapped)
			{
				ghostDeploy.renderer.enabled = true;
				ghostDeploy.transform.position = position;
				ghostDeploy.transform.rotation = rotation;
			}
		}

		if (!snapped)
		{
			ghostDeploy.renderer.enabled = false;
		}
	}

}
