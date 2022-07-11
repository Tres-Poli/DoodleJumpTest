using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IMirrorable<PlayerInstance>
{
    public static PlayerController Instance { get; set; }

    [SerializeField] float _velocityFactor;

    private PlayerInstance _primaryInstance;
    private PlayerInstance _secondaryInstance;

    private Quaternion _rotationRight;
    private Quaternion _rotationLeft;

    private float _offsetYToRemove;
    private Rigidbody2D _rb;

    private ObjectMirroring<PlayerInstance> _mirror;

    private Vector3 _initialPosition;
    private int _currScores;

    public bool IsMirrored { get; set; }
    public PlayerInstance PrimaryInstance => _primaryInstance;
    public PlayerInstance SecondaryInstance => _secondaryInstance;

    public bool IsFalling => _rb.velocity.y <= 0;
    public float CurrentPositionY => PrimaryInstance.transform.position.y;

    public float Width => PrimaryInstance.Renderer.size.x;
    public float PositionX => transform.position.x;

    public event Action<bool> GameOver;

    private void Awake()
    {
        Instance = this;
        _currScores = 0;
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = false;
        _primaryInstance = GetComponentInChildren<PlayerInstance>();
    }

    private void Start()
    {
        _rotationRight = Quaternion.Euler(0, 0, 0);
        _rotationLeft = Quaternion.Euler(0, 180, 0);

        _offsetYToRemove = GameController.Instance.OffsetYFromPivotToRemove + PrimaryInstance.Renderer.size.y / 2;
        _mirror = new ObjectMirroring<PlayerInstance>(this);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var input = Input.mousePosition.x - Screen.width / 2;
            _rb.velocity = new Vector3(Mathf.Lerp(_rb.velocity.x, input * _velocityFactor, Time.deltaTime), _rb.velocity.y);
            PrimaryInstance.Renderer.transform.rotation = input < 0 ? _rotationLeft : _rotationRight;
        }
        else
        {
            _rb.velocity = new Vector3(Mathf.Lerp(_rb.velocity.x, 0f, Time.deltaTime * 10), _rb.velocity.y);
        }

        if (_secondaryInstance != null)
        {
            _secondaryInstance.Renderer.transform.rotation = PrimaryInstance.Renderer.transform.rotation;
        }

        if (PrimaryInstance.transform.position.y > GameController.Instance.LvlGoal)
        {
            GameOver?.Invoke(true);
        }

        if (GameController.Instance.GamePivot.position.y - PrimaryInstance.transform.position.y > _offsetYToRemove)
        {
            GameOver?.Invoke(false);
        }

        if ((int)PrimaryInstance.transform.position.y > _currScores)
        {
            _currScores = (int)Mathf.Clamp(PrimaryInstance.transform.position.y, 0f, GameController.Instance.LvlGoal);
            GameController.Instance.UpdateScores(_currScores);
        }

        _mirror.Update();
    }

    private void PoolSecondary()
    {
        if (_secondaryInstance != null)
        {
            PoolContainer.Instance.Pool(_secondaryInstance.Type, _secondaryInstance);
            _secondaryInstance = null;
        }

        IsMirrored = false;
    }

    public void OnPlatformCollision(float value)
    {
        if (IsFalling)
        {
            _rb.AddForce(Vector2.up * value, ForceMode2D.Impulse);
        }
    }

    public void Mirror(float positionX)
    {
        _secondaryInstance = PoolContainer.Instance.Get(GameEntityType.Player) as PlayerInstance;
        _secondaryInstance.transform.SetParent(transform);
        _secondaryInstance.transform.position = new Vector3(
            positionX,
            _primaryInstance.transform.position.y,
            _primaryInstance.transform.position.z);

        IsMirrored = true;
    }

    public void SetPrimaryAndClean(PlayerInstance instance)
    {
        if (!ReferenceEquals(instance, _primaryInstance))
        {
            var tmp = _primaryInstance;
            _primaryInstance = _secondaryInstance;
            _secondaryInstance = tmp;
        }

        PoolSecondary();
    }

    public void Pause(bool pause)
    {
        _rb.simulated = !pause;
    }

    public void Hide(bool hide)
    {
        _primaryInstance.Renderer.enabled = !hide;
        if (_secondaryInstance != null)
        {
            _secondaryInstance.Renderer.enabled = !hide;
        }
    }

    public void Restart()
    {
        _currScores = 0;
        _rb.velocity = Vector3.zero;
        transform.position = _initialPosition;
        _primaryInstance.transform.localPosition = Vector3.zero;
        PoolSecondary();
    }

    public void Stop()
    {
        Pause(true);
        Restart();
        Hide(true);
    }
}
