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
    private const string HideSpotTag = "HideSpot";
    private uint sortingOrderOriginal = 1000;
    private uint sortingOrderHide = 0;

    private readonly Dictionary<string, DamageTag> damageTagsMap = new Dictionary<string, DamageTag>
    {
        {"Laser", DamageTag.Laser},
        {"Enemy", DamageTag.Enemy},
        {"Bullet", DamageTag.Bullet},
        {"Explosion", DamageTag.Explosion}
    };

    [HideInInspector]
    public float restoreHealthTimer;
    public bool canHide;
    public static bool isHiding;
    public GameObject kickDetector;
    public GameObject punchDetector;
    public GameObject AirPunchDetector;

    internal static State koreyState;

    //Animation hashes
    private readonly int DamageHash = Animator.StringToHash("Damage");
    private readonly int deadHash = Animator.StringToHash("Dead");
    //private readonly int idle = Animator.StringToHash("Idle");
    private readonly int jump = Animator.StringToHash("Jump");

    private float horizontal;
    private Animator animator;
    private bool isSprintig;
    private bool isJumping;
    private bool endOfAnimation;
    private bool lowerKick;
    private bool isGrounded = true;
    private Rigidbody2D rigidbodyPlayer;
    private bool isMoving;
    private SpriteRenderer sprite;
    private bool isHurt;

    private Rigidbody2D RigidbodyPlayer
    {
        get
        {
            if (this.rigidbodyPlayer == null)
            {
                this.RigidbodyPlayer = GetComponent<Rigidbody2D>();
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
        this.sprite = this.GetComponent<SpriteRenderer>();
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

        //When player is hurt will stop AddForce from controllers
        // When player finish his health pennalty will restore 4 health per update 
        if (this.restoreHealthTimer <= 0)
        {
            UpdateHealth(4);
            this.restoreHealthTimer = GameData.DefaultTime;
        }
        else if (this.restoreHealthTimer > 0)
        {
            this.restoreHealthTimer -= Time.deltaTime;

            if (this.isHurt && this.restoreHealthTimer > 9.4f)
            {
                return;
            }

            this.isHurt = false;
        }

        //just like usual Input.GetAxis("Horizontal");
        this.horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

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

        if (!this.damageTagsMap.ContainsKey(tagAsString))
        {
            return;
        }

        tagCollider = this.damageTagsMap[tagAsString];
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == HideSpotTag)
        {
            this.canHide = true;
            Hud.instance.SwitchHideButton(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == HideSpotTag)
        {
            this.canHide = false;
            Hud.instance.SwitchHideButton(false);
            UnHide();
        }
    }

    private void MovingCharacter()
    {
        if (this.horizontal != 0f && !this.isHurt)
        {
            this.RigidbodyPlayer.velocity = new Vector2(this.horizontal * GameData.sprintSpeed, this.rigidbodyPlayer.velocity.y);
            if (this.horizontal < 0)
            {
                this.sprite.flipX = true;
            }
            else
            {
                this.sprite.flipX = false;
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

        if (this.isGrounded && !this.isSprintig && !this.isJumping && koreyState == State.Alive && !this.lowerKick)
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
            this.isJumping = true;
            this.RigidbodyPlayer.AddForce(new Vector2(0f, 300f));
            this.animator.SetTrigger(this.jump);
        }
    }

    public void RecieveDamage(int value)
    {
        this.restoreHealthTimer = GameData.HitTime;
        this.animator.SetTrigger(this.DamageHash);
        Vector2 kickBackImpulse = new Vector2(-2f, 1f);
        this.RigidbodyPlayer.velocity = Vector2.zero;
        this.horizontal = 0;
        this.isHurt = true;
        this.RigidbodyPlayer.AddForce(kickBackImpulse, ForceMode2D.Impulse);
        UpdateHealth(value);
    }

    public void UpdateHealth(int value)
    {
        if (GameData.Health + value <= 0)
        {
            koreyState = State.Dead;
            this.RigidbodyPlayer.AddForce(Vector2.left, ForceMode2D.Impulse);

            Hud.instance.mobileControls.gameObject.SetActive(false);
            this.animator.SetTrigger(this.deadHash);
            Time.timeScale = 0.5f;
            GameData.Health = 0;
            return;
        }

        GameData.Health += value;

        Hud.instance.UpdateHealth();
    }

    public void Hide()
    {
        if (this.canHide)
        {
            isHiding = true;
            GetComponent<SpriteRenderer>().sortingOrder = (int)this.sortingOrderHide;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        }
    }

    public void UnHide()
    {
        isHiding = false;
        GetComponent<SpriteRenderer>().sortingOrder = (int)this.sortingOrderOriginal;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    //Attached to Korey animator/ animation: "Dead"
    public void OnDeadFinish()
    {
        Debug.Log("Fuck");
        Hud.instance.ActivateTryAgainPanel();
    }

    public void ActivateKickDetector()
    {
        this.kickDetector.SetActive(true);
    }

    public void DeactivateKickDetector()
    {
        this.kickDetector.SetActive(false);
    }

    public void ActivatePunchkDetector()
    {
        this.punchDetector.SetActive(true);
    }

    public void DeactivatePunchDetector()
    {
        this.punchDetector.SetActive(false);
    }

    public void ActivateAirPunchkDetector()
    {
        this.AirPunchDetector.SetActive(true);
    }

    public void DeactivateAirPunchDetector()
    {
        this.AirPunchDetector.SetActive(false);
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
}