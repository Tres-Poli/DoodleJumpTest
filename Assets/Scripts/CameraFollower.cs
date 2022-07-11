using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] PlayerController _player;

    private Vector3 _initialPosition;

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (_player.PrimaryInstance.transform.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, _player.PrimaryInstance.transform.position.y, transform.position.z);
        }
    }

    public void Restart()
    {
        transform.position = _initialPosition;
    }

    public void Stop()
    {
        Restart();
    }
}
