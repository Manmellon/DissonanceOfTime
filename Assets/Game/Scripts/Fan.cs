using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FanMode { UP, DOWN}
public class Fan : SwitchingEntity
{
    [Header("Fan Components")]
    public FanMode mode;

    // Start is called before the first frame update
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

    public override void TurnedOnAction()
    {
        base.TurnedOnAction();

        if (mode == FanMode.UP)
            _animator.Play("RotateLeft");
        else
            _animator.Play("RotateRight");
    }

    public override void TurnedOffAction()
    {
        base.TurnedOffAction();

        _animator.Play("Idle");
    }
}
