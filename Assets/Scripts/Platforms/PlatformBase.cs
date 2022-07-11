using UnityEngine;

public abstract class PlatformBase : MonoBehaviour, IGameEntity
{
    [SerializeField] GameEntityType _type;
    public GameEntityType Type => _type;

    public SpriteRenderer Renderer { get; private set; }
    public EdgeCollider2D Collider { get; private set; }

    protected void InitBase()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<EdgeCollider2D>();
    }

    public virtual void SetSize(float width, float height)
    {
        var newSize = new Vector2(width, height);
        Renderer.size = newSize;
        Collider.offset = new Vector2(0, height / 2);
        Collider.points = new Vector2[]
        {
            new Vector2(-width / 2, 0),
            new Vector2(width / 2, 0)
        };
    }

    public virtual void SetPosition(Vector3 value)
    {
        transform.position = value;
    }
}
