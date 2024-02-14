using UnityEngine;

public class Spikes : SpawnObject, IDamage
{
    public int damage = 1;

    public int Damage()
    {
        return damage;
    }
}
