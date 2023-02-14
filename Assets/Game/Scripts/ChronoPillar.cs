using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoPillar : SwitchingEntity
{
    [Header("Chrono Pillar")]
    public Transform topVertex;
    public Transform bottomVertex;

    public ChronoWall chronoWallPrefab;

    public List<ChronoPillar> connections = new List<ChronoPillar>();

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //for test
        if (Input.GetKeyDown(KeyCode.E))
        {
            Switch();
        }
    }

    public void AddConnection(ChronoPillar otherPillar)
    {
        if ( !connections.Contains(otherPillar) )
            connections.Add(otherPillar);
    }

    public void RemoveConnection(ChronoPillar otherPillar)
    {
        if (connections.Contains(otherPillar))
            connections.Remove(otherPillar);
    }
}
