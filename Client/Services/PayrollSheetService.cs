using Client.Models;

namespace Client.Services
{
    public static class PayrollSheetService
    {
        public static PayrollSheet Create()
        {
            var payrollSheet = new PayrollSheet();

            payrollSheet.WorkShop = Verification.InputString("Enter the worshop name");
            payrollSheet.FullName = Verification.InputString("Enter the worker fullname");
            payrollSheet.ScopeCompletedWork = Verification.InputDouble("Enter the scope completed work");
            payrollSheet.UnitPrice = Verification.InputDouble("Enter the unit price");
            payrollSheet.AccuredEarnings = Verification.InputDouble("Enter Accrued earnings"); 

            return payrollSheet;
        }

        public static PayrollSheet Update(PayrollSheet note)
        {
            bool flag = true;
            PayrollSheet update = note;

            while (flag)
            {
                Console.WriteLine("1 - Update scope completed work");
                Console.WriteLine("2 - Update unit price");
                Console.WriteLine("3 - Update accured earnings");
                Console.WriteLine("4 - Exit");
                int select = Verification.InputInt("Your Choice?");

                switch (select)
                {
                    case 1:
                        update.ScopeCompletedWork = Verification.InputDouble("Enter the scope of completed work");
                        break;
                    case 2:
                        update.UnitPrice = Verification.InputDouble("Enter units price");
                        break;
                    case 3:
                        update.AccuredEarnings = Verification.InputDouble("Enter accured earnings");
                        break;
                    case 4:
                        flag = false;
                        break;
                    default:
                        flag = false;
                        break;
                }
            }

            return update;
        }
        public static PayrollSheet Select(List<PayrollSheet> notes)
        {
            PayrollSheet? note = null; 
            bool flag = true;

            while (flag)
            {
                string payroll = Verification.InputString("Enter fullname");
                var find = notes.Find(x => x.FullName == payroll);

                if (find != null)
                {
                    note = find;
                    flag = false;
                }
                else
                {
                    Console.WriteLine("A record with the entered fullname doesn't exist"); 
                }
            }

            return note;
        }

    }
}
