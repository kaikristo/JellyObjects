using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyVertex
{
    private Vector3 startPosition;
    private Vector3 currentPosition;

    private Vector3 currentVelocity;

    public Vector3 CurrentPosition { get => currentPosition; set => currentPosition = value; }
    public Vector3 CurrentVelocity { get => currentVelocity; }

    public JellyVertex(Vector3 startPosition, Vector3 currentPosition, Vector3 currentVelocity)
    {
        this.startPosition = startPosition;
        this.currentPosition = currentPosition;
        this.currentVelocity = currentVelocity;
    }

    public Vector3 GetDisplacement()
    {
        return currentPosition - startPosition;
    }

    public void UpdateVelocity(float bounceSpeed)
    {
        currentVelocity -= GetDisplacement() * bounceSpeed * Time.deltaTime;
    }

    public void Settle(float elasticity)
    {
        currentVelocity *= 1f - elasticity * Time.deltaTime;
    }

    public void ApplyPressure(Transform from, Vector3 inputPosition, float pressure)
    {
        Vector3 distanceVerticePoint = currentPosition - from.InverseTransformPoint(inputPosition);
        float adaptedPressure = pressure / (1f + distanceVerticePoint.sqrMagnitude);
        float velocity = adaptedPressure * Time.deltaTime;
        currentVelocity += distanceVerticePoint.normalized * velocity;
    }

    internal void UpdatePosition()
    {
        CurrentPosition += CurrentVelocity * Time.deltaTime;
    }

}
