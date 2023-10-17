using ServerConnectionApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public TMP_Text TotalWin;
    [Range(30, 1000)] public int ResultDelayInMs = 100;
    private SemaphoreSlim _sem = new(1);
    private CancellationTokenSource _tkc = new();
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        _tkc.Cancel();
    }
    public async void PlayBtnClick()
    {
        try
        {
            await _sem.WaitAsync();
            
            if (_tkc.IsCancellationRequested) return;

            if (float.TryParse(AmountInput.text, out var amount))
            {
                var result = await GameServerConnection.Instance.Play(amount);

                RemoveOldBoardIfExists();

                await PrintBoard(result.Board.ToList());

                TotalWin.text = $"Your Win : {result.TotalWin}";
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
        finally
        {
            _sem.Release();
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
    private readonly static Dictionary<Rank, int> _scores = new()
        {
            {Rank.Ace,5 },
            {Rank.King,4 },
            {Rank.Queen,3 },
            {Rank.Jack,2 },
            {Rank.Zombie,1 },
        };
    private async Task PrintBoard(List<CardDto> cards)
    {
        foreach (var card in cards)
        {
            var pos = new Vector3(card.Position.X, card.Position.Y, 0);

            Card c = Instantiate(CardPrefab, pos, Quaternion.identity);

            c.SetParent(gameObject);

            var text = card.Rank.ToString();

            if (card.IsInWinningSetup)
            {
                var winText = string.Concat(text, " ", _scores[card.Rank]);
                c.SetText(winText);
                c.SetWinMaterial();
            }
            else
            {
                c.SetText(text);
            }

            _currentBoard.Add(c);

            await Task.Delay(ResultDelayInMs);
        }
    }
}
