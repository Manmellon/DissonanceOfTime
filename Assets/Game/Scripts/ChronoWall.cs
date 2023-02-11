using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoWall : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshFilter meshFilterBack;

    public LayerMask chronoLayers;
    
    public ChronoPillar chronoPillarA;
    public ChronoPillar chronoPillarB;

    [SerializeField] private List<Entity> _frozenEntities = new List<Entity>();

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

        CheckRayCasts();
    }

    private void LateUpdate()
    {
        //CheckRayCasts();
    }

    public void CheckRayCasts()
    {
        //First unfreeze all
        foreach (var entity in _frozenEntities)
        {
            entity.isFreezed = false;
        }
        _frozenEntities.Clear();

        //Then create new List of frozen (may cause freese flickering, need think later...)

        int rayCount = 5;

        for (int i = 0; i < rayCount; i++)
        {
            RaycastHit hit;

            Vector3 a = Vector3.Lerp(chronoPillarA.topVertex.position, chronoPillarA.bottomVertex.position, (float)i / (rayCount-1));
            Vector3 b = Vector3.Lerp(chronoPillarB.topVertex.position, chronoPillarB.bottomVertex.position, (float)i / (rayCount-1));

            var ray = new Ray(a, b-a);

            if (Physics.Raycast(ray, out hit, (chronoPillarB.topVertex.position - chronoPillarA.topVertex.position).magnitude, chronoLayers))
            {
                Entity entity = hit.collider.gameObject.GetComponentInParent<Entity>();
                if (entity && entity != chronoPillarA && entity != chronoPillarB)
                {
                    Debug.DrawLine(a, b, Color.green);
                    entity.isFreezed = true;
                    if (! _frozenEntities.Contains(entity))
                        _frozenEntities.Add(entity);
                }
                else
                {
                    Debug.DrawLine(a, b, Color.yellow);
                }
            }
            else
            {
                Debug.DrawLine(a, b, Color.red);
            }
        }
    }
}
