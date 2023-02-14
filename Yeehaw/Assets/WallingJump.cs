using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallingJump : MonoBehaviour
{

    public AnotherMovement thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<AnotherMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            thePlayer.Knockback(hitDirection);
            thePlayer.KnockbackCheck = true;
        }
    }
}
