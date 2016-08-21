using UnityEngine;

public class LightBlinker : MonoBehaviour
{
    public Light lighter;
    public float minSeconds = 0.01f;
    public float maxSeconds = 4f;

    private float randomTime;

    void Start()
    {
        this.randomTime = Random.Range(this.maxSeconds, this.maxSeconds);
    }

    void Update()
    {
        this.lighter.spotAngle = 115;
        this.randomTime -= Time.deltaTime;

        if (this.randomTime <= 0)
        {
            this.lighter.spotAngle = 70f;
            this.randomTime = Random.Range(this.maxSeconds, this.maxSeconds);
        }
    }
}
