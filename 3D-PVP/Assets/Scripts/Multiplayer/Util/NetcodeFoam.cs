using DG.Tweening;
using UnityEngine;
using Unity.Netcode;

public class NetcodeFoam : NetworkBehaviour
{
    private Vector3 startingScale;
    private void StartBattle()
    {
        if (!IsServer) return;
        transform.DOScale(new Vector3(5f, 9f, 5f), 20f).SetEase(Ease.InQuart);
    }

    private void EndBattle()
    {
        if (!IsServer) return;
        DOTween.Complete(transform);

        transform.localScale = new Vector3(10f, 9f, 10f);
    }

    private void OnEnable()
    {
        EventManager.StartMatch += StartBattle;
        EventManager.EndMatch += EndBattle;
    }

    private void OnDisable()
    {
        EventManager.StartMatch -= StartBattle;
        EventManager.EndMatch -= EndBattle;
    }
}
