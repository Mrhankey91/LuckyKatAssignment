using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private int health = 1;
    private int maxHealth = 1;
    public int Health
    {
        get { return health; }
        set {
            if (value < health && !canTakeDamage) return; //Cant take damage so dont do anything.

            int oldHealth = health;
            health = Mathf.Clamp(value, 0, maxHealth);
            int change = health - oldHealth;
            onHealthChange?.Invoke(health, change);

            if(health <= 0) { onOutOfHealth?.Invoke(); }//no more health
        }
    }

    public bool canTakeDamage = true;

    public delegate void OnHealthChange(int health, int change);
    public OnHealthChange onHealthChange;

    public delegate void OnOutOfHealth();
    public OnOutOfHealth onOutOfHealth;

    public void SetFullHealth()
    {
        Health = maxHealth;
    }
}
