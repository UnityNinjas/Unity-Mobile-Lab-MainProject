using UnityEngine;

public class WalkingGurad : MonoBehaviour
{
    private Transform currenTransform;
    private float minPosition;
    private float maxPosition;
    // private bool isFocused;
    private int speed = 1;
    private Animator animator;
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
    private float moving = -1f;

    private void Start()
    {
        this.timeStay = 2f;
        this.currenTransform = this.transform;
        this.animator = GetComponent<Animator>();
        this.minPosition = this.currenTransform.position.x - 3f;
        this.maxPosition = this.currenTransform.position.x + 3f;
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
            SoundManager.instance.FxPlayOnce(Clip.GunShot);
        }
    }

    private void FixedUpdate()
    {
        MovingGuard();
    }

    private void MovingGuard()
    {
        if (this.isDead)
        {
            return;
        }

        if (this.currenTransform.position.x < this.minPosition ||
            this.maxPosition < this.currenTransform.position.x)
        {
            this.moving = -this.moving;
            this.GetComponent<SpriteRenderer>().flipX = !this.GetComponent<SpriteRenderer>().flipX;
            this.timeStay = 2f;
            this.currenTransform.Translate(new Vector2(this.moving * this.speed * Time.deltaTime, 0f));
        }

        if (this.timeStay <= 0)
        {
            this.timeStay = 0f;
            this.speed = 1;
        }
        else
        {
            this.speed = 0;
            this.timeStay -= Time.deltaTime;
        }

        this.animator.SetFloat("Idle", this.speed);
        this.animator.SetFloat("Walking", this.speed);

        this.currenTransform.Translate(new Vector2(this.moving * this.speed * Time.deltaTime, 0f));
    }
}