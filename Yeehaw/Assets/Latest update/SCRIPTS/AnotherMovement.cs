using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherMovement : MonoBehaviour
{
    [Header("Setup du character")]

    public CharacterController controller;  // character controller (composant a mettre sur objet unity)
    public Transform groundCheck; // Positions des pieds du Character
    public float groundDistance = 0.4f; // taille de la spherecast du groundcheck
    public LayerMask groundMask;  // Layer sur lequel on peut marcher et sauter

    [Header("Movement")]

    public float speed;
    public float acceleration = 7f;
    public float deceleration = 20f;
    public float WalkSpeed = 6f;
    public float Sprintspeed = 9f;
    public float CrouchSpeed = 3f;
    public float gravity = -9.81f;
    public float jump = 1f;
    public float Stamina = 4f;
    public float StaminaCounter;
    public float TempDeRecup = 5f;
    public float Recuperation;
    public float TrueGroundDelay = 1f;
    public float glideForce = 10f;
    public float slidingTimeLimit = 5f;

    [Header("Actions")]

    public Vector3 Zvelocity; // vélocité z Du Character (juste pour voir le mouveent en l'air selon velocité z)
    public bool isGrounded;  // boule quand on est au sol
    public bool jumping;  // boule pour bunnyjump
    public bool moving; // savoir si on bouge avec wasd pour ne pas build de la vitesse avec bunny sans bouger
    public bool sprinting;  // lorsque que la touche sprint est utilisée
    public bool SprintUsed; // lorsque le character est fatigué
    public bool crouching; // lorsquon saccroupis
    public bool sliding; // is it sliding
    public bool TrueGrounded; // called after being on ground more then x times 
    public bool KnockbackCheck;

    //privata data

    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);  // scale du joueur accroupis
    private Vector3 playerScale; // scale avant accroupissement
    private float StopSpeed = 0f; // vitesse à l'arret
    private float TrueGroundedTimer;
    private float slidingTime;
    private Vector3 move;

    private void Start()  // update is called at beginning blahblah
    {
        TrueGroundedTimer = 0;
        slidingTime = 0;
        StaminaCounter = Stamina;
        playerScale = transform.localScale;
    }


    // Update is called once per frame
    void Update()
    {
        InputsManager();
        Movement();
    }

    private void InputsManager() // Collecteur de données      
    {
        moving = Input.GetButton("Vertical") || Input.GetButton("Horizontal"); // si on se déplace, le bool déplacement s'active.
        jumping = Input.GetButton("Jump");  // quand jump est ou reste appuyé jumping = true
        sprinting = Input.GetKey(KeyCode.LeftShift); // quand shit est utilisé sprint activé
        crouching = Input.GetKey(KeyCode.LeftControl); // left ctrl pour s'accroupir
    }

    private void Movement()
    {
        // On verifie d'abord la position du character

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);  // on vérifie si on est sur le sol avec un spherecast depuis les pieds

        if (isGrounded)     // on verifie quon est bien au sol et pas en train de bunny
        {
            TrueGroundedTimer += Time.deltaTime;
            if (TrueGroundedTimer >= TrueGroundDelay)
            {
                TrueGrounded = true;
            }
        }

        if (!isGrounded)
        {
            TrueGrounded = false;
            TrueGroundedTimer = 0;
        }

        // Mouvement basic (marche)

        float x = Input.GetAxis("Horizontal");  // tranformation du input wasd en données
        float z = Input.GetAxis("Vertical");    // tranformation du input wasd en données

        Vector3 move = transform.right * x + transform.forward * z;  // détermine la direction du mouvement selon les inputs sur l'axe horizontale (transforms)

        controller.Move(move * speed * Time.deltaTime);  // Bouge le character sur le plan horizontal selon input

        // Saut et bunny jumping

        Zvelocity.y += gravity * Time.deltaTime; // accumulation de la gravité quand on est pas au sol

        controller.Move(Zvelocity * Time.deltaTime);  // on bouge le character sur l'axe z selon la vélocité z


        if (jumping && isGrounded)  // si jumping = true && au sol, on saute (bunny)

        {
            Zvelocity.y = Mathf.Sqrt(jump * -2f * gravity);  //  vélocité y (altitude) = racine de la puissance saut fois la gravité
        }

        if (!isGrounded && moving)  // augmentation de la vitesse quand on fait du bunny

        {
            speed += 1 * Time.deltaTime;  // plus on est en l'air (bunny) plus on va vite
        }

        // Sprint, Fatigue et récupération

        if (sprinting && isGrounded && speed >= Sprintspeed && !SprintUsed)  // Sprint
        { 
            StaminaCounter -= Time.deltaTime; // on diminue la stamina
            if (StaminaCounter <= 0)    // point de fatigue
            {
                Debug.Log("Plus de jus, récupération!");
                SprintUsed = true;  // on utilise sprintused
                Recuperation = 0; // reset du timer de récupération a 0
            }

        }

        if (SprintUsed) // Récupération
        {
            Recuperation += Time.deltaTime; // augmentation du délais de récupération
            if (Recuperation >= TempDeRecup) // lorsqu'on a récupéré,
            {
                SprintUsed = false;     // on déclare la fatigue terminée   
                StaminaCounter = Stamina;   // on remet la stamina au maximum
            }
        }

        // Crouching

        if (crouching)
        {
            transform.localScale = crouchScale;
            if (speed > WalkSpeed + 1 && TrueGrounded && !sliding)      //super glide
            {
                speed += glideForce;
                sliding = true;
            }
            if (sliding)            // timer super glide quand on glisse sur le sol ( pas de spam superglide )
            {
                slidingTime += Time.deltaTime;
                if (slidingTime >= slidingTimeLimit)
                {
                    sliding = false;
                    slidingTime = 0;
                }
            }
        }

        if (!crouching)
        {
            transform.localScale = playerScale;
            sliding = false;
        }

        // Contre mouvements et limitations de vitesse

        if (isGrounded && Zvelocity.y < 0)
        {
            Zvelocity.y = -2f;  // on reset la gravité

            if (moving)
            {
                if (speed > WalkSpeed && !sprinting || 
                    SprintUsed && speed > WalkSpeed ||
                    speed > Sprintspeed && sprinting ||
                    crouching && speed > CrouchSpeed) // ralentissements en cas de vitesse supérieure 
                {
                    speed -= deceleration * Time.deltaTime;
                }
                else
                {
                    speed += acceleration * Time.deltaTime;
                }
            }
            if (!moving && speed > StopSpeed)
            {
                speed -= deceleration * 2 * Time.deltaTime;
            }
        }
    }
    // essai knockback
    public void Knockback(Vector3 direction)
    {
        controller.Move(direction * speed * Time.deltaTime);
        Zvelocity.y = 5;
    }
        
}

