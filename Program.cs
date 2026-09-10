//namespace ATM;

class Program {
    static void Main() {
        /*
            MENU:
            1. Login To Account. //id to account, then pin to get into it
            2. Create New Account. //take user input data to send into factory, factory then both creates account adds it to the dictionary list used in login to account
            3. Exit.
        
        */
        AccountFactory AF = new();

        //TEST: KONTOSKAPARMENU 
        Menu.mainMenu();

        //Account newAccount = new Account();

        Account newAccount = AF.SkapaAccount(AccountType.SPARKONTO, "Bullen Grodtvätt", 6969, 0m);
        Menu.accountMenu(newAccount);
    }
}