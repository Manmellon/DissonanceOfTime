using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : SwitchingEntity
{
    [Header("Platform")]
    public Vector3 offPosition;
    public Vector3 onPosition;

    public float speed;

    protected override void Update()
    {
        base.Update();

        if (isOn)
        {

        }
        else
        {

        }
    }
}
