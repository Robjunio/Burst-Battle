using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;
using Unity.VisualScripting;

[Serializable]
public struct PlayersScore : IEquatable<PlayersScore>, INetworkSerializable
{
    public int[] player1;
    public int[] player2;
    public int[] player3;
    public int[] player4;

    // Implementação de IEquatable<T>
    public bool Equals(PlayersScore other)
    {
        return CompareArrays(player1, other.player1) &&
               CompareArrays(player2, other.player2) &&
               CompareArrays(player3, other.player3) &&
               CompareArrays(player4, other.player4);
    }

    private bool CompareArrays(int[] a, int[] b)
    {
        if (a == null || b == null) return a == b;
        if (a.Length != b.Length) return false;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i]) return false;
        }

        return true;
    }

    public override bool Equals(object obj)
    {
        return obj is PlayersScore other && Equals(other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 31 + GetArrayHashCode(player1);
        hash = hash * 31 + GetArrayHashCode(player2);
        hash = hash * 31 + GetArrayHashCode(player3);
        hash = hash * 31 + GetArrayHashCode(player4);
        return hash;
    }

    private int GetArrayHashCode(int[] array)
    {
        if (array == null) return 0;
        int hash = 17;
        foreach (var item in array)
        {
            hash = hash * 31 + item.GetHashCode();
        }
        return hash;
    }

    // Implementação de INetworkSerializable
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        SerializeArray(serializer, ref player1);
        SerializeArray(serializer, ref player2);
        SerializeArray(serializer, ref player3);
        SerializeArray(serializer, ref player4);
    }

    private void SerializeArray<T>(BufferSerializer<T> serializer, ref int[] array) where T : IReaderWriter
    {
        int length = array?.Length ?? 0;
        serializer.SerializeValue(ref length);

        if (serializer.IsWriter)
        {
            for (int i = 0; i < length; i++)
            {
                int value = array[i];
                serializer.SerializeValue(ref value);
            }
        }
        else
        {
            array = new int[length];
            for (int i = 0; i < length; i++)
            {
                int value = 0;
                serializer.SerializeValue(ref value);
                array[i] = value;
            }
        }
    }
}


public class NetcodePointsManager : NetworkBehaviour
{
    public static NetcodePointsManager Singleton;

    [SerializeField] private GameObject pointsScreem;
    [SerializeField] private GameObject transition;

    [SerializeField] private Image[] player1Points;
    [SerializeField] private Image[] player2Points;
    [SerializeField] private Image[] player3Points;
    [SerializeField] private Image[] player4Points;

    [SerializeField] private Sprite[] killSprite;

    [SerializeField] private Sprite baseSprite;

    private int PointsMax = 10;
    private int[] players = new int[4];

    private NetworkVariable<PlayersScore> _playerScore = new NetworkVariable<PlayersScore>(
        new PlayersScore() {player1 = new int[0], player2 = new int[0], player3 = new int[0], player4 = new int[0] },
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
     );

    private NetworkVariable<int> _winner = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
        );

    private void Awake()
    {
        Singleton = this;
    }

    public int GetWinner()
    {
        return _winner.Value;
    }

    /*private void PlayerGotPoint(string player)
    {
        print("Somebody Die " + player);
        switch (player)
        {
            case "Player 1":
                players[0] = players[0] + 1;
                break;
            case "Player 2":
                players[1] = players[1] + 1;
                break;
            case "Player 3":
                players[2] = players[2] + 1;
                break;
            case "Player 4":
                players[3] = players[3] + 1;
                break;
        }
    }*/

    /*private void PlayerSurvived(string player)
    {
        switch (player)
        {
            case "Player 1":
                players[0] = players[0] + 1;
                break;
            case "Player 2":
                players[1] = players[1] + 1;
                break;
            case "Player 3":
                players[2] = players[2] + 1;
                break;
            case "Player 4":
                players[3] = players[3] + 1;
                break;
        }
    }*/

    /*private void PlayerLostPoint(string player)
    {
        switch (player)
        {
            case "Player 1":
                players[0] = Mathf.Clamp(players[0] - 1, 0, 100);
                break;
            case "Player 2":
                players[1] = Mathf.Clamp(players[1] - 1, 0, 100);
                break;
            case "Player 3":
                players[2] = Mathf.Clamp(players[2] - 1, 0, 100);
                break;
            case "Player 4":
                players[3] = Mathf.Clamp(players[3] - 1, 0, 100);
                break;
        }
    }*/

    private void CheckWinner()
    {
        bool win = false;
        bool draw = false;
        int bestPlayerId = 0;
        int bestPoint = 0;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] >= PointsMax)
            {
                win = true;
                if (bestPoint < players[i])
                {
                    draw = false;
                    bestPlayerId = i + 1;
                    bestPoint = players[i];
                }
                else if (bestPoint == players[i])
                {
                    draw = true;
                }
            }
        }

        if (win)
        {
            if (!draw)
            {
                print("We have a winner");
                if(_winner.Value == bestPlayerId)
                {
                    SetWinnerClientRpc();
                }
                else _winner.Value = bestPlayerId;
                
            }
            else
            {
                print("We have a draw");
                ContinueMatchClientRpc();
            }
        }
       
        else
        {
            print("Nobody win the game continues");
            ContinueMatchClientRpc();
        }
    }

    [ClientRpc]
    private void ContinueMatchClientRpc()
    {
        StartCoroutine(ContinueMatch());
    }

    [ClientRpc]
    private void SetWinnerClientRpc()
    {
        StartCoroutine(SetWinner());
    }

    IEnumerator ContinueMatch()
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(0.67f);
        pointsScreem.SetActive(false);
        
        EventManager.Instance.OnPlayersReady();

        yield return new WaitForSeconds(1f);

        transition.SetActive(false);
    }

    IEnumerator SetWinner()
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(0.67f);
        pointsScreem.SetActive(false);
        EventManager.Instance.OnPlayerWin("Player " + _winner.Value);

        yield return new WaitForSeconds(1f);

        transition.SetActive(false);
    }

    private void UpdatePoints()
    {
        var p1 = EventManager.Instance.GetPlayer1Kills();
        var p2 = EventManager.Instance.GetPlayer2Kills();
        var p3 = EventManager.Instance.GetPlayer3Kills();
        var p4 = EventManager.Instance.GetPlayer4Kills();

        PlayersScore players = new PlayersScore();
        players.player1 = new int[p1.Count];
        players.player2 = new int[p2.Count];
        players.player3 = new int[p3.Count];
        players.player4 = new int[p4.Count];

        for(int i = 0; i < p1.Count; i++)
        {
            players.player1[i] = p1[i];
        }

        for (int i = 0; i < p2.Count; i++)
        {
            players.player2[i] = p2[i];
        }
        for (int i = 0; i < p3.Count; i++)
        {
            players.player3[i] = p3[i];
        }
        for (int i = 0; i < p4.Count; i++)
        {
            players.player4[i] = p4[i];
        }


        UpdateScoreUIClientRpc();
        
        _playerScore.Value = players;

        StartCoroutine(WaitToContinue());
    }

    [ClientRpc]
    private void UpdateScoreUIClientRpc()
    {
        UpdateScoreUI(_playerScore.Value, _playerScore.Value);
    }

    IEnumerator WaitToContinue()
    {
        yield return new WaitForSeconds(3f);

        CheckWinner();
    }

    private void ResetGamePoints(string player)
    {
        players = new int[4];
    }

    private void OnEnable()
    {
        EventManager.EndMatch += UpdatePoints;
        EventManager.PlayerWin += ResetGamePoints;
        _playerScore.OnValueChanged += UpdateScoreUI;
        _winner.OnValueChanged += SetWinner;
    }

    private void SetWinner(int previousValue, int newValue)
    {
        StartCoroutine(SetWinner());
    }

    private void UpdateScoreUI(PlayersScore previousValue, PlayersScore newValue)
    {
        var p1 = _playerScore.Value.player1;
        players[0] = p1.Length;
        var p2 = _playerScore.Value.player2;
        players[1] = p2.Length;
        var p3 = _playerScore.Value.player3;
        players[2] = p3.Length;
        var p4 = _playerScore.Value.player4;
        players[3] = p4.Length;

        pointsScreem.SetActive(true);

        for (int i = 0; i < PointsMax; i++)
        {
            if (i <= p1.Length - 1)
            {
                player1Points[i].sprite = killSprite[p1[i]];
            }
            else
            {
                player1Points[i].sprite = baseSprite;
            }

            if (i <= p2.Length - 1)
            {
                player2Points[i].sprite = killSprite[p2[i]];
            }
            else
            {
                player2Points[i].sprite = baseSprite;
            }

            if (i <= p3.Length - 1)
            {
                player3Points[i].sprite = killSprite[p3[i]];
            }
            else
            {
                player3Points[i].sprite = baseSprite;
            }

            if (i <= p4.Length - 1)
            {
                player4Points[i].sprite = killSprite[p4[i]];
            }
            else
            {
                player4Points[i].sprite = baseSprite;
            }
        }
    }

    private void OnDisable()
    {
        EventManager.EndMatch -= UpdatePoints;
        EventManager.PlayerWin -= ResetGamePoints;
        _playerScore.OnValueChanged -= UpdateScoreUI;
        _winner.OnValueChanged -= SetWinner;
    }

}
