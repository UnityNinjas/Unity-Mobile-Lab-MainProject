using System;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Action<bool> KoreyTrigger;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Korey")
        {
            this.KoreyTrigger.Invoke(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Korey")
        {
            this.KoreyTrigger.Invoke(false);
        }
    }
}