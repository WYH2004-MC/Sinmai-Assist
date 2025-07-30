using System;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.WebApi;
using MelonLoader;
using SinmaiAssist.WebServer.Api;

namespace SinmaiAssist.WebServer;


public class WebServer
{
    private EmbedIO.WebServer _server;
    private readonly string _host;
    private readonly int _port;
    private readonly string _token;
    private readonly string _url;
    private bool _isRunning = false;
    
    public WebServer(string host, int port, string token)
    {
        _host = host;
        _port = port;
        _token = token;
        _url = $"http://{_host}:{_port}/";
        if (!string.IsNullOrEmpty(_token))
            _url += $"{_token}/";
    }

    public void Start()
    {
        try
        {
            _server = new EmbedIO.WebServer(o => o
                    .WithUrlPrefix(_url)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/api", m => m
                    .WithController<ApiController>())
                .WithStaticFolder("/", "Frontend", true);

            Task.Run(() => _server.RunAsync());
            _isRunning = true;
            MelonLogger.Msg($"[WebServer] WebServer started with: {_url}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[WebServer] Failed to start WebServer: {ex}");
            throw;
        }
    }
    
    public void Stop()
    {
        _server?.Dispose();
        _isRunning = false;
        MelonLogger.Msg($"[WebServer] WebServer stopped");
    }
    
    public bool IsRunning()
    {
        return _server != null && _isRunning;
    }
}
