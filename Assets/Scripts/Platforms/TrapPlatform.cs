using System.Collections;
using UnityEngine;

sealed class TrapPlatform : PlatformBase
{
    [SerializeField] float _breakAnimationSpeed;

    private void Awake()
    {
        InitBase();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null && pc.IsFalling)
        {
            BreakPlatform();
        }
    }

    private void BreakPlatform()
    {
        PlatformContainer.Instance.StopOverseePlatform(this);
        Collider.enabled = false;
        StartCoroutine(BreakAnimation());
    }

    private IEnumerator BreakAnimation()
    {
        var heightToWidth = Renderer.size.y / Renderer.size.x;
        while (Renderer.size.x > 0 && Renderer.size.y > 0)
        {
            Renderer.size = new Vector2(Renderer.size.x - _breakAnimationSpeed * Time.deltaTime, 
                Renderer.size.y - _breakAnimationSpeed * Time.deltaTime * heightToWidth);
            yield return new WaitForEndOfFrame();
        }

        PoolContainer.Instance.Pool(Type, this);
    }
}
