using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    [Header("General Stats")]
    public float bulletSpeed = 10f;
    public float bulletGravity = 1f;
    public Camera fpsCam;
    public GameObject muzzleFlash;
    public GameObject bullet;

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

        // on active le mf, voir IE wait pour régler le temps du mf
        muzzleFlash.SetActive(true);
        StartCoroutine(wait());

        // on fait apparaitre la balle a la position/orientation de la caméra joueur 
        GameObject currentBullet = Instantiate(bullet, fpsCam.transform.position, Quaternion.identity);
        currentBullet.transform.forward = fpsCam.transform.forward;
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * bulletSpeed, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(Vector3.down * bulletGravity, ForceMode.Impulse);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.12f);
        muzzleFlash.SetActive(false);

    }
}