using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading;


namespace GeminiCS {
    class App {
        public static List<Account> Accounts = new List<Account>();
        public static Account Account;

        public static void Main() {
            Console.CursorVisible = false;                      

            ConsoleKeyInfo keyInfo;

            bool program = true;
            while (program) {
                JSON_Operations.read_from_JSON();
                int main_select;
                main_select = Menu.MainMenu();

                if (main_select == 0) {
                    int login_select;
                    bool login;
                    while (!(login = Login())) {
                        Console.WriteLine("Wrong username or password!");
                        Console.WriteLine("Press any key to retry or press ESC to return main menu");
                        keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Escape) break;
                    }

                    while (login) {
                        Console.Clear();
                        Console.WriteLine($"Welcome {Account.Username} !");
                        Console.WriteLine($"Your Balance : {Account.Balance}");
                        Console.WriteLine();

                        login_select = Menu.AccountMenu();

                        // Deposit Money
                        while (login_select == 0) {
                            Console.Clear();
                            Console.WriteLine("Please place your cash neatly into the slot!");
                            Console.WriteLine("Press ENTER when you done or press ESC to return account menu!");
                            keyInfo = Console.ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.Enter) {
                                Console.Clear();
                                Console.Write("Your money is counting");
                                Thread.Sleep(1000);
                                Console.Write(".");
                                Thread.Sleep(1000);
                                Console.Write(".");
                                Thread.Sleep(1000);
                                Console.Write(".");
                                Thread.Sleep(1000);

                                Random rnd = new Random();
                                int add_balance;
                                add_balance = rnd.Next(1, 1001);

                                Console.Clear();
                                Account.Balance += add_balance;
                                JSON_Operations.write_to_JSON();
                                Console.WriteLine($"{add_balance}$ succesfully added to your balance");
                                Console.WriteLine("Thank you for choosing us <3");
                                Thread.Sleep(1000);
                                break;
                            }
                            else if (keyInfo.Key == ConsoleKey.Escape) break;
                        }


                        // Withdraw Money
                        while (login_select == 1) {
                            Console.Clear();
                            Console.WriteLine("(Enter 0 to back account menu!)");
                            Console.Write("Please enter amount : ");

                            string input = Console.ReadLine();
                            int amount;
                            while (!int.TryParse(input, out amount) || amount > Account.Balance || amount < 0) {
                                Console.Clear();
                                Console.WriteLine("Please enter a valid amount!");
                                Console.Write(": ");
                                input = Console.ReadLine();
                            }

                            if (amount == 0) break;

                            Console.Clear();
                            Console.Write("Your money is preparing");
                            Thread.Sleep(1000);
                            Console.Write(".");
                            Thread.Sleep(1000);
                            Console.Write(".");
                            Thread.Sleep(1000);
                            Console.Write(".");
                            Thread.Sleep(1000);

                            Console.Clear();
                            Console.WriteLine("Please take your money in the slot!");
                            Console.WriteLine("Press ENTER when you done");
                            keyInfo = Console.ReadKey();

                            while (keyInfo.Key != ConsoleKey.Enter) {
                                keyInfo = Console.ReadKey();
                            }

                            Console.Clear();
                            Account.Balance -= amount;
                            JSON_Operations.write_to_JSON();
                            Console.WriteLine("Thank you for choosing us <3");
                            Thread.Sleep(1000);
                            break;
                        }

                        if (login_select == 2) break;
                    }
                }

                else if (main_select == 1) {
                    bool signup = SignUp();
                    while (!signup) ;
                }

                else if (main_select == 2) break;
            }
        }

        
        public static bool Login() {
            string username = null;
            string password = null;
            int login = 0;

            Console.Clear();

            Console.Write("Username: ");
            while (string.IsNullOrEmpty(username = Console.ReadLine())) {
                Console.Clear();
                Console.Write("Username: ");
            }

            password = Set_Password();

            foreach(Account acc in Accounts) {
                if (acc.Username == username && acc.Password == password) {
                    Account = acc;
                    return true;
                }
            }

            return false;
        }

        public static bool SignUp() {
            ConsoleKeyInfo keyInfo;
            Console.Clear();
            Account Account = new Account(App.Set_Username(), App.Set_Password());
            Console.WriteLine("Press ESC if you want to back main menu or press any key continue");
            keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Escape) return true;
            else {
                Console.Clear();
                Console.WriteLine("Your account is preparing");
                Thread.Sleep(1000);
                Console.Write(".");
                Thread.Sleep(1000);
                Console.Write(".");
                Thread.Sleep(1000);
                Console.Write(".");
                Thread.Sleep(1000);
                Accounts.Add(Account);
                JSON_Operations.write_to_JSON();
                Console.Clear();
                Console.WriteLine("Your account has been created succesfuly. Press any key to back main menu...");
                keyInfo = Console.ReadKey();
                if (keyInfo != null) return true;
                else return false;
            }
        }


        public static string Set_Username() {
            string username = null;
            bool IsUsernameValid = false;

            while (!IsUsernameValid) {
                Console.Write("Username: ");

                while (string.IsNullOrEmpty(username = Console.ReadLine())) {
                    Console.Clear();
                    Console.WriteLine("PLease Enter a Valid Username!");
                    Console.WriteLine();
                    Console.Write("Username: ");
                }

                if (Accounts.Count == 0) break;

                foreach (Account acc in Accounts) {
                    if (acc == null || Accounts is null) IsUsernameValid = true;
                    else if (acc.Username == username) {
                        Console.WriteLine(" ERROR! This Username Already Exist!");
                        Console.WriteLine();
                        Thread.Sleep(1000);
                        Console.Clear();
                        IsUsernameValid = false;
                        break;
                    }
                    else IsUsernameValid = true;                    
                }
            }
            return username;
        }


        public static string Set_Password() {
            Console.Write("Password: ");

            List<char> _PASSWORD_ = new List<char>();

            int lastIndexNumber;
            bool password_input = true;
            ConsoleKeyInfo keyInfo;
            while (password_input) {

                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter) {
                    if(_PASSWORD_.Count > 0) {
                        password_input = false;
                        Console.WriteLine();
                    }                    
                }
                else if (keyInfo.Key == ConsoleKey.Backspace) {
                    if (_PASSWORD_.Count > 0) {
                        lastIndexNumber = _PASSWORD_.Count - 1;
                        _PASSWORD_.RemoveAt(lastIndexNumber);
                        Console.Write("\b \b");
                    }
                }
                else if(!char.IsControl(keyInfo.KeyChar)) {
                    _PASSWORD_.Add(keyInfo.KeyChar);
                    Console.Write("*");
                }                
            }

            string password = new string(_PASSWORD_.ToArray());//password = string.Join(",", _PASSWORD_);

            return password;
        }
        
    }

    class JSON_Operations {
        public static void write_to_JSON() {
            var settings = new JsonSerializerOptions { WriteIndented = true };
            string jsonText = JsonSerializer.Serialize(App.Accounts, settings);

            File.WriteAllText("Accounts.json", jsonText);
        }

        public static void read_from_JSON() {
            if (!File.Exists("Accounts.json")) {
                return;
            }

            string jsonText = File.ReadAllText("Accounts.json");
            if (!string.IsNullOrEmpty(jsonText)) {
                App.Accounts = JsonSerializer.Deserialize<List<Account>>(jsonText);
                Console.WriteLine("Old Records Uploaded!");
            }
        }
    }

    class Account {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }

        public Account(string username, string password) {
            Random rnd = new Random();
            Balance = rnd.Next(1000, 5001);
            Username = username;
            Password = password;
        }

        public static void Show_Accounts() {
            foreach (Account show_accounts in App.Accounts) {
                if (show_accounts.Username != null || show_accounts.Password != null) {
                    Console.WriteLine("\r\r");
                    Console.WriteLine(")=================(");
                    Console.WriteLine($"Username = {show_accounts.Username}");
                    Console.WriteLine($"Password = {show_accounts.Password}");
                    Console.WriteLine($"Balance = {show_accounts.Balance} $");
                    Console.WriteLine(")=================(");
                    Console.WriteLine("\r\r");
                }
            }
        }
    }

    class Menu {
        public static int MainMenu() {

            ConsoleKeyInfo keyInfo;
            bool menu = true;
            int select = 0;
            int CursorLeft = 28, CursorTop = 0;

            Console.Clear();
            Console.WriteLine("1 - Login To Your Account");
            Console.WriteLine("2 - Sign up (Free Balance)");
            Console.WriteLine("3 - Exit");

            while (menu) {
                Console.SetCursorPosition(CursorLeft, CursorTop);
                Console.Write("<<");

                keyInfo = Console.ReadKey(true);

                if ((keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.W) && select > 0) {
                    Console.SetCursorPosition(CursorLeft, CursorTop);
                    Console.Write("  ");
                    CursorTop--;
                    select--;
                    Console.SetCursorPosition(CursorLeft, CursorTop);

                }
                else if ((keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.S) && select < 2) {
                    Console.SetCursorPosition(CursorLeft, CursorTop);
                    Console.Write("  ");
                    CursorTop++;
                    select++;
                    Console.SetCursorPosition(CursorLeft, CursorTop);
                }
                else if (keyInfo.Key == ConsoleKey.Enter) break;
            }

            return select;
        }

        public static int AccountMenu() {

            ConsoleKeyInfo keyInfo;
            bool menu = true;
            int select = 0;
            int CursorLeft = 19, CursorTop = 3;

            Console.WriteLine("1 - Deposit Money");
            Console.WriteLine("2 - Withdraw Money");
            Console.WriteLine("3 - Back");

            while (menu) {
                Console.SetCursorPosition(CursorLeft, CursorTop);
                Console.Write("<<");

                keyInfo = Console.ReadKey(true);

                if ((keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.W) && select > 0) {
                    Console.SetCursorPosition(CursorLeft, CursorTop);
                    Console.Write("  ");
                    CursorTop--;
                    select--;
                    Console.SetCursorPosition(CursorLeft, CursorTop);

                }
                else if ((keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.S) && select < 2) {
                    Console.SetCursorPosition(CursorLeft, CursorTop);
                    Console.Write("  ");
                    CursorTop++;
                    select++;
                    Console.SetCursorPosition(CursorLeft, CursorTop);
                }
                else if (keyInfo.Key == ConsoleKey.Enter) break;
            }

            return select;
        }


    }
}


