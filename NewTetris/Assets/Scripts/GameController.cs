using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float _horizontalBounds;
    private Board _gameBoard;
    private Spawner _spawner;
    private Shape _activeShape;
    
    private float _moveDirection;

    private void Start()
    {
        _gameBoard = FindObjectOfType<Board>();
        _spawner = FindObjectOfType<Spawner>();
        
        if (!_gameBoard)
        {
            Debug.LogWarning("WARNING: Please assign the 'Board' object!");
        } 
        
        if (_spawner)
        {
            if (!_activeShape)
            {
                _activeShape = _spawner.SpawnShape();
            }

            _spawner.transform.position = Vectorf.Round(_spawner.transform.position);
        }
        else
        {
            Debug.LogWarning("WARNING: Please assign the 'Spawner' object!");
        }
    }
    
    private void Update()
    {
        if (!_spawner || !_gameBoard || !_activeShape)
        {
            return;
        }

        PlayerInput();
    }
    
    private void PlayerInput()
    {
        _moveDirection = Input.GetAxis("Horizontal");
        
        if (!_gameBoard.IsValidPosition(_activeShape))
        {
            // _activeShape.Move();
            var positionX = transform.position.x + _moveDirection * Time.deltaTime;
            positionX = Mathf.Clamp(positionX, -_horizontalBounds, _horizontalBounds);
            // _rigidbody.MovePosition(new Vector2(positionX, _rigidbody.position.y));
            _activeShape.Move(new Vector2(positionX, transform.position.y));
        }
        
        
    }
}
