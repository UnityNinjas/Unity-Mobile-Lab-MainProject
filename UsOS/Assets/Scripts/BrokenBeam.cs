using UnityEngine;

public class BrokenBeam : MonoBehaviour
{
    public MonoBehaviour component;
    public float minSeconds = 0.01f;
    public float maxSeconds = 2f;

    private float randomTime;
    private bool isOn;

    void Start()
    {
        this.randomTime = Random.Range(this.minSeconds, this.maxSeconds);
    }

    void Update()
    {
        this.randomTime -= Time.deltaTime;

        if (this.randomTime <= 0)
        {
            this.isOn = !this.isOn;
            this.component.enabled = this.isOn;
            this.randomTime = Random.Range(this.minSeconds, this.maxSeconds);
        }
    }
}
