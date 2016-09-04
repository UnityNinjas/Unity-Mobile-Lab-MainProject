using UnityEngine;
using Object = UnityEngine.Object;

public class WalkingGurad : MonoBehaviour
{
    private Transform currenTransform;
    private float maxPosition;
    // private bool isFocused;
    private int speed = 2;
    private Animator animator;
    private SpriteRenderer sprite;
    private float timeStay = 1;
    private int walking;
    private int shoot;
    private bool isDead;
    private float shootingCounter = 1;
    public GameObject bullet;
    public Transform shootPoint;
    public CollisionManager CollisionManager;
    public HitDetector HitDetector;
    public HitDetector PubchDetector;
    public HitDetector AirPunchDetector;

    private void Start()
    {
        this.currenTransform = this.transform;
        this.sprite = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.maxPosition = this.currenTransform.localPosition.x - 4.6f;
        this.shoot = Animator.StringToHash("Shoot");
        this.walking = Animator.StringToHash("Walking");
        this.CollisionManager.KoreyTrigger = StartShooting;
    }

    public void Dead(bool isTakingHit)
    {
        if (isTakingHit)
        {
            this.animator.SetTrigger("Die");
            this.isDead = true;
        }
    }

    private void StartShooting(bool koreyTrigger)
    {
        if (koreyTrigger)
        {
            this.speed = 0;
            this.animator.SetTrigger(this.shoot);
            this.bullet.SetActive(true);
        }
        else
        {
            this.animator.SetTrigger(this.walking);
            this.speed = 2;
        }

    }

    public void Shooting()
    {
        if (Moving.koreyState == State.Alive)
        {
            Instantiate(this.bullet, this.shootPoint.position, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        MovingGuard();
        ClampPosition();
    }

    private void MovingGuard()
    {
        if (!this.isDead)
        {
            this.currenTransform.Translate(Vector3.left * this.speed * Time.deltaTime);
        }
    }

    private void ClampPosition()
    {
        if (this.currenTransform.localPosition.x <= -this.maxPosition)
        {
            if (this.timeStay <= 0)
            {
                this.transform.rotation = new Quaternion(0, 180, 0, 0);
                this.animator.SetTrigger("Walking");
                this.timeStay = 1;
                this.speed = 2;
            }
            else
            {
                this.timeStay -= Time.deltaTime;
                this.speed = 0;
            }

        }


        if (this.currenTransform.localPosition.x >= this.maxPosition)
        {
            if (this.timeStay <= 0)
            {
                this.transform.rotation = new Quaternion(0, 0, 0, 0);
                this.animator.SetTrigger("Walking");
                this.timeStay = 1;
                this.speed = 2;
            }
            else
            {
                this.timeStay -= Time.deltaTime;
                this.speed = 0;
            }
        }

        this.animator.SetFloat("Idle", this.timeStay);
    }
}