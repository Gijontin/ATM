using System.Security.Cryptography;

namespace ATM;

class Program {
    
    //deklaera en variable för aktuellt saldo
    static int balance = 0;
    static string amount = "";


    static void Main() {
        
        Account newAccount = new Account();

        while (true) {
            
            Console.Clear();
            newAccount.ReportBalance();
            
            //insättning
            Console.WriteLine("Hur mycket vill du sätta in?");
            newAccount.balance = newAccount.Deposit(Console.ReadLine());
            break;
/*
            //deklarera en variable för insättning
            //var amount = Console.ReadLine(); //tryck enter för fortsättning
            amount = Console.ReadLine();
            try {
                balance += int.Parse(amount);
                break;
            }
            catch {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOgiltliga tecken, försök igen...");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(1000);
            }
*/
        }
/*
        if (string.IsNullOrWhiteSpace(amount)) {
            Console.WriteLine("DU MÅSTE ANGE HUR MYCKET DU VILL SÄTTA IN...");
            Environment.Exit(0);
        }
        balance = int.Parse(amount);
        ReportBalance();
*/

        Console.WriteLine("Hur mycket vill du ta ut?");
        amount = Console.ReadLine();
        Console.WriteLine($"Du vill ta ut {amount} Kr");

        //balance = balance - int.Parse(amount); fult och dåligt sätt
        balance -= int.Parse(amount);            //snyggt och pro sätt

        newAccount.ReportBalance();

        /*
        
        kontrollera att man inte tar ut 0

        du ska ej kunna bli minus på saldot av uttag
        eller ta ut mer än vad saldot innehåller

        skriv funktion som visar vad saldot innehåller istället för en massa console lines

        */
    }
}