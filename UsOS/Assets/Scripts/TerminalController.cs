using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerminalController : MonoBehaviour, IPointerDownHandler
{
    public GameObject[] objectControls;
    public Transform ui;
    public ParticleSystem hackEffect;
    private bool isEnabled;

    public void SwitchUIVisibility()
    {
        if (ui.localScale.x <= 0)
        {
            StartCoroutine(ScaleUp());
        }
        else if (ui.localScale.x >= 1)
        {
            StartCoroutine(ScaleDown());
        }
    }

    private IEnumerator ScaleDown()
    {
        while (this.ui.localScale.x > 0)
        {
            this.ui.localScale -= new Vector3(0.1f, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator ScaleUp()
    {
        while (this.ui.localScale.x < 1)
        {
            this.ui.localScale += new Vector3(0.1f, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SwitchObjects()
    {
        if (this.isEnabled)
        {
            EnableObjects();
        }
        else
        {
            DesableObjects();
        }
    }

    public void EnableObjects()
    {
        this.isEnabled = true;
        this.hackEffect.Play();
        EtarateAll();
    }

    public void DesableObjects()
    {
        this.isEnabled = false;
        this.hackEffect.Play();
        EtarateAll();
    }

    private void EtarateAll()
    {
        for (int i = 0; i < this.objectControls.Length; i++)
        {
            this.objectControls[i].SetActive(this.isEnabled);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SwitchUIVisibility();
    }
}
