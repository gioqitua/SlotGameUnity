using Connection;
using ServerConnectionApi;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GameServerConnection : MonoBehaviour
{
    public static GameServerConnection Instance;
    private Connector Connector;
    private CancellationTokenSource _tkc = new();
    public int PlayDelayInMs = 500;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    async void Start()
    {
        Connector = new Connector("http://localhost:7215", "token");

        //while (!_tkc.IsCancellationRequested)
        //{
        //    await Play(1f);

        //    await Task.Delay(PlayDelayInMs);
        //}
    }

    void OnDisable()
    {
        _tkc.Cancel();
    }

    public async Task<ResultDto> Play(float amount) => await Connector.Api().PlayAsync(amount);

}
