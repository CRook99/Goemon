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

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("JoyHorizontal");
        verticalInput = Input.GetAxisRaw("JoyVertical");
        direction = new Vector3(horizontalInput, 0f, verticalInput);

        if (!dodging)
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
        }
        

        transform.LookAt(new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));

        if (Input.GetButtonDown("Dodge"))
        {
            StartCoroutine(Dodge(direction));
        }

        if (Input.GetButtonDown("Light Attack"))
        {
            LightAttack();
        }

        if (Input.GetButtonDown("Heavy Attack"))
        {
            HeavyAttack();
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

    void LightAttack()
    {
        Debug.Log("Light");
        enemy.SendMessage("TakeDamageAndStun", new int[] { 10, 2 });
    }

    void HeavyAttack()
    {
        Debug.Log("Heavy");
        enemy.SendMessage("TakeDamageAndStun", new int[] { 20, 20 });
    }
}
