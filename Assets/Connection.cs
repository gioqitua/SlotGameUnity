using Connection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GameServerConnection : MonoBehaviour
{
    private Connector Connector;
    private CancellationTokenSource _tkc = new();
    public int PlayDelayInMs = 500;

    async void Start()
    {
        Connector = new Connector("http://localhost:7215", "token");

        while (!_tkc.IsCancellationRequested)
        {
            await Play(1f);

            await Task.Delay(PlayDelayInMs);
        }

    }

    void OnDisable()
    {
        _tkc.Cancel();
    }

    public async Task Play(float amount)
    {
        try
        {
            var result = await Connector.Api().PlayAsync(amount);

            Debug.Log("Win => " + result.TotalWin);
        }
        catch
        {
            Debug.Log("Error");
        }
    }

}
