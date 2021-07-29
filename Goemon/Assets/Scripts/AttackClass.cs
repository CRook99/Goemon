using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Attack", menuName = "Attack")]
public class AttackClass : ScriptableObject
{
    public int damage;
    public int stun;
    public float hitstop;
    public float start_time;
    public float active_time;
}
