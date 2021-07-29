using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaScript : MonoBehaviour
{
    [Header("References")]
    public PlayerAnimState pas;
    public Collider hitbox;

    [Header("Attacks")]
    public AttackClass light_1;
    public AttackClass light_2;
    public AttackClass light_3;
    public AttackClass heavy_1;
    public AttackClass heavy_2;

    [Space]
    private AttackClass activeAttack;
    private AttackClass[] attacks;

    private void Start()
    {
        attacks = new AttackClass[] { light_1, light_2, light_3, heavy_1, heavy_2 };
        hitbox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        activeAttack = attacks[pas.stage];
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.SendMessage("TakeDamageAndStun", activeAttack);
        }
    }

    IEnumerator HitboxManager()
    {
        activeAttack = attacks[pas.stage];
        hitbox.enabled = false;
        yield return new WaitForSeconds(activeAttack.start_time);
        hitbox.enabled = true;
        yield return new WaitForSeconds(activeAttack.active_time);
        hitbox.enabled = false;
    }
}
