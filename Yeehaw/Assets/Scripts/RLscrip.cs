using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLscrip : MonoBehaviour
{
    [Header("General Stats")]
    public float bulletSpeed = 10f;
    public GameObject Missile;
    public GameObject spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Toha t'es un boss");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Toha t'es un boss");
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject currentBullet = Instantiate(Missile, spawnPoint.transform.position, Quaternion.identity);
        currentBullet.transform.forward = spawnPoint.transform.forward;
        currentBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.transform.forward * bulletSpeed, ForceMode.Impulse);
    }
}


