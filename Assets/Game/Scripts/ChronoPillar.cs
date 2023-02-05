using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoPillar : SwitchingEntity
{
    [Header("Chrono Pillar")]
    public Transform topVertex;
    public Transform bottomVertex;

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
}
