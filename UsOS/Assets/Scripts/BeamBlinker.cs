using UnityEngine;

public class BeamBlinker : MonoBehaviour
{
    public GameObject beam;
    public float minSeconds = 0.01f;
    public float maxSeconds = 4f;

    private float randomTime;

    void Start()
    {
        this.randomTime = Random.Range(this.minSeconds, this.maxSeconds);
    }

    void Update()
    {
        this.randomTime -= Time.deltaTime;

        if (this.randomTime <= 0)
        {
            this.beam.SetActive(!this.beam.activeSelf);
            this.randomTime = Random.Range(this.maxSeconds, this.maxSeconds);
        }
    }
}