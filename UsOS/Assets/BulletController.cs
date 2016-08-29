using UnityEngine;

public class BulletController : MonoBehaviour
{

    public Moving kroey;
    void LateUpdate()
    {
        this.transform.Translate(Vector3.left * Time.deltaTime);
    }


    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Walls2")
        {
            Destroy(this);
        }
        else if (collider.tag == "Korey" && Moving.koreyState == State.Alive)
        {
            kroey.TakeDamageTest(GameData.DamageByBullet);
            Destroy(this.gameObject);
        }
    }
}
