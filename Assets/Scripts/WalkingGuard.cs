using UnityEngine;

public class WalkingGuard : MonoBehaviour
{
    private readonly int ShootHash = Animator.StringToHash("Shoot");
    private readonly int WalkingHash = Animator.StringToHash("Walking");
    private readonly int IdleHash = Animator.StringToHash("Idle");

    public GameObject bullet;
    public Transform shootPoint;
    public CollisionManager CollisionManager;

    private Transform currenTransform;
    private float minPosition;
    private float maxPosition;
    private int speed = 1;
    private Animator animator;
    private float timeStay = 1;
    private bool isDead;
    private float shootingCounter = 1;
    private float moving = -1f;
    private bool isShooting;

    private void Start()
    {
        this.timeStay = 2f;
        this.currenTransform = this.transform;
        this.animator = this.GetComponent<Animator>();
        this.minPosition = this.currenTransform.position.x - 3f;
        this.maxPosition = this.currenTransform.position.x + 6f;
        this.CollisionManager.koreyTrigger = StartShooting;
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
        this.isShooting = koreyTrigger;
        if (koreyTrigger)
        {
            this.speed = 0;
            this.bullet.SetActive(true);
            this.animator.SetTrigger(this.ShootHash);
        }
    }

    //Attached to animation "Shoot"
    public void Shooting()
    {
        if (Moving.koreyState == State.Alive)
        {
            Instantiate(this.bullet, this.shootPoint.position, this.shootPoint.rotation);
            SoundManager.instance.FxPlayOnce(Clip.GunShot);
        }
    }

    public void OnShootAnimaionFinish()
    {
        if (!this.isShooting)
        {
            this.animator.Play(this.WalkingHash);
        }
        else
        {
            this.animator.Play(this.ShootHash);
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
             this.currenTransform.position.x > this.maxPosition)
        {
            if (this.transform.localRotation.y == 180)
            {
                this.transform.Rotate(Vector2.zero);
            }
            else
            {
                this.transform.Rotate(0, 180, 0);
            }

            this.timeStay = 2f;
            //Prevent stoping guard on one position. 
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

        if (!this.isShooting && this.speed > 0f)
        {
            this.currenTransform.Translate(new Vector2(this.moving * this.speed * Time.deltaTime, 0f));
            this.animator.SetFloat(this.IdleHash, this.speed);
            this.animator.SetFloat(this.WalkingHash, this.speed);
        }
    }
}