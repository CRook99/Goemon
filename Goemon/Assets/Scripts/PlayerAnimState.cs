using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimState : MonoBehaviour
{
    [Header("References")]
    public PlayerController controller;
    public KatanaScript ks;
    public Animator animator;

    [Header("Input")]
    public float velMagnitude;
    private float horizontalInput;
    private float verticalInput;

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
        chainWindow = false;
        attacking = false;
        stage = 0;

        lightAttacks = new int[] { light_1, light_2, light_3 };
        heavyAttacks = new int[] { heavy_1, heavy_2 };
    }

    void Update()
    {
        if (!dodging)
        {
            horizontalInput = Input.GetAxisRaw("JoyHorizontal");
            verticalInput = Input.GetAxisRaw("JoyVertical");
        }
        
        animator.SetFloat("VelMagnitude", velMagnitude);
        animator.SetFloat("HorizontalInput", horizontalInput);
        animator.SetFloat("VerticalInput", verticalInput);
        animator.SetBool("Dodging", dodging);
    }

    IEnumerator LightCombo()
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
    }

    IEnumerator OpenChainWindow(float time)
    {
        chainWindow = true;
        yield return new WaitForSeconds(time);
        chainWindow = false;
        if (!attacking)
            stage = 0;
    }
}
