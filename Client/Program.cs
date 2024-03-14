using Client.Models;
using Client.Services;
using System.Net.Sockets;
using System.Text;

using TcpClient tcpClient = new TcpClient();
await tcpClient.ConnectAsync("127.0.0.1", 2510);
//await tcpClient.ConnectAsync("127.0.0.1", 8888);
var stream = tcpClient.GetStream();
int bytesRead = 10;
var response = new List<byte>();
bool flag = true;

string json_string = string.Empty;
int code = 0;
var payrolls = new List<PayrollSheet>();


while (flag)
{

    flag = Menu.MainMenu(out json_string, payrolls, out code);

    if (code != -1)
    {
        byte[] data = Encoding.UTF8.GetBytes(json_string + '\n');

        // Sending data
        await stream.WriteAsync(data);

        // Reading data to the last character
        while ((bytesRead = stream.ReadByte()) != '\n')
        {
            // Adding to buffer
            response.Add((byte)bytesRead);
        }
        var translation = Encoding.UTF8.GetString(response.ToArray());

        Menu.OutputMenu(out payrolls, translation);

        response.Clear();
    }
}

await stream.WriteAsync(Encoding.UTF8.GetBytes("END\n"));

