using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;


public class ConnectOnManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject LoadingPanel;

    [SerializeField] private UnityTransport _unityTransport;
    [SerializeField] private TMP_Text code;
    [SerializeField] private TMP_InputField codeInput;
    [SerializeField] private NetcodeCameraMovement netcodeCameraMovement;
    private async void Start()
    {
        //Autenrticação no Unity Services
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Connected " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRoom()
    {
        CreateRelay();
    }

    private async void CreateRelay()
    {
        LoadingPanel.SetActive(true);

        RemoveAnyOldNetworkObject();

        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.Log(joinCode);

        _unityTransport.SetHostRelayData(
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key, allocation.ConnectionData);

        code.text = joinCode;
        code.gameObject.SetActive(true);

        panel.SetActive(false);
        NetworkManager.Singleton.StartHost();

        LoadingPanel.SetActive(false);

        panel.SetActive(false);
        netcodeCameraMovement.MoveToCharacterSelection();
    }

    public async void JoinRelay()
    {
        string joinCode = codeInput.text;
        if (joinCode.Equals(string.Empty)) return; // Entrada inválida
        try
        {
            LoadingPanel.SetActive(true);

            RemoveAnyOldNetworkObject();

            Debug.Log("Joing with code " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            _unityTransport.SetClientRelayData(joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData);

            NetworkManager.Singleton.StartClient();

            LoadingPanel.SetActive(false);

            panel.SetActive(false);
            netcodeCameraMovement.MoveToCharacterSelection();
        }
        catch (RelayServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    private void RemoveAnyOldNetworkObject()
    {
        var players = FindObjectsByType<NetcodePlayerController>(FindObjectsSortMode.None);
        foreach(var player in players)
        {
            Destroy(player.gameObject);
        }
        EventManager.Instance.ClearNetcodePlayers();
    }
}
