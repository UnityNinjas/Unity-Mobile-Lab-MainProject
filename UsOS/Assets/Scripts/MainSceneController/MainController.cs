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

    public Transform finishDoor;
    public Collider2D colliderOnFinishDoor;

    public void Start()
    {
        SoundManager.instance.MusicPlay(Clip.Rain);
        Object.DontDestroyOnLoad(SoundManager.instance);
    }

    public void OnDoorLockerClicked()
    {
        StartCoroutine(OpenDoor(this.door, this.colliderOnDoor));
    }

    public void OnFinishDoorLockerClicked()
    {
        StartCoroutine(OpenDoor(this.finishDoor, this.colliderOnDoor));
        this.colliderOnFinishDoor.isTrigger = true;
    }

    private IEnumerator OpenDoor(Transform door, Collider2D collider)
    {
        while (door.rotation.y > 0)
        {
            door.Rotate(new Vector3(0, -5f));
            yield return new WaitForEndOfFrame();
        }

        collider.enabled = false;
        this.lightOnDoor.color = Color.green;
    }
}