using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public EnemyHealthBar healthBar;

    [Header("Vitals")]
    [SerializeField] int health;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int stun;
    [SerializeField] int maxStun = 50;
    [SerializeField] bool alive;
    [SerializeField] bool stunned;

    private void Awake()
    {
        alive = true;
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
    }

    void TakeDamageAndStun(int[] values)
    {
        health -= values[0];
        stun += values[1];

        healthBar.SendMessage("SetHealthAndStun", new int[] { health, stun });
    }
}
