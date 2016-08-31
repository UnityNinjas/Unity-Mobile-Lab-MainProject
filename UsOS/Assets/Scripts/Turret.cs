using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform detector;
    public string targetTag;

    private bool detectTarget;
    private Transform target;

    private void Update()
    {
        this.detector.LookAt(this.target);

        //if (!this.detectTarget)
        //{
        //    this.detector.Rotate(new Vector2(0, 0.7f), Space.Self);
        //}
        //else
        //{
        //    this.detector.LookAt(this.target);
        //}
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == this.targetTag)
        {
            this.target = other.transform;
            this.detectTarget = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        this.detectTarget = false;
    }
}
