using UnityEngine;
using UnityEngine.EventSystems;

public class DoorLocker : MonoBehaviour, IPointerDownHandler
{
    public Transform door;
    public Light light;

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerBind.touchPosition = eventData.position;
    }
}
