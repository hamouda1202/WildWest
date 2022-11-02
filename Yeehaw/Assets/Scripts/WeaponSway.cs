using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    public float amount;
    public float maxAmount;
    public float smoothAmount;

    private Vector3 initialposition;


    // Start is called before the first frame update
    void Start()
    {
        initialposition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;

        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        Vector3 finalposition = new Vector3(movementY, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalposition + initialposition, Time.deltaTime * smoothAmount);
    }
}
