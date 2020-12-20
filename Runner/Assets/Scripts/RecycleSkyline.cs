using System.Collections.Generic;
using UnityEngine;

public class RecycleSkyline : RecycleObjects
{
    [SerializeField] protected Vector3 minSize;
    [SerializeField] protected Vector3 maxSize;
    
    private Vector3 _nextPosition;
    private readonly Queue<Transform> _objectQueue = new Queue<Transform>();
    
    public override void Recycle()
    {
        var scale = SetScale();
        var position = SetPosition(_nextPosition, scale);
        
        var obj = _objectQueue.Dequeue();
        obj.localScale = scale;
        obj.localPosition = position;
        _nextPosition.x += scale.x;
        
        _objectQueue.Enqueue(obj);
    }
    
    public override Vector3 SetScale()
    {
        return new Vector3
        (
            Random.Range(minSize.x, maxSize.x),
            Random.Range(minSize.y, maxSize.y),
            Random.Range(minSize.z, maxSize.z)
        );
    }

    public override Vector3 SetPosition(Vector3 nextPosition, Vector3 scale)
    {
        var position = nextPosition;
        position.x += scale.x * 0.5f;
        position.y += scale.y * 0.5f;

        return position;
    }
}
