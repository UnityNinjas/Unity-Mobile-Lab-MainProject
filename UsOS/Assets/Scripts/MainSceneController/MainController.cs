using System.Collections;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public PlayerBind Korey;

    [Header("Level 2")]
    public Transform door;
    public Light lightOnDoor;
    public Collider2D colliderOnDoor;
    [Space(10)]
    [Header("Level 3")]
    public GameObject laserOnDoor;

    public void OnDoorLockerClicked()
    {
        StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        while (this.door.rotation.y > 0)
        {
            this.door.Rotate(new Vector3(0, -5f));
            yield return new WaitForEndOfFrame();
        }

        this.colliderOnDoor.enabled = false;
        this.lightOnDoor.color = Color.green;
    }
}