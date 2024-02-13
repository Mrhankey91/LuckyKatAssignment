using UnityEngine;

public class Spikes : MonoBehaviour, IDamage
{
    public int damage = 1;

    public int Damage()
    {
        return damage;
    }
}
