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
    private GameObject gun;
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
        laserSpot.renderer.material.color = renderer.material.color;
        originalSpotBrightness = laserSpot.renderer.material.GetFloat("_Overbright");
        originalLaserScale = transform.localScale.z;
        originaLaserlLength = originalLaserScale * 10;
        transform.position += transform.forward * originaLaserlLength / 2;
        gun = transform.parent.parent.gameObject;
    }

    public void OnDestroy()
    {
        Destroy(laserSpot);
    }

    public void SetOrigin(Transform orig)
    {
        origin = orig;
    }

    public void Update()
    {
        //laser stuff
        if (origin == null)
            return;
        var arrayOfRays = new List<Ray>();
        var arrayOfManyRayHits = new List<List<RaycastHit>>();

        var chosenRay = new Ray();
        var closestEnemyHit = new RaycastHit();
        for (var i = -20; i <= 20; i += 5)
        {
            var thisRay = new Ray(origin.position, Quaternion.AngleAxis(i, gun.transform.parent.right) * gun.transform.parent.forward);
            arrayOfRays.Add(thisRay);

            var hitsOnThisRay = Physics.RaycastAll(thisRay).Where(a => !a.transform.gameObject.CompareTag("Bullet")).ToList();
            if (!hitsOnThisRay.Any())
                continue;

            arrayOfManyRayHits.Add(hitsOnThisRay);
            var closestHitOnThisRay = hitsOnThisRay.First(a => a.distance == hitsOnThisRay.Min(b => b.distance));
            if (closestHitOnThisRay.transform.gameObject.GetComponent<AIBase>() != null &&
                (closestHitOnThisRay.distance < closestEnemyHit.distance || closestEnemyHit.Equals(default(RaycastHit))))
            {
                closestEnemyHit = closestHitOnThisRay;
                chosenRay = new Ray(origin.position, closestEnemyHit.transform.position - origin.position);
            }
        }

        //no enemies found
        if (closestEnemyHit.Equals(default(RaycastHit)))
        {
            chosenRay = arrayOfRays[arrayOfRays.Count / 2];
        }

        var allHits = Physics.RaycastAll(chosenRay).ToList();
        var datHit = allHits.FirstOrDefault(a => !a.transform.gameObject.CompareTag("Bullet") && a.distance == allHits.Min(b => b.distance));

        //if we hit anything at all
        if (!datHit.Equals(default(RaycastHit)))
        {
            gun.transform.rotation = Quaternion.LookRotation(chosenRay.direction, gun.transform.parent.up);
            if (datHit.distance < originaLaserlLength)
            {
                var newLength = datHit.distance;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newLength / originaLaserlLength * originalLaserScale);
                transform.position = origin.position + transform.forward * newLength / 2;
            }
            var cross2 = chosenRay.direction - datHit.normal * Vector3.Dot(datHit.normal, chosenRay.direction);
            laserSpot.transform.rotation = Quaternion.LookRotation(cross2, datHit.normal);
            var stretchAmount = 1 / Mathf.Abs(Vector3.Dot(chosenRay.direction, datHit.normal));
            laserSpot.transform.localScale = new Vector3(LaserSpotSize, LaserSpotSize, LaserSpotSize * stretchAmount);
            laserSpot.renderer.material.SetFloat("_Overbright", (originalSpotBrightness - .1f * transform.localScale.z / originalLaserScale));
            laserSpot.transform.position = chosenRay.origin + chosenRay.direction * datHit.distance + .01f * laserSpot.transform.up;
            laserSpot.renderer.enabled = true;
        }

        else
        {
            gun.transform.rotation = Quaternion.LookRotation(gun.transform.parent.forward, gun.transform.parent.up);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, originalLaserScale);
            transform.position = origin.position + transform.forward * originaLaserlLength / 2;
            laserSpot.renderer.enabled = false;
        }
        var tilt = Mathf.Cos(transform.parent.eulerAngles.y * radsPerDeg + 45);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, tilt*45);
        renderer.material.SetTextureScale("_Noise", new Vector2(transform.localScale.x, transform.localScale.z));
        renderer.material.mainTextureOffset = new Vector2(.5018f, 1 - transform.localScale.z / originalLaserScale);
        renderer.material.mainTextureScale = new Vector2(1, transform.localScale.z / originalLaserScale);


    }


}
