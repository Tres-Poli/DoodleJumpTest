using UnityEngine;

public class Platform : PlatformBase
{
    [SerializeField] float _onCollisionImpulse;

    protected virtual void Awake()
    {
        InitBase();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.OnPlatformCollision(_onCollisionImpulse);
        } 
    }
}
