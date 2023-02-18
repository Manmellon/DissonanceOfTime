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

        if (isFreezed) return;

        doorWayCollider.enabled = false;
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        _animator.SetBool("Pressed", false);

        if (isFreezed) return;

        doorWayCollider.enabled = true;
    }

    protected override void FreezeTimeAction()
    {
        base.FreezeTimeAction();
    }

    protected override void UnFreezeTimeAction()
    {
        base.UnFreezeTimeAction();

        if (isOn && doorWayCollider.enabled)
            doorWayCollider.enabled = false;
        else if (!isOn && !doorWayCollider.enabled)
            doorWayCollider.enabled = true;
    }
}
