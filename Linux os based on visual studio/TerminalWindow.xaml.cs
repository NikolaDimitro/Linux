using System.Windows;
using System.Windows.Input;

namespace LinuxMintOS
{
    public partial class TerminalWindow : Window
    {
        public TerminalWindow()
        {
            InitializeComponent();
            TerminalOutput.Text = "Welcome to Linux Mint Terminal Simulator\nType 'help' for available commands.\n\n";
        }

        private void CommandInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string command = CommandInput.Text.Trim();
                ProcessCommand(command);
                CommandInput.Clear();
                e.Handled = true;
            }
        }

        private void ProcessCommand(string command)
        {
            TerminalOutput.AppendText($"user@mint:~$ {command}\n");

            switch (command.ToLower())
            {
                case "help":
                    TerminalOutput.AppendText("Available commands:\n  help - Show this help\n  date - Show current date/time\n  pwd - Print working directory\n  echo - Echo text\n  clear - Clear screen\n\n");
                    break;
                case "date":
                    TerminalOutput.AppendText($"{System.DateTime.Now}\n\n");
                    break;
                case "pwd":
                    TerminalOutput.AppendText("/home/user/documents\n\n");
                    break;
                case "clear":
                    TerminalOutput.Clear();
                    break;
                default:
                    if (command.StartsWith("echo "))
                    {
                        TerminalOutput.AppendText(command.Substring(5) + "\n\n");
                    }
                    else
                    {
                        TerminalOutput.AppendText($"Command not found: {command}\nType 'help' for available commands.\n\n");
                    }
                    break;
            }

            TerminalOutput.ScrollToEnd();
        }
    }
}