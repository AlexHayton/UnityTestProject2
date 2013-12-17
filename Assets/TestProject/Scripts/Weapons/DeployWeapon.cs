using System;
using UnityEngine;

public class DeployableWeapon : Weapon  {
	public GameObject deployedPrefab;
	public GameObject ghostPrefab;
	private GameObject ghostDeploy;
	public float stickyRange;
	private Transform gripPoint;
	private bool snapped = false;
	
	public void Start()
	{
		base.Start();
		gripPoint = transform.root.FindChildRecursive("PlayerGrabPoint");
		ghostDeploy = Instantiate(ghostPrefab) as GameObject;
	}

	public override bool Attack() {
		Vector3 snapPoint;
		Quaternion snapRotation;
		
		if (GetSnapPoint(out snapPoint, out snapRotation) && CanBePlaced())
		{
			OnSuccessfulPlacement(snapPoint, snapRotation);
		}
	}
	
	public bool CanBePlaced()
	{
		return snapped;
	}
	
	public void OnSuccessfulPlacement(Vector3 position, Quaternion rotation)
	{
		Destroy(ghostDeploy);
		Instantiate(deployedPrefab) as GameObject;
	}
	
	public bool GetSnapPoint(out Vector3 snapPoint, out Quaternion rotation)
	{
		// Calculate position
		snapPoint = owner.transform.origin;
		rotation = owner.transform.rotation;
		Vector3 mousePos = GetMouseOnPlane(new Plane(Vector3.up, gripPoint.position));
		
		IEnumerable<Collider> hits = Physics.OverlapSphere(transform.position, stickyRange).Where(c => c.IsStatic());
		Collider target = hits.FirstOrDefault();
		if (target)
		{
			snapped = true;
			snapPoint = target.transform.origin;
		}
		else
		{
			snapped = false;
		}
		
		return snapPoint;
	}
	
	public void LateUpdate()
	{
		GetSnapPoint(out ghostDeploy.transform.position, out ghostDeploy.transform.rotation);
	}

}
