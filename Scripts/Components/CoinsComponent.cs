using UnityEngine;

public class CoinsComponent : MonoBehaviour
{
    private int coins = 0;
    public int Coins
    {
        get { return coins; }
        set { 
            int change = value - coins; 
            coins = value; 
            onCoinsUpdate?.Invoke(coins, change); 
        }
    }

    public delegate void OnCoinsUpdate(int coins, int change);
    public OnCoinsUpdate onCoinsUpdate;

    public bool HasEnoughCoins(int amount)
    {
        return coins >= amount;
    }

    public bool Buy(int cost)
    {
        if(coins >= cost)
        {
            Coins -= cost;
            return true;
        }
        else
        {
            return false;
        }
    }
}
