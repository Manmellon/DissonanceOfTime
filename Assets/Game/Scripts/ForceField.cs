using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : SwitchingEntity
{
    [Header("Force Field")]
    public Collider frontCollider;
    public Collider backCollider;
    public MeshRenderer frontMeshRenderer;
    public MeshRenderer backMeshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;

            //player.MoveHoldingItem();
            player.Drop();
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;

            //player.MoveHoldingItem();
            player.Drop();
        }
    }

    protected override void TurnedOnAction()
    {
        base.TurnedOnAction();

        frontCollider.enabled = true;
        backCollider.enabled = true;
        frontMeshRenderer.enabled = true;
        backMeshRenderer.enabled = true;
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        frontCollider.enabled = false;
        backCollider.enabled = false;
        frontMeshRenderer.enabled = false;
        backMeshRenderer.enabled = false;
    }
}
