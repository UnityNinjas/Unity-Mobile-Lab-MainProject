using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Moving : MonoBehaviour
{
    public int sprintingSpeed = 50;

    private float horizontal;
    private Animator animator;
    private bool isSprintig;
    private bool isJumping;
    private bool endOfAnimation;
    private bool lowerKick;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();

    }



    private void FixedUpdate()
    {
        //horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        horizontal = Input.GetAxis("Horizontal");

        MovingCharacter();

    }

    private void MovingCharacter()
    {
        Vector3 movement = new Vector3(horizontal, 0.0f, 0);

        if (horizontal <= 0.5 && this.horizontal > 0)
        {
            this.animator.SetTrigger("Walking");

            this.rb.velocity = movement;

            //  this.transform.Translate(Vector3.right * Time.deltaTime);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (this.horizontal > 0.5)
        {
            this.isSprintig = true;
            this.animator.SetTrigger("Sprint");
            this.rb.velocity = movement * sprintingSpeed;

            //TODO this is moving with transform
            //    this.transform.Translate(Vector3.right * sprintingSpeed * Time.deltaTime);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }
        else if (this.horizontal >= -0.5 && this.horizontal < 0)
        {
            this.rb.velocity = movement;

            this.animator.SetTrigger("Walking");
            //TODO Moving with transform
            //this.transform.Translate(Vector3.left * Time.deltaTime);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;


        }
        else if (this.horizontal < -0.5)
        {
            this.rb.velocity = movement * sprintingSpeed;

            this.isSprintig = true;
            this.animator.SetTrigger("Sprint");

            //TODO Moving with transform
            //     this.transform.Translate(Vector3.left * sprintingSpeed * Time.deltaTime);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (!this.isSprintig && !this.isJumping && this.horizontal == 0)
        {
            this.animator.SetTrigger("Idle");
            this.rb.velocity = Vector2.down * 2;

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


    public void OnCollisionEnter2D(Collision2D hitObject)
    {
        if (hitObject.gameObject.CompareTag("Floor"))
        {
            this.isJumping = false;
        }
    }


    public void Jump()
    {

        if (!isJumping)
        {

            this.isJumping = true;
            this.rb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);

            this.animator.SetTrigger("Jump");
        }

    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }


    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
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
