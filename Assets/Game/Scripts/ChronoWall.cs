using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoWall : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public ChronoPillar chronoPillarA;
    public ChronoPillar chronoPillarB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meshFilter.mesh.vertices[0] = chronoPillarA.topVertex.position;
        meshFilter.mesh.vertices[1] = chronoPillarA.bottomVertex.position;
        meshFilter.mesh.vertices[2] = chronoPillarB.bottomVertex.position;
        meshFilter.mesh.vertices[3] = chronoPillarB.topVertex.position;
    }
}
