using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextUiAnimation : MonoBehaviour
{
    TMP_Text text;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();
    }
    public void MakeTransition()
    {
        StartCoroutine(CountdownTransition());
    }

    IEnumerator CountdownTransition()
    {
        text.text = "3";
        transform.localPosition = new Vector3(-2000f, 0f, 0f);
        yield return rectTransform.DOAnchorPos(new Vector2(0f, 0f), 0.4f, false).SetEase(Ease.OutQuart).WaitForCompletion();
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.75f).SetLoops(4, LoopType.Yoyo);
        yield return new WaitForSeconds(1f);
        text.text = "2";
        yield return new WaitForSeconds(1f);
        text.text = "1";
        yield return new WaitForSeconds(1f);
        text.text = "Go!";
        yield return new WaitForSeconds(0.3f);

        EventManager.Instance.OnMatchStarted();
        yield return rectTransform.DOAnchorPos(new Vector2(2000f, 0f), 0.4f, false).SetEase(Ease.OutQuart).WaitForCompletion();

    }

    private void OnEnable()
    {
        EventManager.ReachGameplay += MakeTransition;
    }

    private void OnDisable()
    {
        EventManager.ReachGameplay -= MakeTransition;
    }
}
