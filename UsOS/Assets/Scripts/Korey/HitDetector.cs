using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "GuardBot")
        {
            WalkingGurad guardRobot = collider2D.gameObject.GetComponent<WalkingGurad>();
            guardRobot.Dead(true);
        }
    }
}