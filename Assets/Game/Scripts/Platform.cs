using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : SwitchingEntity
{
    [Header("Platform")]
    public Vector3 offPosition;
    public Vector3 onPosition;

    public float speed;

    public float startMovingTime;
    public Vector3 curDirection;

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

    protected override void TurnedOnAction()
    {
        base.TurnedOnAction();

        curDirection = onPosition - offPosition;
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        curDirection = offPosition - onPosition;
    }
}
