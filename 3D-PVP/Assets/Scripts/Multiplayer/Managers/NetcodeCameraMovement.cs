using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class NetcodeCameraMovement : NetworkBehaviour
{
    [SerializeField] CameraPointSettings[] cameraPoints;

    private void StartingMatchMovement()
    {
        if (IsServer)
        {
            StartMovementClientRPC();
        }
    }

    [ClientRpc]
    private void StartMovementClientRPC()
    {
        UIManager.Singleton.DisableStartCharacter();
        MoveToGameplay();
    }

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

    public void RestartingMovement()
    {
        if (IsServer)
        {
            RestartingClientRPC();
        }
    }

    [ClientRpc]
    private void RestartingClientRPC()
    {
        UIManager.Singleton.DisableVictoryScreen();
        MoveToCharacterSelection();
    }

    public void ToMenuMovement()
    {
        if (IsServer)
        {
            ToMenuClientRPC();
        }
    }

    [ClientRpc]
    private void ToMenuClientRPC()
    {
        UIManager.Singleton.DisableVictoryScreen();
        MoveToMenu();
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
        EventManager.PlayersReady += StartingMatchMovement;
    }

    private void OnDisable()
    {
        EventManager.PlayersReady -= StartingMatchMovement;
    }
}
