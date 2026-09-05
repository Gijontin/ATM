namespace ATM;

class Program {

    static void Main() {
        
        Account newAccount = new Account();
        List<string> newTransaktioner = new List<string>();

        Menu.drawMenu(newAccount);
    }
}