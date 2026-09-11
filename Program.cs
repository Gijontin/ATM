//namespace ATM;

class Program {
    static void Main() {
        /*
            MENU:
            1. Login To Account. //id to account, then pin to get into it
            2. Create New Account. //take user input data to send into factory, factory then both creates account adds it to the dictionary list used in login to account
            3. Exit.
        
            Använd mejladress som Dictionary key och sedan lagra Account...
        */
        AccountFactory AF = new();

        //test av funktion
        
/*
        if (genUtil.testaValideraPIN(out int sant)) {
            Console.WriteLine("Lyckat!");
            Thread.Sleep(2500);
        } else {
            Console.WriteLine("Misslyckat");
            Thread.Sleep(2500);
        }
*/

        //TEST: KONTOSKAPARMENU 
        Menu.mainMenu();

        //Account newAccount = new Account();

        Account newAccount = AF.SkapaAccount(AccountType.SPARKONTO, "Bullen Grodtvätt", 6969, 0m);
        Menu.accountMenu(newAccount);
    }
}