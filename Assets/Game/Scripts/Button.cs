using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : SwitchingEntity
{
    [Header("Button")]
    public ButtonCollider buttonCollider;
    public Collider pressingCollider;
    public Collider baseCollider;

    public SwitchingEntity[] influencedSwitchEntities;

    protected override void Start()
    {
        base.Start();

        Physics.IgnoreCollision(pressingCollider, baseCollider);
    }

    protected override void Update()
    {
        base.Update();

        //Physics.BoxCast();
    }

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

    private void OnTriggerStay(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && entity.isFreezed) return;

        if (!isOn)
        {
            TurnOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && entity.isFreezed) return;

        if (isOn)
        {
            TurnOff();
        }
    }
}
