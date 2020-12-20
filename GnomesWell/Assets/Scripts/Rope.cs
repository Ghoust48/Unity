using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject _ropeSegment;
    [SerializeField] private Rigidbody2D _rigidbodyConnectedObj;
    [SerializeField] private float _maxRopeSegmentLength = 1.0f;
    [SerializeField] private float _ropeSpeed = 4.0f;

    private const int TopSegmentIndex = 0;
    private const int NextSegmentIndex = 1;
    private const int MinSegmentCount = 2;

    private List<GameObject> _ropeSegments = new List<GameObject>();
    
    private LineRenderer _lineRenderer;
    
    private Rigidbody2D _segmentBody;
    private SpringJoint2D _segmentJoin;
    
    private GameObject _topSegment;
    private SpringJoint2D _topSegmentJoin;
    
    private bool _isLineRendererNotNull;

    public bool IsIncreasing { get; set; }
    public bool IsDecreasing { get; set; }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _isLineRendererNotNull = _lineRenderer != null;
        
        ManagerPool.Instance.AddPool(PoolType.RopeSegment);

        CreateSegment();
    }
    private void CreateSegment()
    {
        var segment = ManagerPool.Instance.Spawn(PoolType.RopeSegment, _ropeSegment);
        
        segment.transform.SetParent(gameObject.transform, true);

        _segmentBody = segment.GetComponent<Rigidbody2D>();
        _segmentJoin = segment.GetComponent<SpringJoint2D>();

        if (_segmentBody == null || _segmentJoin == null)
        {
            Debug.LogError("Отсутствует компонент Rigidbody2D или SpringJoint2D");
            
            return;
        }
        
        _ropeSegments.Insert(TopSegmentIndex, segment);

        if (_ropeSegments.Count == NextSegmentIndex)
        {
            var connectedSpringJoin = _rigidbodyConnectedObj.GetComponent<SpringJoint2D>();
            connectedSpringJoin.connectedBody = _segmentBody;
            connectedSpringJoin.distance = 0.1f;
            _segmentJoin.distance = _maxRopeSegmentLength;
        }
        else
        {
            var nextSegment = _ropeSegments[NextSegmentIndex];
            var nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D>();
            nextSegmentJoint.connectedBody = _segmentBody;
            _segmentJoin.distance = 0.0f;
        }

        _segmentJoin.connectedBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void RemoveSegment()
    {
        if (_ropeSegments.Count < MinSegmentCount) 
            return;
        
        _topSegment = _ropeSegments[TopSegmentIndex];
        var nextSegment = _ropeSegments[NextSegmentIndex];

        var nextSegmentJoin = nextSegment.GetComponent<SpringJoint2D>();
        nextSegmentJoin.connectedBody = gameObject.GetComponent<Rigidbody2D>();
        _ropeSegments.RemoveAt(TopSegmentIndex);
            
        ManagerPool.Instance.Despawn(PoolType.RopeSegment, _topSegment);
    }

    private void Update()
    {
        _topSegment = _ropeSegments[TopSegmentIndex];
        _topSegmentJoin = _topSegment.GetComponent<SpringJoint2D>();

        if (IsIncreasing)
        {
            if (_topSegmentJoin.distance >= _maxRopeSegmentLength)
            {
                CreateSegment();
            }
            else
            {
                _topSegmentJoin.distance += _ropeSpeed * Time.deltaTime;
            }
        }
        
        if (IsDecreasing)
        {
            if (_topSegmentJoin.distance <= 0.005f)
            {
                RemoveSegment();
            }
            else
            {
                _topSegmentJoin.distance -= _ropeSpeed * Time.deltaTime;
            }
        }

        if (_isLineRendererNotNull)
        {
            _lineRenderer.positionCount = _ropeSegments.Count + MinSegmentCount;
            
            _lineRenderer.SetPosition(0, gameObject.transform.position);
            
            for (var i = 0; i < _ropeSegments.Count; i++) 
            {
                _lineRenderer.SetPosition(i+1,
                    _ropeSegments[i].transform.position);
            }
            
            var connectedObjectJoint = _rigidbodyConnectedObj.GetComponent<SpringJoint2D>();
            
            _lineRenderer.SetPosition(_ropeSegments.Count + 1, 
                _rigidbodyConnectedObj.transform.TransformPoint(connectedObjectJoint.anchor));
        }
    }
}