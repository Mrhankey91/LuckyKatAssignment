using UnityEngine;

public class SpikesBlock : SpawnObject, IDamage
{
    public int damage = 1;

    private float rotateSpeed = 30f;

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    public int Damage()
    {
        return damage;
    }
}
