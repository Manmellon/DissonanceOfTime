using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLever : SwitchingEntity
{
    [Header("Switch Lever")]
    public SwitchingEntity[] influencedSwitchEntities;

    protected override void TurnedOnAction()
    {
        base.TurnedOnAction();

        if (_animator != null)
            _animator.SetBool("Pressed", true);

        if (isFreezed) return;

        foreach (var ise in influencedSwitchEntities)
        {
            if (ise != null)
                ise.TurnOn();
        }
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        if (_animator != null)
            _animator.SetBool("Pressed", false);

        if (isFreezed) return;

        foreach (var ise in influencedSwitchEntities)
        {
            if (ise != null)
                ise.TurnOff();
        }
    }

    protected override void FreezeTimeAction()
    {
        base.FreezeTimeAction();
    }

    protected override void UnFreezeTimeAction()
    {
        base.UnFreezeTimeAction();

        foreach (var ise in influencedSwitchEntities)
        {
            if (ise == null) continue;

            if (isOn && !ise.isOn)
                ise.TurnOn();
            else if (!isOn && ise.isOn)
                ise.TurnOff();
        }
    }
}
