using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public GameObject enemy;
    public PlayerAnimState pas;
    public FinisherScript fs;
    public Collider hurtbox;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;
    private Vector3 direction;

    [Header("Movement")]
    [SerializeField] float speed = 10f;
    [SerializeField] float dodgeDistance = 150f;
    [SerializeField] bool moving;
    [SerializeField] bool dodging;
    [SerializeField] bool attacking;
    [SerializeField] bool canAttack;
    [SerializeField] bool executing;

    private void Awake()
    {
        canAttack = true;
        dodging = false;
        executing = false;
    }

    void Update()
    {
        if (!executing)
        {
            attacking = pas.attacking;
            pas.dodging = dodging;

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
                    rb.angularVelocity = Vector3.zero;
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
                if (enemy.GetComponentInParent<EnemyController>().stunned == true)
                {
                    executing = true;
                    fs.SendMessage("PrepareFinisher");
                }
                else if (!attacking)
                {
                    canAttack = false;
                    rb.velocity = Vector3.zero;
                    pas.SendMessage("HeavyCombo");
                }
            }

            pas.velMagnitude = direction.magnitude;
        }
    }

    IEnumerator Dodge(Vector3 direction)
    {
        dodging = true;
        Vector3 dodgeVector = new Vector3();
        dodgeVector = transform.right * horizontalInput * dodgeDistance;
        dodgeVector += transform.forward * verticalInput * dodgeDistance;
        rb.AddForce(dodgeVector);
        hurtbox.enabled = false;
        yield return new WaitForSeconds(0.3f);
        hurtbox.enabled = true;
        yield return new WaitForSeconds(0.5f);
        dodging = false;
    }

    void ComboEnded()
    {
        canAttack = true;
    }

    void AddDriveForce(float thrustDistance)
    {
        Vector3 thrustVector = new Vector3();
        thrustVector = transform.forward * thrustDistance;
        rb.AddForce(thrustVector);
    }
}
