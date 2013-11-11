using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;

public class LaserBase : MonoBehaviour
{
    private float originalLength;
    private Transform origin;
    private GameObject laserSpot;
    private float originalSpotBrightness;
    public float LaserSpotSize;

    private float originaLaserlLength;
    private float originalLaserScale;

    private float radsPerDeg = 2f * Mathf.PI / 360f;

    // Use this for initialization


    void Start()
    {
        //magic numbers, that's how big the plane is.
        originalLength = 10;
        transform.position += transform.forward * originalLength / 2;
        laserSpot = transform.GetChild(0).gameObject;
        laserSpot.transform.parent = null;
        LaserSpotSize *= .01f;
        laserSpot.transform.localScale = new Vector3(1, 1, 1) * LaserSpotSize;
        originalSpotBrightness = laserSpot.renderer.material.GetFloat("_Overbright");
        originalLaserScale = transform.localScale.z;
        originaLaserlLength = originalLaserScale * 10;
        transform.position += transform.forward * originaLaserlLength / 2;
    }

    public void SetOrigin(Transform orig)
    {
        origin = orig;
    }

    public void Update()
    {
        ///laser stuff
        if (origin == null)
            return;
        Ray ray = new Ray(origin.position, transform.forward);
        var hits = new List<RaycastHit>(Physics.RaycastAll(ray));
        var nonColliderHits = hits.ToList();

        if (nonColliderHits.Any())
        {
            RaycastHit closestHit = nonColliderHits[0];
            foreach (var ncHit in nonColliderHits)
            {
                if (ncHit.distance < closestHit.distance)
                {
                    closestHit = ncHit;
                }
            }
            print(closestHit.collider);
            if (closestHit.distance < originaLaserlLength)
            {
                var newLength = closestHit.distance;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newLength / originaLaserlLength * originalLaserScale);
                transform.position = origin.position + transform.forward * newLength / 2;
            }
            var cross2 = ray.direction - closestHit.normal * Vector3.Dot(closestHit.normal, ray.direction);
            laserSpot.transform.rotation = Quaternion.LookRotation(cross2, closestHit.normal);
            var stretchAmount = 1 / Mathf.Abs(Vector3.Dot(ray.direction, closestHit.normal));
            laserSpot.transform.localScale = new Vector3(LaserSpotSize, LaserSpotSize, LaserSpotSize * stretchAmount);
            laserSpot.renderer.material.SetFloat("_Overbright", (originalSpotBrightness - transform.localScale.z / originalLaserScale));
            laserSpot.transform.position = origin.position + origin.forward * closestHit.distance + .001f * laserSpot.transform.up;
        }

        else
        {
            laserSpot.renderer.material.SetFloat("_Overbright", 0);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, originalLaserScale);
            transform.position = origin.position + transform.forward * originaLaserlLength / 2;
        }
        var tilt = Mathf.Cos(transform.eulerAngles.y * radsPerDeg + 45);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, tilt * 45);
        renderer.material.SetTextureScale("_Noise", new Vector2(transform.localScale.x, transform.localScale.z));
        renderer.material.mainTextureOffset = new Vector2(.5018f, 1 - transform.localScale.z / originalLaserScale);
        renderer.material.mainTextureScale = new Vector2(1, transform.localScale.z / originalLaserScale);


    }


}
