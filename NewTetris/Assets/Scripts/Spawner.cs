using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Shape[] _allShapes;
    [SerializeField] private Transform[] _queuedXForms = new Transform[3];
    
    private Shape[] _queuedShapes = new Shape[3];

    private Shape _shape = null;

    private float _queueScale = 0.5f;

    private void Start()
    {
        InitQueue();
    }
    
    private void InitQueue()
    {
        for (var i = 0; i < _queuedShapes.Length; i++)
        {
            _queuedShapes[i] = null;
        }
        
        FillQueue();
    }
    
    //TODO: Добавить ObjectPool
    private void FillQueue()
    {
        for (var i = 0; i < _queuedShapes.Length; i++)
        {
            if (_queuedShapes[i] == null)
            {
                _queuedShapes[i] = Instantiate(RandomShape, transform.position, Quaternion.identity);
                _queuedShapes[i].transform.position = _queuedXForms[i].position + _queuedShapes[i].queueOffset;
                _queuedShapes[i].transform.localScale = new Vector3(_queueScale, _queueScale, _queueScale);
            }
        }
    }

    private Shape RandomShape
    {
        get
        {
            var index = Random.Range(0, _allShapes.Length);
        
            if (_allShapes[index])
            {
                return _allShapes[index];
            }
            else
            {
                Debug.LogWarning("WARNING: Invalid shape in spawner!");
                return null;
            }
        }
    }

    public Shape SpawnShape()
    {
        _shape = GetQueuedShape();
        _shape.transform.position = transform.position;
        _shape.transform.localScale = Vector3.one;
        
        if (_shape)
        {
            return _shape;
        }
        else
        {
            Debug.LogWarning("WARNING: Invalid shape in spawner!");
            
            //TODO: Возможно нужно _shape = null
            return null;
        }
    }

    private Shape GetQueuedShape()
    {
        Shape firstShape = null;

        if (_queuedShapes[0])
        {
            firstShape = _queuedShapes[0];
        }

        for (var i = 1; i < _queuedShapes.Length; i++)
        {
            _queuedShapes[i - 1] = _queuedShapes[i];
            _queuedShapes[i - 1].transform.position = _queuedXForms[i - 1].position + _queuedShapes[i - 1].queueOffset;
        }

        _queuedShapes[_queuedShapes.Length - 1] = null;
        
        FillQueue();

        return firstShape;
    }
}
