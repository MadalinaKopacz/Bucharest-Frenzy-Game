using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Powerup : MonoBehaviour
{
    [SerializeField] private int hpAddition;
    [SerializeField] private int damageAddition;
    [SerializeField] private float speedAddition;
    [SerializeField] private float jumpAddition;
    [SerializeField] private float powerupDuration;
    [SerializeField] private int price;

    [SerializeField] public string id;
    [SerializeField] private bool inShop;
    
    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }
    

    public int getHpAddition()
    {
        return hpAddition;
    }

    public void setHpAddition(int x)
    {
        hpAddition = x;
    }

    public int getDamageAddition()
    {
        return damageAddition;
    }

    public void setDamageAddition(int x)
    {
        damageAddition = x;
    }

    public float getSpeedAddition()
    {
        return speedAddition;
    }

    public void setSpeedAddition(float x)
    {
        speedAddition = x;
    }

    public float getJumpAddition()
    {
        return jumpAddition;
    }

    public void setJumpAddition(float x)
    {
        jumpAddition = x;
    }

    public float getPowerupDuration()
    {
        return powerupDuration;
    }

    public void setPowerupDuration(float x)
    {
        powerupDuration = x;
    }

    public int getPowerupPrice()
    {
        return price;
    }

    public void setPowerupPrice(int x)
    {
        price = x;
    }
}
