using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Moving : MonoBehaviour
{
    public int sprintingSpeed = 5;

    private float horizontal;
    private Animator animator;
    private bool isSprintig;

    public void Awake()
    {
        this.animator = this.GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        MovingCharacter();

    }

    private void MovingCharacter()
    {
        if (horizontal <= 0.5 && this.horizontal > 0)
        {
            this.animator.SetTrigger("Walking");
            this.transform.Translate(Vector3.right * Time.deltaTime);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }
        else if (this.horizontal > 0.5)
        {
            this.isSprintig = true;
            this.animator.SetTrigger("Sprint");
            this.transform.Translate(Vector3.right * sprintingSpeed * Time.deltaTime);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }
        else if (this.horizontal >= -0.5 && this.horizontal < 0)
        {
            this.animator.SetTrigger("Walking");
            this.transform.Translate(Vector3.left * Time.deltaTime);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        }
        else if (this.horizontal < -0.5)
        {
            this.animator.SetTrigger("Sprint");
            this.transform.Translate(Vector3.left * sprintingSpeed * Time.deltaTime);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        }
        else
        {
            this.animator.SetTrigger("Idle");
        }
    }
}
