using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class ObjectMirroring<T> where T : MonoBehaviour
{
    private IMirrorable<T> _mirrorable;

    private float _rightScreenBorder;
    private float _leftScreenBorder;

    private float _halfWidth;

    public ObjectMirroring(IMirrorable<T> obj)
    {
        _mirrorable = obj;
        _mirrorable.IsMirrored = false;
        _rightScreenBorder = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        _leftScreenBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        _halfWidth = _mirrorable.Width;
    }

    public void Update()
    {
        var primPosX = _mirrorable.PrimaryInstance.transform.position.x;
        if (!_mirrorable.IsMirrored)
        {
            if (primPosX + _halfWidth > _rightScreenBorder || primPosX - _halfWidth < _leftScreenBorder)
            {
                _mirrorable.Mirror(primPosX - Mathf.Sign(primPosX) * _rightScreenBorder * 2);
            }
        }
        else // Have second instance
        {
            var secPosX = _mirrorable.SecondaryInstance.transform.position.x;
            if (IsOutsideScreen(primPosX))
            {
                _mirrorable.SetPrimaryAndClean(_mirrorable.SecondaryInstance);
            }
            else if (IsOutsideScreen(secPosX))
            {
                _mirrorable.SetPrimaryAndClean(_mirrorable.PrimaryInstance);
            }
        }
    }

    private bool IsOutsideScreen(float x)
    {
        return x - _halfWidth > _rightScreenBorder || x + _halfWidth < _leftScreenBorder;
    }
}
