using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Text.Json;
using System.IO;

namespace GeminiCS {
    class App {
        public static void Main() {
            bool döngü = true;
            ConsoleKeyInfo keyInfo;
            while (döngü) {
                Account Account = new Account(App.Set_Username(), App.Set_Password());
                Console.WriteLine(")=================(");
                Console.WriteLine($"Username = {Account._Username}");
                Console.WriteLine($"Password = {Account._Password}");
                Console.WriteLine($"Balance = {Account._Balance} $");
                Console.WriteLine(")=================(");
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape) döngü = false;
            }

            Console.ReadKey();
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

    class Account {
        private string m_USERNAME;
        private string m_PASSWORD;
        private int m_BALANCE;

        public Account(string username, string password) {
            Random rnd = new Random();
            m_BALANCE = rnd.Next(1000, 5001);
            m_USERNAME = username;
            m_PASSWORD = password;
        }

        public string _Username {
            get { return m_USERNAME; }
            set { m_USERNAME = value; }
        }

        public string _Password {
            get { return m_PASSWORD; }
            set { m_PASSWORD = value; }
        }

        public int _Balance {
            get { return m_BALANCE; }
        }
    }
}


