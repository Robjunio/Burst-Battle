using DG.Tweening;
using UnityEngine;

public class Foam : MonoBehaviour
{
    private void StartBattle()
    {
        transform.DOScale(new Vector3(0.5f, 0.9f, 0.5f), 90f).SetEase(Ease.InQuart);
    }

    private void EndBattle()
    {
        transform.DOScale(new Vector3(1f, 0.9f, 1f), 3f);
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
