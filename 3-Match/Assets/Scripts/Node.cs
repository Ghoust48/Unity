using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject highlight;
    public int Id { get; set; }
    public bool Ready { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
}
