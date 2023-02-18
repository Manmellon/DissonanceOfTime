using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : SwitchingEntity
{
    [Header("Door")]
    public Collider doorWayCollider;
    public float openSpeed = 1;

    //public bool openByDefault;

    protected override void TurnedOnAction()
    {
        base.TurnedOnAction();

        _animator.SetBool("Pressed", true);

        doorWayCollider.enabled = false;
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        doorWayCollider.enabled = true;

        _animator.SetBool("Pressed", false);
    }
}
