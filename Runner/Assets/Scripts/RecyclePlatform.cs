using System.Collections.Generic;
using UnityEngine;

public class RecyclePlatform : RecycleObjects
{
    [SerializeField] protected Vector3 minSize;
    [SerializeField] protected Vector3 maxSize;
    
    [SerializeField] private Vector3 minGap;
    [SerializeField] private Vector3 maxGap;
    
    [Space]
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
	
    [Space]
    [SerializeField] private Material[] materials;
    
    private Vector3 _nextPosition;
    private readonly Queue<Transform> _objectQueue = new Queue<Transform>();
    
    public override void Recycle()
    {
        var scale = SetScale();
        var position = SetPosition(_nextPosition, scale);
        //coin.Spawned(position);
        
        var obj = _objectQueue.Dequeue();
        obj.localScale = scale;
        obj.localPosition = position;

        //var materialIndex = Random.Range(0, materials.Length);
        //_materialController.SetMaterial(obj, materials, materialIndex);
        //_materialController.SetPhysicMaterial(obj, physicMaterials, materialIndex);

        _objectQueue.Enqueue(obj);
		
        _nextPosition += new Vector3
        (
            Random.Range(minGap.x, maxGap.x) + scale.x,
            Random.Range(minGap.y, maxGap.y),
            Random.Range(minGap.z, maxGap.z)
        );

        if (_nextPosition.y < minY)
        {
            _nextPosition.y = minY + maxGap.y;
        }
        else if (_nextPosition.y > maxY)
        {
            _nextPosition.y = maxY - maxGap.y;
        }
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
