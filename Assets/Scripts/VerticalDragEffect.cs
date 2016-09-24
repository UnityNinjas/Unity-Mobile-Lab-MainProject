using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class VerticalDragEffect : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public enum Position
    {
        Up, Down
    }

    public int minPosition;
    public int maxPosition;
    public UnityEvent onFinish;
    public Position transformPosition;
    public GameObject shader;

    private RectTransform objectToDrag;
    private float lastDelta;

    public RectTransform ObjectToDrag
    {
        get
        {
            if (this.objectToDrag == null)
            {
                this.objectToDrag = this.GetComponent<RectTransform>();
            }

            return this.objectToDrag;
        }
    }

    public void DeactivateEsc()
    {
        this.shader.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Translate(eventData.delta.y * 1.3f);
        this.lastDelta = eventData.delta.y;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(FinishToTheEnd());
    }

    public void GoUp(float power = 0.5f)
    {
        this.gameObject.SetActive(true);
        this.lastDelta = power;
        this.ObjectToDrag.localPosition += new Vector3(0f, 0.1f, 0f);
        StartCoroutine(FinishToTheEnd());
    }

    public void GoDown(float power = 0.5f)
    {
        this.lastDelta = -power;
        this.ObjectToDrag.localPosition -= new Vector3(0f, 0.1f, 0f);
        if (this.isActiveAndEnabled)
        {
            StartCoroutine(FinishToTheEnd());
        }
    }

    private IEnumerator FinishToTheEnd()
    {
        while (this.minPosition < this.ObjectToDrag.localPosition.y && this.ObjectToDrag.localPosition.y < this.maxPosition)
        {
            Translate(Mathf.Sign(this.lastDelta) >= 0 ? this.lastDelta += 2 : this.lastDelta -= 2);

            yield return new WaitForEndOfFrame();
        }

        if (this.minPosition >= this.ObjectToDrag.localPosition.y)
        {
            this.gameObject.SetActive(false);
            this.transformPosition = Position.Down;
        }

        if (this.ObjectToDrag.localPosition.y >= this.maxPosition)
        {
            this.transformPosition = Position.Up;
        }

        this.onFinish.Invoke();
        this.lastDelta = 0;
    }

    private void Translate(float deltaY)
    {
        float y = Mathf.Clamp(this.ObjectToDrag.localPosition.y + deltaY, this.minPosition, this.maxPosition);
        this.ObjectToDrag.localPosition = new Vector2(this.ObjectToDrag.localPosition.x, y);
    }
}