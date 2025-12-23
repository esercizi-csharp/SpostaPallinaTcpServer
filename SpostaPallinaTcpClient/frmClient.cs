using System.Net.Sockets;

namespace SpostaPallinaTcpClient;

public partial class frmClient : Form
{
    public frmClient()
    {
        InitializeComponent();
    }

    private async void btnConnect_Click(object sender, EventArgs e)
    {
        string host = txtServerAddress.Text;
        int port = 1234;

        using TcpClient client = new TcpClient();
        await client.ConnectAsync(host, port);
        await using NetworkStream networkStream = client.GetStream();
        using StreamReader reader = new StreamReader(networkStream);
        while (true)
        {
            string? receivedMessage = await reader.ReadLineAsync();
            if (receivedMessage == null)
                break;

            string[] parts = receivedMessage.Split(',');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int x) &&
                int.TryParse(parts[1], out int y))
            {
                lblBall.Left = x;
                lblBall.Top = y;
            }
        }
    }
}