using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour, IDamage
{
    public int damage = 1;

    public int Damage()
    {
        return damage;
    }
}
