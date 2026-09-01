using System.Security.Cryptography;

namespace ATM;

class Program {

    static void Main() {
        
        Account newAccount = new Account();

        Menu.drawMenu(newAccount);
    }
}