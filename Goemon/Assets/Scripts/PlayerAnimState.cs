using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimState : MonoBehaviour
{
    public Animator animator;
    public float velMagnitude;
    private float horizontalInput;
    private float verticalInput;

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("JoyHorizontal");
        verticalInput = Input.GetAxisRaw("JoyVertical");

        animator.SetFloat("VelMagnitude", velMagnitude);
        animator.SetFloat("HorizontalInput", horizontalInput);
        animator.SetFloat("VerticalInput", verticalInput);
    }
}
