using System.Collections.Generic;
using UnityEngine;

public interface IMirrorable<T>
{
    public bool IsMirrored { get; set; }
    public float Width { get; }
    public float PositionX { get; }
    public T PrimaryInstance { get; }
    public T SecondaryInstance { get; }
    public void Mirror(float positionX);
    public void SetPrimaryAndClean(T instance);
}
