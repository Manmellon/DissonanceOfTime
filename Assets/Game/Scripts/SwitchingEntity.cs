using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Indicator
{
    public MeshRenderer indicatorMesh;
    public int material_index;
    public Material enabledMaterial;
    public Material disabledMaterial;
}

public class SwitchingEntity : Entity
{
    [Header("Switching Entity")]
    public Indicator[] indicators;

    public bool switchByClick;

    [SerializeField]private bool _isOn;
    public bool isOn {
        get {return _isOn;}
        set {
            _isOn = value;
            if (value)
            {
                TurnedOnAction();
            }
            else
            {
                TurnedOffAction();
            }
        }
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
 
        if (isOn)
        {
            TurnedOnAction();
        }
        else
        {
            TurnedOffAction();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Switch()
    {
        isOn = !isOn;
    }

    public void TurnOn()
    {
        isOn = true;
    }

    public void TurnOff()
    {
        isOn = false;
    }

    protected virtual void TurnedOnAction()
    {
        foreach (var indicator in indicators)
        {
            Material[] mats = indicator.indicatorMesh.materials;
            mats[indicator.material_index] = indicator.enabledMaterial;
            indicator.indicatorMesh.materials = mats;
        }

        if (_particleSystem)
        {
            _particleSystem.Play();
        }
    }

    protected virtual void TurnedOffAction()
    {
        foreach (var indicator in indicators)
        {
            Material[] mats = indicator.indicatorMesh.materials;
            mats[indicator.material_index] = indicator.disabledMaterial;
            indicator.indicatorMesh.materials = mats;
        }

        if (_particleSystem)
        {
            _particleSystem.Stop();
        }
    }

    protected override void UnFreezeTimeAction()
    {
        base.UnFreezeTimeAction();

        if (!isOn)
        {
            if (_particleSystem)
                _particleSystem.Stop();
        }
    }
}
