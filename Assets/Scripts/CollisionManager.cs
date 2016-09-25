using UnityEngine;
using System;

public class CollisionManager : MonoBehaviour
{
    public Action<bool> koreyTrigger;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Korey")
        {
            this.koreyTrigger.Invoke(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Korey")
        {
            this.koreyTrigger.Invoke(false);
        }
    }
}
