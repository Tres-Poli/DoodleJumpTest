using System.Collections;
using UnityEngine;

sealed class MovingPlatform : PlatformBase, IMovingPlatform
{
    [SerializeField] float _movingAmplitude;
    [SerializeField] float _velocityFactor;
    [SerializeField] float _onCollisionImpulse;
    [SerializeField] Color _horMovement;
    [SerializeField] Color _vertMovement;

    private MovingPlatformDirection _direction;
    private Vector3 _prevPosition;
    private Vector3 _nextPosition;
    private float _prevTime;

    private void Awake()
    {
        InitBase();

        _prevTime = Time.time;
        _direction = Random.value < 0.5f ? MovingPlatformDirection.Horizontally : MovingPlatformDirection.Vertically;
        Renderer.color = _direction == MovingPlatformDirection.Horizontally ? _horMovement : _vertMovement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.OnPlatformCollision(_onCollisionImpulse);
        }
    }

    public void Move()
    {
        var currTime = (Time.time - _prevTime) * _velocityFactor;
        transform.position = Vector3.Lerp(_prevPosition, _nextPosition, currTime);

        if (currTime >= 1f)
        {
            _nextPosition = _prevPosition;
            _prevPosition = transform.position;
            _prevTime = Time.time;
        }
    }

    public override void SetPosition(Vector3 value)
    {
        base.SetPosition(value);
        _prevPosition = transform.position;

        var offset = _direction == MovingPlatformDirection.Horizontally ? Vector3.right : Vector3.up * _movingAmplitude;
        offset *= Random.value < 0.5f ? -1 : 1;
        _nextPosition = _prevPosition + offset;
    }
}

public enum MovingPlatformDirection
{
    Horizontally = 0,
    Vertically = 1,
}

