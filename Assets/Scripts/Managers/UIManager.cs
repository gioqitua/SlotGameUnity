using ServerConnectionApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_InputField AmountInput;
    public Card CardPrefab;
    private List<Card> _currentBoard = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    public async void PlayBtnClick()
    {
        try
        {
            if (float.TryParse(AmountInput.text, out var amount))
            {
                var result = await GameServerConnection.Instance.Play(amount);

                RemoveOldBoardIfExists();

                await PrintBoard(result.Board.ToList());

                Debug.Log(result.TotalWin);
            }
            else
            {
                Debug.LogWarning("Wrong Bet Amount");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void RemoveOldBoardIfExists()
    {
        if (_currentBoard.Any())
        {
            foreach (var c in _currentBoard)
            {
                if (c.isActiveAndEnabled)
                {
                    Destroy(c.gameObject);
                }
            }
            _currentBoard.Clear();
        }
    }

    private async Task PrintBoard(List<CardDto> cards)
    {
        foreach (var card in cards)
        {
            var pos = new Vector3(card.Position.X, card.Position.Y, 0);

            Card c = Instantiate(CardPrefab, pos, Quaternion.identity);

            c.SetParent(gameObject);

            c.SetText(card.Rank.ToString());

            if (card.IsInWinningSetup)
            {
                c.SetWinMaterial();
            }

            _currentBoard.Add(c);

            await Task.Delay(50);
        }
    }
}
