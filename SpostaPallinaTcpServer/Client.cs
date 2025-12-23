using System.Net.Sockets;

namespace SpostaPallinaTcpServer;

internal class Client
{
    public TcpClient? TcpClient { get; set; }
    public NetworkStream? NetworkStream { get; set; }
    public StreamWriter? StreamWriter { get; set; }
}