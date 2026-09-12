//namespace ATM;

class Program {
    static void Main() {
        Menu menu = new();
        /*
            MENU:
            1. Login To Account. //id to account, then pin to get into it
            2. Create New Account. //take user input data to send into factory, factory then both creates account adds it to the dictionary list used in login to account
            3. Exit.
        
            Använd mejladress som Dictionary key och sedan lagra Account...
        */
        //AccountFactory AF = new();

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

        menu.mainMenu();
        //TEST: KONTOSKAPARMENU 
        //Menu.mainMenu();

        KontoData kd_placeholder = new();
        Account acn_placeholder = new(kd_placeholder);
        //Account newAccount = AF.SkapaAccount(AccountType.SPARKONTO, "Bullen Grodtvätt", 6969, 0m);
        //Menu.accountMenu(acn_placeholder);
    }
}