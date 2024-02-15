using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFloorPart : FloorPart
{
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private ParticleSystem particleSystem;

    public Material normalMat;
    public Material finishMat;

    public override void Init()
    {
        base.Init();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        particleSystem= transform.Find("ParticleSystem").GetComponent<ParticleSystem>();
    }

    public void Reached(bool reached)
    {
        meshRenderer.material = reached ? normalMat : finishMat;
        meshCollider.isTrigger = !reached;
        particleSystem.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Ball ball))
        {
            Reached(true);
            ball.FinishedLevel();
        }
    }
}
