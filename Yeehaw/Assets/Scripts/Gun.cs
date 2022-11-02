using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    [Header("General Stats")]
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public GameObject muzzleFlash;

    [Header("Laser")]
    public GameObject laser;
    public Transform positionDépart;
    public float fadeDuration = 0.3f;


    private void Start()
    {
        muzzleFlash.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.SetActive(true);
        StartCoroutine(wait());
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            CreateLaser(hit.point);
        } else
        {
            CreateLaser(fpsCam.transform.position + fpsCam.transform.forward * range);
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.12f);
        muzzleFlash.SetActive(false);
    }

    void CreateLaser(Vector3 end)
    {
        LineRenderer lr = Instantiate(laser).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { positionDépart.position, end});
        StartCoroutine(FadeLaser(lr));
    }

    IEnumerator FadeLaser(LineRenderer lr)
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alpha);
            yield return null;
        }
    }
}
