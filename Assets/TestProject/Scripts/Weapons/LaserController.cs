using UnityEngine;

public class LaserController : MonoBehaviour
{
	private RigidPlayerScript playerScript;
	private Vector2 defaultTiling;
	private Vector3 defaultScale;
	private GameObject laserSpot;
	public GameObject laserLine;
	
	private const float MAX_LASER_DISTANCE = 100.0f;

	public void Start()
	{
		playerScript = transform.root.GetComponentInChildren<RigidPlayerScript>();
		if (this.laserLine != null)
		{
			defaultTiling = this.laserLine.renderer.material.mainTextureScale;
			defaultLength = this.laserLine.scale.z;
		}
		this.UpdateLaser();
	}

    public void Update()
    {
    	this.UpdateLaser();
    }
    
    public void UpdateLaser()
    {
    	// Cast a ray from our position to the mouse cursor.
    	var endPoint = playerScript.GetMouseOnPlane();
		var direction = endPoint - this.position;
		direction.y = 0;
		direction.Normalize();
		
		Ray ray = new Ray(this.position, direction);
		HitInfo hit;
		Vector3 landingPoint;
		float distance;
		if (Collider.rayCast(ray, ref hit, MAX_LASER_DISTANCE))
		{
			// We hit something
			landingPoint = hit.point;
			distance = hit.distance;
		}
		else
		{
			// We are aiming way into the distance
			landingPoint = endPoint + direction*MAX_LASER_DISTANCE;
			distance = MAX_LASER_DISTANCE;
		}
    	
    	// adjust scale and tiling here.
		if (this.laserLine != null)
		{
    		this.laserLine.renderer.material.mainTextureScale = new Vector2 (defaultTiling.x * distance / defaultLength, defaultTiling.y);
		}
    	
    	// Adjust laser spot position here.
    	if (this.laserSpot != null)
    	{
    		this.laserSpot.position = landingPoint;
    	}
    }

}
