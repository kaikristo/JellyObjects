using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    enum MoveType
    {
        AlwaysMove,
        Drag
    };

    private Camera main;
    [SerializeField]
    private MoveType moveType = MoveType.AlwaysMove;

    private void Start()
    {
        main = Camera.main;
        float distanceToScreen = main.WorldToScreenPoint(gameObject.transform.position).z;
    }

    private void Update()
    {
        if(moveType == MoveType.AlwaysMove)
        {
            Move();
        }
    }
    // Update is called once per frame
    void OnMouseDrag()
    {
        if (moveType == MoveType.Drag)
        {
            Move();
        }
    }

    private void Move()
    {
        float distanceToScreen = main.WorldToScreenPoint(gameObject.transform.position).z;
        transform.position = main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
    }
}
