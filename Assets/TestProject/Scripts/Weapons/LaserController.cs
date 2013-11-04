using UnityEngine;

public class LaserController : MonoBehaviour
{
	public float defaultTiling;
	private GameObject laserSpot;

	public void Start()
	{
		this.UpdateLaser();
	}

    public void Update()
    {
    	this.UpdateLaser();
    }
    
    public void UpdateLaser()
    {
    	if (this.renderer)
    	{
    		// adjust tiling here.
    	}
    }

}
