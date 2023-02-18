using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingEntity : Entity
{
    [Header("Switching Entity")]
    public MeshRenderer indicatorMesh;
    public int material_index;
    public Material enabledMaterial;
    public Material disabledMaterial;

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
        if (indicatorMesh)
        {
            Material[] mats = indicatorMesh.materials;
            mats[material_index] = enabledMaterial;
            indicatorMesh.materials = mats;
        }

        if (_particleSystem)
        {
            _particleSystem.Play();
        }
    }

    protected virtual void TurnedOffAction()
    {
        if (indicatorMesh)
        {
            Material[] mats = indicatorMesh.materials;
            mats[material_index] = disabledMaterial;
            indicatorMesh.materials = mats;
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
