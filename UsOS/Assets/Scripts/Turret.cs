using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform detector;
    public Transform turret;
    public Transform bulletPivot;
    public GameObject bulletPrefab;

    public string targetTag;

    private float timer;
    private bool detectTarget;
    private Transform target;

    private void Update()
    {
        if (!this.detectTarget)
        {
            this.detector.Rotate(new Vector2(0, 0.7f), Space.Self);
            this.timer = 4f;
        }
        else
        {
            this.timer -= Time.deltaTime;
            if (this.timer <= 0)
            {
                this.timer = 1f;
                Object bullet = Instantiate(this.bulletPrefab, this.bulletPivot, false);
                (bullet as GameObject).SetActive(true);
            }

            this.detector.LookAt(this.target);
            this.turret.localRotation = Quaternion.Lerp(this.turret.transform.localRotation, this.target.localRotation, 1f * Time.deltaTime);
        }
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
