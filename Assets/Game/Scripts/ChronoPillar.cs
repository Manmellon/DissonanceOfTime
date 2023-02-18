using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoPillar : SwitchingEntity
{
    [Header("Chrono Pillar")]
    public Transform topVertex;
    public Transform bottomVertex;

    public ChronoWall chronoWallPrefab;

    public List<ChronoWall> connections = new List<ChronoWall>();

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
        ChronoWall chronoWall = Instantiate(chronoWallPrefab);
        chronoWall.chronoPillarA = this;
        chronoWall.chronoPillarB = otherPillar;

        connections.Add(chronoWall);
        otherPillar.connections.Add(chronoWall);
    }

    public void RemoveConnection(ChronoWall wall)
    {
        wall.UnfreezeAll();
        Destroy(wall.gameObject);
    }

    public void ProcessConnection(ChronoPillar otherPillar)
    {
        foreach (var wall in connections)
        {
            if (wall.chronoPillarA == otherPillar || wall.chronoPillarB == otherPillar)
            {
                connections.Remove(wall);
                otherPillar.connections.Remove(wall);
                RemoveConnection(wall);
                return;
            }
        }

        AddConnection(otherPillar);
    }

    public void ResetAllConnections()
    {
        for (int i = 0; i < connections.Count; i++)
        {
            var wall = connections[i];
            if (wall.chronoPillarA == this)
                wall.chronoPillarB.connections.Remove(wall);
            else if (wall.chronoPillarB == this)
                wall.chronoPillarA.connections.Remove(wall);

            RemoveConnection(wall);
        }

        connections.Clear();
    }
}
