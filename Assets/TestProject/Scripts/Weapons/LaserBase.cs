﻿using System;
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

    // Use this for initialization


    void Start()
    {
        //magic numbers, that's how big the plane is.
        originalLength = 10;
        transform.position += transform.forward * originalLength / 2;
        laserSpot = transform.GetChild(0).gameObject;
        laserSpot.transform.parent = null;
        LaserSpotSize *= .003f;
        laserSpot.transform.localScale = new Vector3(1, 1, 1) * LaserSpotSize;
        laserSpot.renderer.material.color = renderer.material.color;
        originalSpotBrightness = laserSpot.renderer.material.GetFloat("_Overbright");
        originalLaserScale = transform.localScale.z;
        originaLaserlLength = originalLaserScale * 10;
        transform.position += transform.forward * originaLaserlLength / 2;
        LateUpdate();
    }

    public void OnDestroy()
    {
        Destroy(laserSpot);
    }

    public void SetOrigin(Transform orig)
    {
        origin = orig;
    }

    public void LateUpdate()
    {
        //laser stuff
        if (origin == null)
            return;

        var castedRay = new Ray(origin.position, origin.forward);
        var allHits = Physics.RaycastAll(castedRay).Where(a => !(a.collider.CompareTag("Bullet") || a.collider.CompareTag("NoCollide")));
        var datHit = new RaycastHit();
        foreach (var raycastHit in allHits)
        {
            if (raycastHit.distance < datHit.distance || datHit.Equals(default(RaycastHit)))
                datHit = raycastHit;
        }
        //if we hit anything at all
        if (!datHit.Equals(default(RaycastHit)))
        {
            if (datHit.distance < originaLaserlLength)
            {
                var newLength = datHit.distance;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newLength / originaLaserlLength * originalLaserScale);
                transform.position = origin.position + transform.forward * newLength / 2;
            }
            var cross2 = castedRay.direction - datHit.normal * Vector3.Dot(datHit.normal, castedRay.direction);
            laserSpot.transform.rotation = Quaternion.LookRotation(cross2, datHit.normal);
            var stretchAmount = 1 / Mathf.Abs(Vector3.Dot(castedRay.direction, datHit.normal));
            laserSpot.transform.localScale = new Vector3(LaserSpotSize, LaserSpotSize, LaserSpotSize * stretchAmount);
            laserSpot.renderer.material.SetFloat("_Overbright", (originalSpotBrightness - .1f * transform.localScale.z / originalLaserScale));
            laserSpot.transform.position = castedRay.origin + castedRay.direction * datHit.distance + .01f * laserSpot.transform.up;
            laserSpot.renderer.enabled = true;

        }

        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, originalLaserScale);
            transform.position = origin.position + transform.forward * originaLaserlLength / 2;
            laserSpot.renderer.enabled = false;
        }
        var tilt = Mathf.Cos((transform.parent.eulerAngles.y + 45) * Mathf.Deg2Rad);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, tilt * 45);
        renderer.material.SetTextureScale("_Noise", new Vector2(transform.localScale.x, transform.localScale.z));
        renderer.material.mainTextureOffset = new Vector2(.5018f, 1 - transform.localScale.z / originalLaserScale);
        renderer.material.mainTextureScale = new Vector2(1, transform.localScale.z / originalLaserScale);


    }


}
