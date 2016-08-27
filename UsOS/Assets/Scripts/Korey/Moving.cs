using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

internal enum DamageTag
{
    Laser, Enemy, Bullet, Explosion
}

internal enum State
{
    Alive, Dead
}

public class Moving : MonoBehaviour
{
    private readonly Dictionary<string, DamageTag> damagetagsMap = new Dictionary<string, DamageTag>
    {
        {"Laser", DamageTag.Laser},
        {"Enemy", DamageTag.Enemy},
        {"Bullet", DamageTag.Bullet},
        {"Explosion", DamageTag.Explosion}
    };

    //Animation hashes
    private readonly int DamageHash = Animator.StringToHash("Damage");
    private readonly int deadHash = Animator.StringToHash("Dead");
    private readonly int idle = Animator.StringToHash("Idle");
    private readonly int jump = Animator.StringToHash("Jump");

    [HideInInspector]
    public float restoreHealthTimer;

    internal static State koreyState;
    private float horizontal;
    private Animator animator;
    private bool isSprintig;
    private bool isJumping;
    private bool endOfAnimation;
    private bool lowerKick;
    private bool isGrounded = true;
    private Rigidbody2D rigidbodyPlayer;

    private Rigidbody2D RigidbodyPlayer
    {
        get
        {
            if (this.rigidbodyPlayer == null)
            {
                this.RigidbodyPlayer = this.GetComponent<Rigidbody2D>();
            }

            return this.rigidbodyPlayer;
        }

        set
        {
            this.rigidbodyPlayer = value;
        }
    }

    public void Awake()
    {
        this.animator = GetComponent<Animator>();

    }

    public void Start()
    {
        koreyState = State.Alive;
        Time.timeScale = 1f;
        GameData.Health = 100;
    }

    private void Update()
    {
        if (koreyState == State.Dead)
        {
            return;
        }

        this.restoreHealthTimer -= Time.deltaTime;
        if (this.restoreHealthTimer <= 0)
        {
            UpdateHealth(4);
            this.restoreHealthTimer = GameData.DefaultTime;
        }

        this.horizontal = CrossPlatformInputManager.GetAxis("Horizontal");//Input.GetAxis("Horizontal");

        MovingCharacter();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tagAsString;
        DamageTag tagCollider;

        tagAsString = collision.collider.tag;
        if (tagAsString == "FloorCollider")
        {
            this.isJumping = false;
        }

        if (!this.damagetagsMap.ContainsKey(tagAsString))
        {
            return;
        }

        tagCollider = this.damagetagsMap[tagAsString];
        switch (tagCollider)
        {
            case DamageTag.Laser:
                RecieveDamage(-GameData.DamageByLaser);
                break;
            case DamageTag.Enemy:
                RecieveDamage(-GameData.DamageByKick);
                break;
            case DamageTag.Bullet:
                RecieveDamage(-GameData.DamageByBullet);
                break;
            case DamageTag.Explosion:
                RecieveDamage(-GameData.DamageByExplosion);
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("FloorCollider"))
        {
            this.isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("FloorCollider"))
        {
            this.isGrounded = true;
        }
    }

    private bool isMoving;
    private void MovingCharacter()
    {
        Vector3 movement = new Vector3(this.horizontal, 0f, 0f);

        if (this.horizontal != 0)
        {
            if (this.horizontal <= 0.5f && this.horizontal > 0f)
            {

                this.RigidbodyPlayer.velocity = movement * 2;

                //  this.transform.Translate(Vector3.right * Time.deltaTime);
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (this.horizontal > 0.5f)
            {
                this.RigidbodyPlayer.velocity = movement * GameData.sprintSpeed;

                //TODO Moving with transform
                // this.transform.Translate(Vector3.left * sprintingSpeed * Time.deltaTime);
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

            }
            else if (this.horizontal >= -0.5f && this.horizontal < 0)
            {
                this.RigidbodyPlayer.velocity = movement * 2;
                //TODO Moving with transform
                //this.transform.Translate(Vector3.left * Time.deltaTime);
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;


            }
            else if (this.horizontal < -0.5f)
            {
                this.RigidbodyPlayer.velocity = movement * GameData.sprintSpeed;

                //TODO Moving with transform
                // this.transform.Translate(Vector3.left * sprintingSpeed * Time.deltaTime);
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

            this.isSprintig = true;

        }
        else
        {

            this.isSprintig = false;
        }

        this.animator.SetFloat("Sprint", Mathf.Abs(this.horizontal));
        this.animator.SetFloat("Walking", Mathf.Abs(this.horizontal));
        this.animator.SetBool("Idle", this.isSprintig);
        this.animator.SetBool("JumpToIdle", this.isGrounded);

        if (isGrounded && !this.isSprintig && !this.isJumping && !this.isMoving)
        {
            this.RigidbodyPlayer.velocity = Vector2.zero;
        }


    }

    public void EndOfLowerKick()
    {
        this.lowerKick = false;
        this.endOfAnimation = true;
    }

    public void Punch()
    {
        if (this.isJumping)
        {
            this.animator.SetTrigger("AirPunch");
        }
        else
        {
            this.animator.SetTrigger("Punch");
        }
    }

    public void Jump()
    {
        if (!this.isJumping)
        {
            this.animator.SetTrigger(this.jump);
            this.isJumping = true;
            this.RigidbodyPlayer.AddForce(Vector3.up * 5f, ForceMode2D.Impulse);
        }
    }

    private void RecieveDamage(int value)
    {
        this.restoreHealthTimer = GameData.HitTime;
        this.animator.SetTrigger(this.DamageHash);
        Vector2 kickBackImpulse = new Vector2(-2f, 1f);
        this.RigidbodyPlayer.velocity = Vector2.zero;
        this.horizontal = 0;
        this.RigidbodyPlayer.AddForce(kickBackImpulse, ForceMode2D.Impulse);
        UpdateHealth(value);
    }

    public void UpdateHealth(int value)
    {
        if (GameData.Health + value <= 0)
        {
            koreyState = State.Dead;
            Hud.instance.mobileControls.gameObject.SetActive(false);
            this.animator.SetTrigger(this.deadHash);
            Time.timeScale = 0.5f;
            GameData.Health = 0;
            return;
        }

        GameData.Health += value;
        Hud.instance.UpdateHealth();
    }

    //Attached to Korey animator/ animation: "Dead"
    public void OnDeadFinish()
    {
        Hud.instance.ActivateTryAgainPanel();
    }

    public void Kick()
    {
        if (this.isSprintig)
        {
            this.animator.SetTrigger("LowerKick");
            this.lowerKick = true;
        }
        else
        {
            this.animator.SetTrigger("NormalKick");
        }
    }

    public void TakeDamageTest(int damage)
    {
        UpdateHealth(-10);
    }
}