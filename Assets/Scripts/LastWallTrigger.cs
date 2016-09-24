using UnityEngine;

public class LastWallTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Korey")
        {
            Hud.instance.FinishScreenOpen();
        }
    }
}
