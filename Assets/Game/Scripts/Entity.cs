using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Components")]
    public Rigidbody _rigidbody;
    public Collider _collider;
    public Animator _animator;
    public AudioSource _audioSource;
    public ParticleSystem _particleSystem;
    public Outline _outline;

    [Header("Entity")]
    public bool isDraggable;
    public bool resetRotationAfterDrop;
    [SerializeField]private bool _isFreezed;

    public bool unfreezedUseGravity;
    public bool unfreezedIsKinematic;
    //public int wasItemLayer;

    public Vector3 unfreezedVelocity;
    public Vector3 unfreezedAngularVelocity;

    public bool isFreezed
    {
        get { return _isFreezed; }
        set
        {
            if (value && !_isFreezed)
            {
                FreezeTimeAction();
            }
            else if (!value && _isFreezed)
            {
                UnFreezeTimeAction();
            }
            _isFreezed = value;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FreezeTimeAction()
    {
        if (_rigidbody)
        {
            unfreezedIsKinematic = _rigidbody.isKinematic;
            unfreezedUseGravity = _rigidbody.useGravity;

            unfreezedVelocity = _rigidbody.velocity;
            unfreezedAngularVelocity = _rigidbody.angularVelocity;

            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        if (_particleSystem)
        {
            _particleSystem.Pause();
        }

        if (_animator)
            _animator.speed = 0;
    }

    protected virtual void UnFreezeTimeAction()
    {
        
        if (_rigidbody)
        {
            if (Player.singleton.holdingItem == this)//if holding right now, not return gravity, only set variables in player
            {
                Player.singleton.wasItemUseGravity = unfreezedUseGravity;
                Player.singleton.wasItemIsKinematic = unfreezedIsKinematic;
            }
            else
            {
                _rigidbody.useGravity = unfreezedUseGravity;
                _rigidbody.isKinematic = unfreezedIsKinematic;

                _rigidbody.velocity = unfreezedVelocity;
                _rigidbody.angularVelocity = unfreezedAngularVelocity;
            }
        }

        if (_particleSystem)
        {
            _particleSystem.Play();
        }

        if (_animator)
            _animator.speed = 1;
    }
}
