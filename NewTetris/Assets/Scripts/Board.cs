using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _emptySprite;
    [SerializeField] private int _height = 30;
    [SerializeField] private int _width = 10;
    [SerializeField] private int _header = 8;

    public int CompletedRows { get; set; } = 0;

    private Transform[,] _grid;

    private void Awake()
    {
        _grid = new Transform[_width, _height];
    }

    private void Start()
    {
        DrawEmptyCells();
    }

    private bool IsWithinBoard(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0);
    }

    private bool IsOccupied(int x, int y, Shape shape)
    {
        return (_grid[x, y] != null && _grid[x, y].parent != shape.transform);
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);

            if (!IsWithinBoard((int) pos.x, (int) pos.y))
            {
                return false;
            }

            if (IsOccupied((int) pos.x, (int) pos.y, shape))
            {
                return false;
            }
        }

        return true;
    }

    private void DrawEmptyCells()
    {
        if (_emptySprite == null)
        {
            Debug.LogWarning("WARNING: Please assign the 'Empty Sprite' object!");
        }
        else
        {
            for (var y = 0; y < _height - _header; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    Transform clone;
                    clone = Instantiate(_emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    clone.name = $"Board Space ( x = {x}, y = {y} )";
                    clone.parent = transform;
                }
            }
        }
    }

    public void StorageShapeInGrid(Shape shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            _grid[(int) pos.x, (int) pos.y] = child; 
        }
    }

    private bool IsComplete(int y)
    {
        for (var x = 0; x < _width; x++)
        {
            if (_grid[x,y] == null)
            {
                return false;
            }
        }
        
        return true;
    }

    private void ClearRow(int y)
    {
        for (var x = 0; x < _width; x++)
        {
            if (_grid[x,y] != null)
            {
                Destroy(_grid[x,y].gameObject);
            }

            _grid[x, y] = null;
        }
    }

    private void ShiftOneRowDown(int y)
    {
        for (var x = 0; x < _width; x++)
        {
            if (_grid[x,y] != null)
            {
                _grid[x, y - 1] = _grid[x, y];
                _grid[x, y] = null;
                _grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    private void ShiftRowsDown(int startY)
    {
        for (var i = startY; i < _height; i++)
        {
            ShiftOneRowDown(i);
        }
    }

    public void ClearAllRows()
    {
        CompletedRows = 0;
        
        for (var y = 0; y < _height; y++)
        {
            if (IsComplete(y))
            {
                CompletedRows++;
                ClearRow(y);
                ShiftRowsDown(y + 1);
                y--;
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= _height - _header - 1)
            {
                return true;
            }
        }

        return false;
    }
}
