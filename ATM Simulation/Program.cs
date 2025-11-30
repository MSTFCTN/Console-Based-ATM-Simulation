using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

namespace GeminiCS {
    class App {
        public static void Main() {
            JSON_Operations.read_from_JSON();
            Console.WriteLine("Dosya Yolu: " + Path.GetFullPath("Accounts.json"));
            Console.WriteLine();
            Show_Accounts();

            bool döngü = true;
            ConsoleKeyInfo keyInfo;
            while (döngü) {
                Account Account = new Account(App.Set_Username(), App.Set_Password());

                Console.Clear();
                
                Console.WriteLine($"Username = {Account.Username}");
                Console.WriteLine($"Password = {Account.Password}");
                Console.WriteLine($"Balance = {Account.Balance} $");
                
                Accounts.Add(Account);
                JSON_Operations.write_to_JSON();
                
                Console.WriteLine("\r\r\r\r\r\r\rPress ESC to exit");
                
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape) döngü = false;
                
                Console.Clear();
            }



            Console.ReadKey();
        }

        public static List<Account> Accounts = new List<Account>();

        public static void Show_Accounts() {             
            foreach (Account show_accounts in Accounts) {
                if (show_accounts.Username != "0" || show_accounts.Password != "0") {
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







        public static string Set_Username() {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            return username;
        }


        public static string Set_Password() {
            Console.Write("Password: ");

            List<char> _PASSWORD_ = new List<char>();

            int lastIndexNumber;
            char pass_letters;
            bool password_input = true;
            ConsoleKeyInfo keyInfo;
            while (password_input) {

                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter) {
                    password_input = false;
                    Console.WriteLine();
                }
                else if (keyInfo.Key == ConsoleKey.Backspace) {
                    if (_PASSWORD_.Count > 0) {
                        lastIndexNumber = _PASSWORD_.Count - 1;
                        _PASSWORD_.RemoveAt(lastIndexNumber);
                        Console.Write("\b \b");
                    }
                }
                else if (char.TryParse(char.ToString(keyInfo.KeyChar), out pass_letters)) {
                    _PASSWORD_.Add(pass_letters);
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
    }
}


