using System.Net;
using System.Net.Sockets;

namespace SpostaPallinaTcpServer;

public partial class frmServer : Form
{
    private TcpListener? _tcpListener;
    private readonly List<Client> _clients = new List<Client>();

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
        for (int index = 0; index < _clients.Count;)
        {
            Client client = _clients[index];
            if (client.TcpClient.Connected == false)
            {
                _clients.RemoveAt(index);
                continue;
            }

            try
            {
                var message = $"{e.X},{e.Y}";
                client.StreamWriter.WriteLine(message);
                index++;
            }
            catch
            {
                client.StreamWriter.Close();
                client.NetworkStream.Close();
                client.TcpClient.Close();
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
}