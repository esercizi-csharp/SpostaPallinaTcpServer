using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SpostaPallinaTcpServer;

public partial class frmServer : Form
{
    private TcpListener? _tcpListener;
    private List<Client> _clients = new List<Client>();

    public frmServer()
    {
        InitializeComponent();
    }

    private void frmServer_Load(object sender, EventArgs e)
    {
        StartTcpServer();
    }

    private void frmServer_MouseClick(object? sender, MouseEventArgs e)
    {
        Log($"Mouse click at {e.X},{e.Y}");
        for (int index = 0; index < _clients.Count;)
        {
            Client client = _clients[index];
            if (client.TcpClient.Connected == false)
            {
                Log("Client disconnected, removing from list");
                _clients.RemoveAt(index);
                continue;
            }

            try
            {
                var message = $"{e.X},{e.Y}";
                client.StreamWriter.WriteLine(message);
                index++;
            }
            catch (Exception ex)
            {
                Log($"Error sending to client: {ex.Message}. Removing from list");
                _clients.RemoveAt(index);
            }
        }
    }

    private void StartTcpServer()
    {
        int port = 1234;
        _tcpListener = new TcpListener(IPAddress.Any, port);
        _tcpListener.Start();
        ListenForClients();
    }

    private async void ListenForClients()
    {
        while (_tcpListener != null)
        {
            var tcpClient = await _tcpListener.AcceptTcpClientAsync();
            Log("New client connected");
            var networkStream = tcpClient.GetStream();
            var streamWriter = new StreamWriter(networkStream) { AutoFlush = true };
            var client = new Client
            {
                TcpClient = tcpClient,
                NetworkStream = networkStream,
                StreamWriter = streamWriter
            };
            _clients.Add(client);
        }
    }


    private void Log(string content)
    {
        Debug.WriteLine(content);
    }
}