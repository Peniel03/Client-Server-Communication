using Server.Models;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server.Service
{
    public static class RequestMenu
    {
        private static readonly object fileLock = new object();
        public static async Task<string> Menu(string json_string, TcpClient client)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true
            };

            var obj = JsonSerializer.Deserialize<Request>(json_string);

            string str = "";

            string newJsonString = await SwitchMeny(client, obj, options, str);
            return newJsonString;
        }

        private static async Task<string> SwitchMeny(TcpClient client, Request obj,
            JsonSerializerOptions options, string str) => obj.Code switch
            {
                1 => await CreateAsync(client, obj, options, str),
                2 => await UpdateAsync(client, obj, options, str),
                3 => await DeleteAsync(client, obj, options, str),
                4 => await FindAsync(client, obj, str),
                5 => await SortAsync(client, obj, options, str),
                6 => await ReadAsync(client, obj, str),
                _ => ""
            };

        private static async Task<string> CreateAsync(TcpClient client, Request obj,
            JsonSerializerOptions options, string str)
        {
            Console.WriteLine($"Client {client.Client.RemoteEndPoint} Adding an element operation");
            var payroll = JsonSerializer.Deserialize<PayrollSheet>(obj.JsonData);

            lock (fileLock)
            {
                using (StreamReader sr = new StreamReader("payrollsheet.json"))
                {
                    str = sr.ReadToEndAsync().Result;
                }
            }

            var events = JsonSerializer.Deserialize<List<PayrollSheet>>(str);
            events.Add(payroll);
            var json = JsonSerializer.Serialize(events, options);

            lock (fileLock)
            {
                using (StreamWriter streamWriter = new StreamWriter("payrollsheet.json", false, Encoding.UTF8))
                {
                    streamWriter.WriteLineAsync(json);
                }
            }

            string newJsonString = JsonSerializer.Serialize(new Request()
            {
                JsonData = "Successfully Added",
                Code = 1
            });

            return newJsonString;
        }

        private static async Task<string> UpdateAsync(TcpClient client, Request obj,
            JsonSerializerOptions options, string str)
        {
            Console.WriteLine($"Client {client.Client.RemoteEndPoint} Updating an element operation");

            var payroll = JsonSerializer.Deserialize<PayrollSheet>(obj.JsonData);

            lock (fileLock)
            {
                using (StreamReader streamReader = new StreamReader("payrollsheet.json")) 
                {
                    str = streamReader.ReadToEndAsync().Result;
                }
            }
            var list = JsonSerializer.Deserialize<List<PayrollSheet>>(str);
            list.RemoveAll(x => x.FullName == payroll.FullName);
            list.Add(payroll);
            var str_list = JsonSerializer.Serialize(list, options);

            lock (fileLock)
            {
                using (StreamWriter streamWriter = new StreamWriter("event.json", false, Encoding.UTF8))
                {
                    streamWriter.WriteLineAsync(str_list);
                }
            }

            string newJsonString = JsonSerializer.Serialize(new Request()
            {
                JsonData = "Успешно обновлено",
                Code = 2
            });

            return newJsonString;
        }

        private static async Task<string> DeleteAsync(TcpClient client, Request obj,
            JsonSerializerOptions options, string str)
        {
            Console.WriteLine($"Клиент {client.Client.RemoteEndPoint} выполняет операцию удаления элемента");

            var offense = JsonSerializer.Deserialize<PayrollSheet>(obj.JsonData);

            lock (fileLock)
            {
                using (StreamReader sr = new StreamReader("event.json"))
                {
                    str = sr.ReadToEndAsync().Result;
                }
            }

            var lst = JsonSerializer.Deserialize<List<PayrollSheet>>(str);
            lst.Remove(offense);
            var str_lst = JsonSerializer.Serialize(lst, options);

            lock (fileLock)
            {
                using (StreamWriter sw = new StreamWriter("event.json", false, Encoding.UTF8))
                {
                    sw.WriteLineAsync(str_lst);
                }
            }

            string newJsonString = JsonSerializer.Serialize(new Request()
            {
                JsonData = "Успешно удалено",
                Code = 3
            });

            return newJsonString;
        }

        private static async Task<string> FindAsync(TcpClient client, Request obj,
            string str)
        {
            Console.WriteLine($"Клиент {client.Client.RemoteEndPoint} выполняет операцию поиска элементов");

            lock (fileLock)
            {
                using (StreamReader sr = new StreamReader("event.json"))
                {
                    str = sr.ReadToEndAsync().Result;
                }
            }

            var data = JsonSerializer.Deserialize<List<PayrollSheet>>(str);
            var find = data.Where(x => x.FullName == obj.FullName).ToList();
            var findStr = JsonSerializer.Serialize(find);

            string newJsonString = JsonSerializer.Serialize(new Request()
            {
                JsonData = findStr,
                Code = 4
            });

            return newJsonString;
        }

        private static async Task<string> SortAsync(TcpClient client, Request obj,
           JsonSerializerOptions options, string str)
        {
            Console.WriteLine($"Клиент {client.Client.RemoteEndPoint} выполняет операцию сортировки массива");

            lock (fileLock)
            {
                using (StreamReader sr = new StreamReader("event.json"))
                {
                    str = sr.ReadToEndAsync().Result;
                }
            }

            var payrolls = JsonSerializer.Deserialize<List<PayrollSheet>>(str);
            payrolls = payrolls.OrderBy(x => x.ScopeCompletedWork).ToList();

            var str_notes = JsonSerializer.Serialize(payrolls, options);
            string newJsonString = JsonSerializer.Serialize(new Request()
            {
                JsonData = str_notes,
                Code = 5
            });

            return newJsonString;
        }

        private static async Task<string> ReadAsync(TcpClient client, Request obj,
           string str)
        {
            Console.WriteLine($"Клиент {client.Client.RemoteEndPoint} выполняет операцию считывания списка");

            lock (fileLock)
            {
                using (StreamReader sr = new StreamReader("payrollsheet.json"))
                {
                    str = sr.ReadToEndAsync().Result;

                }
            }

            var offences = JsonSerializer.Deserialize<List<PayrollSheet>>(str);
            var str_nots = JsonSerializer.Serialize(offences);
            string newJsonString = JsonSerializer.Serialize(new Request()
            {
                JsonData = str_nots,
                Code = 6
            }
            );

            return newJsonString;
        }
    }
}
