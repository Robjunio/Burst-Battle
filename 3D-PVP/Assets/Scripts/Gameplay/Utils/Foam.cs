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
        DOTween.Complete(transform);

        transform.localScale = new Vector3(1f, 0.9f, 1f);
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
