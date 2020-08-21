using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class JellyMesh : MonoBehaviour
{
    [SerializeField]
    private float bounceSpeed;
    [SerializeField]
    private float force;
    [SerializeField]
    private float resistance;

    private MeshFilter meshFilter;
    private Mesh mesh;

    JellyVertex[] jellyVertices;
    Vector3[] currentMeshVerices;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        GetVerticies();
    }

    private void GetVerticies()
    {
        jellyVertices = new JellyVertex[mesh.vertices.Length];
        currentMeshVerices = new Vector3[mesh.vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            jellyVertices[i] = new JellyVertex(mesh.vertices[i], mesh.vertices[i], Vector3.zero);
            currentMeshVerices[i] = mesh.vertices[i];
        }
    }

    private void Update()
    {
        UpdateVerticies();
    }

    private void UpdateVerticies()
    {
        bool needToUpdate = false;
        for (int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].UpdateVelocity(bounceSpeed);
            jellyVertices[i].Settle(resistance);
            jellyVertices[i].UpdatePosition();
           
            if (!currentMeshVerices[i].Equals(jellyVertices[i].CurrentPosition))
            {
                needToUpdate = true;
                currentMeshVerices[i] = jellyVertices[i].CurrentPosition;
            }
        }

        if (needToUpdate)
        {
            mesh.vertices = currentMeshVerices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            Vector3 inputPoint = contactPoints[i].point + contactPoints[i].point * .1f;
            ApplyPressureToPoint(inputPoint, force);
        }
    }

    private void ApplyPressureToPoint(Vector3 inputPoint, float mass)
    {
        for (int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].ApplyPressure(transform, inputPoint, mass);
        }
    }
}
