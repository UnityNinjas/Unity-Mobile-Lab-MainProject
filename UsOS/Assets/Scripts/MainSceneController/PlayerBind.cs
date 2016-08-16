using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerBind : MonoBehaviour
{
    public Transform beam;
    public static Vector2 touchPosition;
    private Vector2 currentTouchPosition;

    public void Update()
    {
        this.currentTouchPosition = touchPosition;
        CalculateNewPosition();
    }

    public void CalculateNewPosition()
    {
        //Vector2.Distance(new Vector2(touchPosition.x/100, touchPosition.y/100), this.beam.localPosition);
        //Debug.Log(Vector2.Distance(new Vector2(touchPosition.x / 100, touchPosition.y / 100), this.beam.position));
    }
}