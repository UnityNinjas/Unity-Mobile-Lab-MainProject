using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "GuardBot")
        {
            WalkingGuard guardRobot = collider2D.gameObject.GetComponent<WalkingGuard>();
            guardRobot.Dead(true);
        }
    }
}