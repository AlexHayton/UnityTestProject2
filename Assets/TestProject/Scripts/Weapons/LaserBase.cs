using UnityEngine;
using System.Collections;

public class LaserBase : MonoBehaviour
{
    private float originalLength;
    private Vector3 origin;
    private Material laserMaterial;
	// Use this for initialization
	void Start ()
    {
        //magic numbers, that's how big the plane is.
        originalLength = 10;
        origin = transform.position;
        transform.position += transform.forward * originalLength/2;
        laserMaterial = gameObject.GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Ray ray = new Ray(transform.parent.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.distance < originalLength * transform.localScale.z)
        {
            var newLength = hit.distance;
            transform.localScale.Set(1,1,newLength/originalLength);
            transform.position = origin + transform.forward * newLength / 2;
            laserMaterial.mainTextureOffset.Set(.5f, 1 - transform.localScale.z);
            laserMaterial.mainTextureScale.Set(1,transform.localScale.z);
        }
	}
}
