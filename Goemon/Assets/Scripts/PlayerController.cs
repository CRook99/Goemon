using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("External Objects")]
    public Rigidbody rb;
    public GameObject enemy;
    public PlayerAnimState pas;

    // MOVEMENT
    [SerializeField] float speed = 10f;
    [SerializeField] float dodgeDistance = 150f;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 direction;
    [SerializeField] bool moving;
    [SerializeField] bool dodging;
    [SerializeField] bool attacking;
    [SerializeField] bool canAttack;

    private void Awake()
    {
        canAttack = true;
        dodging = false;
    }

    void Update()
    {
        attacking = pas.attacking;

        horizontalInput = Input.GetAxisRaw("JoyHorizontal");
        verticalInput = Input.GetAxisRaw("JoyVertical");
        direction = new Vector3(horizontalInput, 0f, verticalInput);

        if (!dodging && !attacking)
        {
            if (direction.magnitude >= 0.1f)
            {
                moving = true;
                rb.velocity = transform.right * horizontalInput * speed; // Handles lateral movement
                rb.velocity += transform.forward * verticalInput * speed; // Handles vertical movement. = then += to add the velocities: = and = would overwrite the first, += and += would make velocity extremely big
            }
            else
            {
                moving = false;
                rb.velocity = Vector3.zero;
            }

            if (Input.GetButtonDown("Dodge"))
            {
                StartCoroutine(Dodge(direction));
            }
        }
        

        transform.LookAt(new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));

        

        if (Input.GetButtonDown("Light Attack") && !attacking)
        {
            canAttack = false;
            rb.velocity = Vector3.zero;
            pas.SendMessage("LightCombo");
        }

        if (Input.GetButtonDown("Heavy Attack"))
        {
            // do stuff
        }

        pas.velMagnitude = direction.magnitude;
    }

    IEnumerator Dodge(Vector3 direction)
    {
        dodging = true;
        Vector3 dodgeVector = new Vector3();
        dodgeVector = transform.right * horizontalInput * dodgeDistance;
        dodgeVector += transform.forward * verticalInput * dodgeDistance;
        rb.AddForce(dodgeVector);
        yield return new WaitForSeconds(0.8f);
        dodging = false;
    }

    void ComboEnded()
    {
        canAttack = true;
    }

    void Light_3_Thrust()
    {
        Debug.Log("Thrust");
        rb.AddForce(0f, 0f, 250f);
    }
}
