using Client.Models;
using System.Text.Json;

namespace Client.Services
{
    public static class Menu
    {
        public static bool MainMenu(out string json_string, List<PayrollSheet> list, out int code)
        {
            code = 0;
            json_string = string.Empty;

            Console.WriteLine("1 - Create a new record on the list");
            Console.WriteLine("2 - Update a record");
            Console.WriteLine("3 - Delete a record");
            Console.WriteLine("4 - Find a record");
            Console.WriteLine("5 - Sort all records");
            Console.WriteLine("6 - Output records");
            Console.WriteLine("7 - Exit");
            int select = Verification.InputInt("Your Choice\n");

            switch (select)
            {
                case 1:
                    PayrollSheet create = PayrollSheetService.Create();
                    string json_create = JsonSerializer.Serialize(create);
                    json_string = JsonSerializer.Serialize(new Request()
                    {
                        JsonData = json_create,
                        Code = 1
                    });

                    return true;
                case 2:
                    if (list.Count > 0)
                    {
                        Console.WriteLine("Searching the worker for the update");
                        var sel = PayrollSheetService.Select(list);
                        PayrollSheet update = PayrollSheetService.Update(sel);
                        string json_update = JsonSerializer.Serialize(update);
                        json_string = JsonSerializer.Serialize(new Request() { JsonData = json_update, Code = 2 });
                    }
                    else
                    {
                        Console.WriteLine("First load the list to update the worker");
                        code = -1;
                    }

                    return true;
                case 3:
                    if (list.Count > 0)
                    {
                        Console.WriteLine("Searching the worker for the deletion");
                        var selected = PayrollSheetService.Select(list);
                        string json_delete = JsonSerializer.Serialize(selected);
                        json_string = JsonSerializer.Serialize(new Request() { JsonData = json_delete, Code = 3 });
                    }
                    else
                    {
                        Console.WriteLine("First load the list to delete the worker");
                        code = -1;
                    }

                    return true;
                case 4:
                    Console.WriteLine("Search worker");
                    string fullName = Verification.InputString("Worker fullname");
                    json_string = JsonSerializer.Serialize(new Request() { FullName = fullName, Code = 4 });
                    return true;
                case 5:
                    Console.WriteLine("Sort record list");
                    json_string = JsonSerializer.Serialize(new Request() { Code = 5 });
                    return true;
                case 6:
                    if (list is null || list.Count > 0)
                    {

                        var table = new Table(
                            "Record output",
                            new string[4]
                            {
                                "WorkShop",
                                "FullName",
                                "ScopeCompletedWork",
                                "AccuredEarnings",
                            },
                            new int[4]
                            {
                                30, 30, 50, 25
                            });
                        table.Hat();
                        foreach (PayrollSheet view in list)
                        {
                            table.Body(new object[4]
                            {
                                view.WorkShop,
                                view.FullName,
                                view.ScopeCompletedWork,
                                view.AccuredEarnings,
                            });
                        }
                        table.Bottom();
                        code = -1;
                    }
                    else
                    {
                        Console.WriteLine("Reading records from the server");
                        json_string = JsonSerializer.Serialize(new Request
                        {
                            Code = 6
                        });
                    }
                    return true;
                case 7:
                    Console.WriteLine("Exit -See you");
                    code = -1;
                    return false;
                default:
                    return false;

            }
        }


        public static void OutputMenu(out List<PayrollSheet> list, string json_string)
        {
            list = new List<PayrollSheet>();
            var obj = JsonSerializer.Deserialize<Request>(json_string);
            if ((obj.Code == 1 || obj.Code == 2 || obj.Code == 3))
            {
                Console.WriteLine(obj.JsonData);
            }
            else if (obj.Code == 5 || obj.Code == 6)
            {
                string data = obj.Code == 5
                    ? "Sorted Data"
                    : "Unsorted Data";
                Console.WriteLine(data);
                var lst = JsonSerializer.Deserialize<List<PayrollSheet>>(obj.JsonData);
                list = lst;
            }
            else
            {
                var find = JsonSerializer.Deserialize<List<PayrollSheet>>(obj.JsonData);
                list = find;

                var table = new Table(
                             "Record Output",
                             new string[4]
                             {
                                "WorkShop",
                                "FullName",
                                "ScopeCompletedWork",
                                "AccuredEarnings",
                             },
                             new int[4]
                             {
                                30, 30, 50, 25
                             });
                table.Hat();
                foreach (PayrollSheet view in find)
                {
                    table.Body(new object[4]
                    {
                                view.WorkShop,
                                view.FullName,
                                view.ScopeCompletedWork,
                                view.AccuredEarnings,
                    });
                }
                table.Bottom();
            }
        } 
    }
}
