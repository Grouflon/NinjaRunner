using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class SlopeController : MonoBehaviour
{
    public bool uphill = false;
    public Material material;

	void Start ()
	{
        Mesh mesh = new Mesh();
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_collider = GetComponent<PolygonCollider2D>();

        m_meshFilter.mesh = mesh;
        m_meshRenderer.material = material;

        int[] indices = new int[3];
        Vector3[] vertices = new Vector3[3];

        if (uphill)
        {
            vertices[0] = new Vector3(0.0f, 0.0f, 0.0f);
            vertices[1] = new Vector3(1.0f, 1.0f, 0.0f);
            vertices[2] = new Vector3(1.0f, 0.0f, 0.0f);
        }
        else
        {
            vertices[0] = new Vector3(0.0f, 0.0f, 0.0f);
            vertices[1] = new Vector3(1.0f, -1.0f, 0.0f);
            vertices[2] = new Vector3(0.0f, -1.0f, 0.0f);
        }

        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;

        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        Vector2[] colliderVertices = new Vector2[3];
        for (int i = 0; i < 3; ++i)
            colliderVertices[i] = new Vector2(vertices[i].x, vertices[i].y);

        m_collider.points = colliderVertices;
    }
	
	void Update ()
	{
	    
	}

    MeshFilter m_meshFilter;
    MeshRenderer m_meshRenderer;
    PolygonCollider2D m_collider;
}
