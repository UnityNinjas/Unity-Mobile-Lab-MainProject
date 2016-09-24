using UnityEngine;

public class ResizeBackground : MonoBehaviour
{
    public void OnValidate()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        this.transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        this.transform.localScale = new Vector2(worldScreenWidth / width, worldScreenHeight / height);
    }
}
