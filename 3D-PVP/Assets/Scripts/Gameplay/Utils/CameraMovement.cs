using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Points {
    Menu,
    Gameplay,
    CharacterSelection
}

[Serializable]
public class CameraPointSettings
{
    public Points Point;
    public Transform pointTransform;
    public float FOV;
}

public class CameraMovement : MonoBehaviour
{
    [SerializeField] CameraPointSettings[] cameraPoints;

    public void MoveToGameplay()
    {
        transform.DOMove(cameraPoints[1].pointTransform.position, 0.8f);
        transform.DORotate(cameraPoints[1].pointTransform.rotation.eulerAngles, 0.8f).OnComplete(() => {
            EventManager.Instance.OnReachGameplay();
        });

        Camera.main.fieldOfView = cameraPoints[1].FOV;
    }

    public void MoveToMenu()
    {
        transform.DOMove(cameraPoints[0].pointTransform.position, 0.8f);
        transform.DORotate(cameraPoints[0].pointTransform.rotation.eulerAngles, 0.8f).OnComplete(() => {
            EventManager.Instance.OnReachMenu();
        });

        Camera.main.fieldOfView = cameraPoints[0].FOV;
    }

    public void MoveToCharacterSelection()
    {
        transform.DOMove(cameraPoints[2].pointTransform.position, 0.8f);
        transform.DORotate(cameraPoints[2].pointTransform.rotation.eulerAngles, 0.8f).OnComplete(() => {
            EventManager.Instance.OnReachCharacter();
        });

        Camera.main.fieldOfView = cameraPoints[2].FOV;
    }

    private void OnEnable()
    {
        EventManager.PlayersReady += MoveToGameplay;
    }

    private void OnDisable()
    {
       EventManager.PlayersReady -= MoveToGameplay;
    }
}
