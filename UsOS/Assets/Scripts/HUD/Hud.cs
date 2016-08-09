using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Scrollbar battery;

    public void Start()
    {
        UpdateBattery();
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        
    }

    private void UpdateBattery()
    {
        this.battery.size = Player.batteryPower;
    }
}
