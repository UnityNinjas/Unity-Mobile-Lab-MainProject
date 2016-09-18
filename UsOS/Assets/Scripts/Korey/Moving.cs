using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
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
    private readonly int isGroundedHash = Animator.StringToHash("Jump");
    private readonly int sprintHash = Animator.StringToHash("Sprint");
    private readonly int idleHash = Animator.StringToHash("Idle");
    private readonly int walkingHash = Animator.StringToHash("Walking");

    private float horizontal;
    private Animator animator;
    private bool isGrounded = true;
    private Rigidbody2D rigidbodyPlayer;
    private SpriteRenderer sprite;
    private bool isHurt;
    private bool direction;

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
        this.sprite = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        koreyState = State.Alive;
        Time.timeScale = 1f;
        GameData.Health = 100;
    }

    //With directives you can describe to builder or compiler when to use code. 
    //In that case will use only in editor and standalone build.
    //When you build on Android for example that Update didn't exist there.
#if UNITY_EDITOR || UNITY_STANDALONE

    public void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            Kick();
        }
    }

#endif

    private void FixedUpdate()
    {
        if (koreyState == State.Dead)
        {
            return;
        }

        // When player is hurt will stop AddForce from controllers
        // When player finish his health pennalty will restore 4 health per update 
        if (this.restoreHealthTimer <= 0)
        {
            UpdateHealth(4);
            this.restoreHealthTimer = GameData.DefaultTime;
        }
        else if (this.restoreHealthTimer > 0)
        {
            this.restoreHealthTimer -= Time.fixedDeltaTime;

            if (this.isHurt && this.restoreHealthTimer > 9.4f)
            {
                return;
            }

            this.isHurt = false;
        }

        //just like usual Input.GetAxis("Horizontal"); but from thumb in UI
        this.horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

        if (!this.isHurt)
        {
            this.RigidbodyPlayer.velocity = new Vector2(
                this.horizontal * GameData.sprintSpeed,
                this.rigidbodyPlayer.velocity.y);

            if (this.horizontal == 0f)
            {
                this.sprite.flipX = this.direction;
            }
            else
            {
                this.direction = this.horizontal < 0;
                this.sprite.flipX = this.direction;
            }
        }

        if (this.isGrounded)
        {
            float absValue = Mathf.Abs(this.horizontal);
            this.animator.SetFloat(this.idleHash, absValue);
            this.animator.SetFloat(this.walkingHash, absValue);
            this.animator.SetFloat(this.sprintHash, absValue);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "FloorCollider")
        {
            this.isGrounded = true;
        }

        if (this.damageTagsMap.ContainsKey(other.collider.tag))
        {
            switch (this.damageTagsMap[other.collider.tag])
            {
                case DamageTag.Laser:
                    RecieveDamage(GameData.DamageByLaser);
                    break;
                case DamageTag.Enemy:
                    RecieveDamage(GameData.DamageByKick);
                    break;
                case DamageTag.Bullet:
                    RecieveDamage(GameData.DamageByBullet);
                    break;
                case DamageTag.Explosion:
                    RecieveDamage(GameData.DamageByExplosion);
                    break;
            }
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

    public void EndOfLowerKick()
    {

    }

    public void Punch()
    {
        if (!this.isGrounded)
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
        if (this.isGrounded)
        {
            this.isGrounded = false;
            this.RigidbodyPlayer.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);

            this.animator.SetFloat(this.idleHash, 2);
            this.animator.SetFloat(this.sprintHash, 2);
            this.animator.SetFloat(this.walkingHash, 2);
            this.animator.SetTrigger(this.isGroundedHash);
        }
    }

    public void RecieveDamage(int value)
    {
        this.restoreHealthTimer = GameData.HitTime;
        this.animator.SetTrigger(this.DamageHash);
        Vector2 kickBackImpulse = new Vector2(-0.5f, 1f);
        this.isHurt = true;
        this.RigidbodyPlayer.AddForce(kickBackImpulse, ForceMode2D.Impulse);
        UpdateHealth(-value);
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
        if (!this.isGrounded)
        {
            this.animator.SetTrigger("AirPunch");
        }
        else
        {
            this.animator.SetTrigger("NormalKick");
        }
    }
}