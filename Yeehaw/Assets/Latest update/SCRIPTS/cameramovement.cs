using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    float xRotation = 0f;  // float nécessaire pour le mouvement tete haut bas
    public Transform playerBody; // modèle du joueur

    // Start is called before the first frame update
    void Start()
    {
        // annule le cruseur quand on fait play depuis unity
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Données mouvements de la souris
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Rotation du corp en meme temps que la ceméra (droite gauche)
        playerBody.Rotate(Vector3.up * mouseX);


        // Rotation de la tete haut bas 
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
