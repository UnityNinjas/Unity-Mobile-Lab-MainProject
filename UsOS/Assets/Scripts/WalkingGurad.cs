using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class WalkingGurad : MonoBehaviour
{
    private RectTransform currenTransform;
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
        this.currenTransform = GetComponent<RectTransform>();
        this.sprite = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.maxPosition = this.currenTransform.anchoredPosition.x + 2;
        shoot = Animator.StringToHash("Shoot");
        walking = Animator.StringToHash("Walking");
        CollisionManager.KoreyTrigger = StartShooting;
    }

    public void Dead(bool isTakingHit)
    {
        if (isTakingHit)
        {
            this.animator.SetTrigger("Die");
            isDead = true;
        }
    }

    private void StartShooting(bool koreyTrigger)
    {
        if (koreyTrigger)
        {
            this.speed = 0;
            this.animator.SetTrigger(shoot);
            bullet.SetActive(true);
        }
        else
        {
            this.animator.SetTrigger(walking);
            speed = 2;
        }

    }

    public void Shooting()
    {
        if (Moving.koreyState == State.Alive)
        {
            Instantiate(bullet, shootPoint.position, Quaternion.identity);
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
        if (this.currenTransform.anchoredPosition.x <= -this.maxPosition)
        {
            if (timeStay <= 0)
            {
                this.transform.rotation = new Quaternion(0, 180, 0, 0);
                this.animator.SetTrigger("Walking");
                timeStay = 1;
                speed = 2;
            }
            else
            {
                timeStay -= Time.deltaTime;
                speed = 0;
            }

        }


        if (this.currenTransform.anchoredPosition.x >= this.maxPosition)
        {
            if (timeStay <= 0)
            {
                this.transform.rotation = new Quaternion(0, 0, 0, 0);
                this.animator.SetTrigger("Walking");
                timeStay = 1;
                speed = 2;
            }
            else
            {
                timeStay -= Time.deltaTime;
                speed = 0;
            }
        }

        this.animator.SetFloat("Idle", timeStay);
    }
}

