using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public HealthBar healthBar;
    public GameObject player;
    public Rigidbody rb;

    [Header("Vitals")]
    [SerializeField] int health;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int stun;
    [SerializeField] int maxStun = 50;
    public bool stunned;

    private void Awake()
    {
        health = maxHealth;
        stun = 0;
        stunned = false;

        healthBar.SendMessage("Initialise", new int[] { maxHealth, maxStun });
    }

    void Update()
    {
        if (health <= 0 || stun >= maxStun)
        {
            stunned = true;
            healthBar.SendMessage("BecomeStunned");
        }

        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }

    void TakeDamageAndStun(AttackClass attack)
    {
        health -= attack.damage;
        stun += attack.stun;

        StartCoroutine(Hitstop(attack.hitstop));

        healthBar.SendMessage("SetHealthAndStun", new int[] { health, stun });
    }

    IEnumerator Hitstop(float time)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

    void AddDriveForce(float thrustDistance)
    {
        Vector3 thrustVector = new Vector3();
        thrustVector = transform.forward * thrustDistance;
        rb.AddForce(thrustVector);
    }
}
