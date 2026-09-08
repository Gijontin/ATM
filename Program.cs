//namespace ATM;

class Program {
    static void Main() {
        AccountFactory AF = new();
        //Account newAccount = new Account();
        Account newAccount = AF.SkapaAccount(AccountType.SPARKONTO, "Bullen Grodtvött", 6969, 0m);
        Menu.drawMenu(newAccount);
    }
}