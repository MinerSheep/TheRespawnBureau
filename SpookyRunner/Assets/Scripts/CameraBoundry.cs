using System;
using UnityEngine;
using Unity.Cinemachine;

public class CameraBoundry : MonoBehaviour
{
    public bool upDownMovement = true;

    CinemachinePositionComposer composer;

    private void Start()
    {
        CameraEvents.OnGrounded += SetCheckUpper;

        composer = GetComponent<CinemachinePositionComposer>();
    }

    public void SetCheckUpper(bool flag)
    {
        if (!upDownMovement)
            return;

        var comp = composer.Composition;
        comp.DeadZone.Size.y = flag ? 0.3f : 0.6f;
        composer.Composition = comp;
    }

    private void OnDestroy()
    {
        CameraEvents.OnGrounded -= SetCheckUpper;
    }
}
