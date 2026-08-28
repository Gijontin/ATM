using System.Security.Cryptography;

namespace ATM;

class Program {

    static void Main() {
        
        Account newAccount = new Account();

        Menu.drawMenu(newAccount);

        while (true) {
            
            Console.Clear();
            newAccount.ReportBalance();
            
            //insättning
            Console.WriteLine("Hur mycket vill du sätta in?"); //lägg in detta i deposit funktionen om vi ska göra menyer sen
            newAccount.Deposit(Console.ReadLine());
            break;
        }

        while (true) {

            Console.Clear();
            newAccount.ReportBalance();

            //uttag
            Console.WriteLine("Hur mycket vill du ta ut?"); //lägg in detta i withdraw funktionen om vi ska göra menyer sen
            newAccount.Withdraw(Console.ReadLine());
            break;
        }
    }
}