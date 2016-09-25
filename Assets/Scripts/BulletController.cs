using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Moving target;

    private void Update()
    {
        this.transform.Translate(new Vector2(-1, 0) * 20 * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(this);
        }
        else if (other.tag == "Korey" && Moving.koreyState == State.Alive)
        {
            this.target.RecieveDamage(GameData.DamageByBullet);
            Destroy(this.gameObject);
        }
    }
}