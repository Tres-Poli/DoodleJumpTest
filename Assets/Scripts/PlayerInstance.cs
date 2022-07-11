using UnityEngine;

public sealed class PlayerInstance : MonoBehaviour
{
    [SerializeField] GameEntityType _type;
    public GameEntityType Type => _type;

    private SpriteRenderer _renderer;

    public SpriteRenderer Renderer => _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
}
