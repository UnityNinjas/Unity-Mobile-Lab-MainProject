using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    private Vector3 targetPos;
    // Use this for initialization
    private void Start()
    {
        this.targetPos = this.transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (this.target)
        {
            Vector3 posNoZ = this.transform.position;
            posNoZ.z = this.target.transform.position.z;

            Vector3 targetDirection = (this.target.transform.position - posNoZ);

            this.interpVelocity = targetDirection.magnitude * 5f;

            this.targetPos = this.transform.position + targetDirection.normalized *this.interpVelocity * Time.deltaTime;

            this.transform.position = Vector3.Lerp(this.transform.position, this.targetPos + this.offset, 0.25f);

        }
    }
}
