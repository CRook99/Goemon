using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    //public KatanaScript ks;
    public Animator animator;
    public EnemyController controller;
    public Rigidbody rb;

    [Header("Input")]
    public float velMagnitude;
    private float horizontalInput;
    private float verticalInput;
    private float speed = 1.5f;

    [Header("Animations")]
    [SerializeField] bool chainWindow;
    public bool attacking;
    public bool dodging;
    public int stage;

    int light_1 = Animator.StringToHash("LightAttack_1");
    int light_2 = Animator.StringToHash("LightAttack_2");
    int light_3 = Animator.StringToHash("LightAttack_3");

    int heavy_1 = Animator.StringToHash("HeavyAttack_1");
    int heavy_2 = Animator.StringToHash("HeavyAttack_2");

    int thrust = Animator.StringToHash("Thrust");

    int[] lightAttacks;
    int[] heavyAttacks;

    private void Awake()
    {
        //chainWindow = false;
        attacking = false;
        stage = 0;

        lightAttacks = new int[] { light_1, light_2, light_3 };
        heavyAttacks = new int[] { heavy_1, heavy_2 };
    }

    private void Start()
    {
        StartCoroutine(Walk());
    }

    void Update()
    {
        velMagnitude = new Vector3(horizontalInput, 0f, verticalInput).magnitude;

        animator.SetFloat("VelMagnitude", velMagnitude);
        animator.SetFloat("HorizontalInput", horizontalInput);
        animator.SetFloat("VerticalInput", verticalInput);
        //animator.SetBool("Dodging", dodging);

        if (velMagnitude < 0.1f)
            rb.angularVelocity = Vector3.zero;

        rb.velocity = transform.right * horizontalInput * speed; // Handles lateral movement
        rb.velocity += transform.forward * verticalInput * speed; // Handles vertical movement. = then += to add the velocities: = and = would overwrite the first, += and += would make velocity extremely big
    }

    IEnumerator Walk()
    {
        int direction = Random.Range(0, 3); // 0 - forward; 1 - right; 2 - back; 3 - left
        float walkTime = Random.Range(1f, 5f);

        Debug.Log(direction);
        Debug.Log(walkTime);

        switch (direction)
        {
            case 0:
                horizontalInput = 0f;
                verticalInput = 1f;
                break;
            case 1:
                horizontalInput = 1f;
                verticalInput = 0f;
                break;
            case 2:
                horizontalInput = 0f;
                verticalInput = -1f;
                break;
            case 3:
                horizontalInput = -1f;
                verticalInput = 0f;
                break;
        }

        yield return new WaitForSeconds(walkTime);
        horizontalInput = 0f;
        verticalInput = 0f;

        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        int combo = Random.Range(0, 1);
        
        switch (combo)
        {
            case 0:
                animator.Play(light_1);
                yield return new WaitForSeconds(0.9f);
                controller.SendMessage("AddDriveForce", 50f);

                animator.Play(light_2);
                yield return new WaitForSeconds(0.8f);
                controller.SendMessage("AddDriveForce", 50f);

                animator.Play(light_3);
                yield return new WaitForSeconds(1.5f);
                controller.SendMessage("AddDriveForce", 250f);

                break;

            case 1:
                animator.Play(heavy_1);
                yield return new WaitForSeconds(1.6f);
                controller.SendMessage("AddDriveForce", 250f);

                animator.Play(heavy_2);
                yield return new WaitForSeconds(2.5f);
                controller.SendMessage("AddDriveForce", 150f);

                break;
        }

        StartCoroutine(Walk());
    }

    /*IEnumerator LightCombo()
    {
        StopCoroutine("OpenChainWindow");
        attacking = true;
        chainWindow = false;

        animator.Play(lightAttacks[stage]);
        ks.SendMessage("HitboxManager");

        switch (stage)
        {
            case 0:
                controller.SendMessage("AddDriveForce", 50f, SendMessageOptions.RequireReceiver);
                yield return new WaitForSeconds(0.5f);
                stage++;
                break;
            case 1:
                controller.SendMessage("AddDriveForce", 50f, SendMessageOptions.RequireReceiver);
                yield return new WaitForSeconds(0.4f);
                stage++;
                break;
            case 2:
                stage = 0;
                controller.SendMessage("AddDriveForce", 250f, SendMessageOptions.RequireReceiver);
                yield return new WaitForSeconds(1.5f);
                break;
        }

        StartCoroutine(OpenChainWindow(0.5f));
        attacking = !attacking;
    }

    IEnumerator HeavyCombo()
    {
        if (stage != 4)
            stage = 3;

        StopCoroutine("OpenChainWindow");
        attacking = true;
        chainWindow = false;

        animator.Play(heavyAttacks[stage - 3]);
        ks.SendMessage("HitboxManager");

        switch (stage)
        {
            case 3:
                controller.SendMessage("AddDriveForce", 250f, SendMessageOptions.RequireReceiver);
                yield return new WaitForSeconds(1f);
                stage++;
                break;
            case 4:
                controller.SendMessage("AddDriveForce", 150f, SendMessageOptions.RequireReceiver);
                yield return new WaitForSeconds(2f);
                stage = 0;
                break;
        }

        StartCoroutine(OpenChainWindow(0.5f));
        attacking = !attacking;
    }*/

    /*IEnumerator OpenChainWindow(float time)
    {
        chainWindow = true;
        yield return new WaitForSeconds(time);
        chainWindow = false;
        if (!attacking)
            stage = 0;
    }*/
}
