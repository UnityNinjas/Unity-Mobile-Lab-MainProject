using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Moving : MonoBehaviour
{
    public int sprintingSpeed = 1;

    private float horizontal;
    private Animator animator;
    private bool isSprintig;
    private bool isJumping;
    private bool endOfAnimation;
    private bool lowerKick;
    private Rigidbody2D rb;
    private bool isMoving = true;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
    }



    private void FixedUpdate()
    {
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

        MovingCharacter();


        //if (!this.isMoving)
        //{
        //    this.animator.SetTrigger("Idle");
        //}

        if (endOfAnimation)
        {
            this.animator.SetTrigger("Idle");
            this.endOfAnimation = false;
        }
    }

    private void MovingCharacter()
    {
        if (!this.isJumping)
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
                this.isSprintig = true;
                this.animator.SetTrigger("Sprint");
                this.transform.Translate(Vector3.left * sprintingSpeed * Time.deltaTime);
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (this.horizontal == 0)
            {
                this.animator.SetTrigger("Idle");
            }

        }

        if (this.horizontal != 0)
        {
            this.isMoving = false;
        }

        this.isSprintig = false;
    }

    public void EndOfLowerKick()
    {
        this.lowerKick = false;
        this.endOfAnimation = true;
    }

    public void Punch()
    {
        if (isJumping)
        {
            this.animator.SetTrigger("AirPunch");
        }
        else
        {
            this.animator.SetTrigger("Punch");
        }
    }

    public void OnCollisionExit2D(Collision2D hitObject)
    {
        if (hitObject.gameObject.tag == "Korey"
            && hitObject.transform.position.y < this.transform.position.y)
        {
            this.isJumping = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D hitObject)
    {
        if (hitObject.gameObject.name.Equals("Floor"))
        {
            this.isJumping = false;
        }

        this.animator.SetTrigger("Idle");

    }


    public void Jump()
    {

        this.rb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
        this.animator.SetTrigger("Jump");

    }

    public void Kick()
    {
        if (isSprintig)
        {
            this.animator.SetTrigger("LowerKick");
            lowerKick = true;
        }
        else
        {
            this.animator.SetTrigger("NormalKick");
        }
    }
}
