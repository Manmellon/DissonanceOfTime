using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoWall : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;

    public MeshFilter meshFilterBack;
    public MeshCollider meshColliderBack;


    public ChronoPillar chronoPillarA;
    public ChronoPillar chronoPillarB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] vertices = { chronoPillarA.topVertex.position, chronoPillarA.bottomVertex.position, chronoPillarB.bottomVertex.position, chronoPillarB.topVertex.position };

        meshFilter.mesh.SetVertices(vertices);

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            0, 3, 2
        };
        meshFilter.mesh.SetTriangles(tris, 0);
        //meshFilter.mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        meshFilter.mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
              new Vector2(0, 0),
              new Vector2(1, 0),
              new Vector2(0, 1),
              new Vector2(1, 1)
        };
        meshFilter.mesh.uv = uv;

        meshFilter.mesh.RecalculateBounds();

        meshCollider.sharedMesh = meshFilter.mesh;

        //And for back face

        meshFilterBack.mesh.SetVertices(vertices);
        int[] backTris = new int[6]
        {
            // lower left triangle
            0, 1, 2,
            // upper right triangle
            0, 2, 3
        };
        meshFilterBack.mesh.SetTriangles(backTris, 0);

        meshFilterBack.mesh.SetNormals(normals);

        meshFilterBack.mesh.uv = uv;

        meshFilterBack.mesh.RecalculateBounds();

        meshColliderBack.sharedMesh = meshFilterBack.mesh;
    }
}
