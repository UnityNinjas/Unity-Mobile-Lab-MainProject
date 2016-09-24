using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    public UnityEvent OnClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.OnClick.Invoke();
    }
}
