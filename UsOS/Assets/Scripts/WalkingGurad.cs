using UnityEngine;

public class WalkingGurad : MonoBehaviour
{
    private RectTransform currenTransform;
    private float maxPosition;
    private bool isFocused;
    private int speed = 2;
    private Animator animator;
    private SpriteRenderer sprite;

    void Start()
    {
        this.currenTransform = GetComponent<RectTransform>();

        this.sprite = this.GetComponent<SpriteRenderer>();

        this.animator = this.GetComponent<Animator>();

        this.maxPosition = this.currenTransform.localPosition.x;
    }

    void FixedUpdate()
    {

        MovingGuard();
        ClampPosition();
    }

    private void MovingGuard()
    {
        if (!isFocused)
        {
            this.currenTransform.Translate(Vector3.right * this.speed * Time.deltaTime);

        }
    }


    public void Shoot()
    {
        this.animator.SetTrigger("Shoot");
    }

    private void ClampPosition()
    {
        Debug.Log(this.maxPosition);
        if (!isFocused)
        {
            this.animator.SetTrigger("Walking");

            if (currenTransform.localPosition.x < -this.maxPosition)
            {
                this.speed *= -1;
                this.sprite.flipX = true;
            }

            if (currenTransform.localPosition.x > this.maxPosition)
            {
                this.speed *= -1;
                this.sprite.flipX = false;
            }

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Korey")
        {
            this.animator.SetTrigger("Idle");
            this.isFocused = true;
            this.sprite.flipX = false;

        }
    }



    public void OnTriggerExit2D(Collider2D go)
    {
        if (go.gameObject.tag == "Korey")
        {
            this.isFocused = false;
        }
    }

}