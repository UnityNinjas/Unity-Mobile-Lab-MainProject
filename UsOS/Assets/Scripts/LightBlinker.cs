using UnityEngine;

public class LightBlinker : MonoBehaviour
{
    public Light lighter;
    public float randomTime;

    void Start()
    {
        this.randomTime = Random.Range(0f, 4f);
    }

    void Update()
    {
        this.lighter.spotAngle = 115;
        this.randomTime -= Time.deltaTime;

        if (this.randomTime <= 0)
        {
            this.lighter.spotAngle =70f;
            this.randomTime = Random.Range(0f, 4f);
        }
    }
}
