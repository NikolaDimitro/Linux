# Linux Mint OS Simulator
# Read the installation instructions,if you want to try it!!



A complete operating system simulator built with **C#** and **WPF (.NET Framework)** in Visual Studio. This project replicates the Linux Mint desktop experience with a functional GUI that includes file management, terminal, text editor, and system settings.

## Features

✅ **Desktop Environment** - Linux Mint-themed green interface (#4CAF50, #2d5016)  
✅ **Taskbar** - Bottom taskbar with menu, application launcher, and system tray  
✅ **System Clock** - Real-time clock display  
✅ **File Manager** - Browse directories, create folders, view files  
✅ **Terminal Simulator** - Linux-like terminal with built-in commands (help, date, pwd, echo, clear)  
✅ **Text Editor** - Create, open, and save text files  
✅ **Settings Panel** - Display, sound, and system information settings  

## Project Structure

```
LinuxMintOS/
├── App.xaml / App.xaml.cs
├── MainWindow.xaml / MainWindow.xaml.cs (Main desktop)
├── FileManagerWindow.xaml / FileManagerWindow.xaml.cs
├── TerminalWindow.xaml / TerminalWindow.xaml.cs
├── SettingsWindow.xaml / SettingsWindow.xaml.cs
└── TextEditorWindow.xaml / TextEditorWindow.xaml.cs
```

## System Requirements

- Visual Studio 2019 or later
- .NET Framework 4.7.2 or higher
- Windows OS, MacOs or Linux

## Installation & Setup

1. **Create a new WPF Application (.NET Framework)** in Visual Studio
2. **Replace or add the XAML and C# files** from the project
3. **Update App.xaml** with the styling resources
4. **Add all window classes** (FileManagerWindow, TerminalWindow, SettingsWindow, TextEditorWindow)
5. **Build the solution** (Ctrl+Shift+B)
6. Install Microsoft.Web.WebView2 - click right button on the project, click Manage NuGet packages and copy the name
7. **Run the application** (F5)

## Main Features Explained

### Desktop
- Walpaper background 
- Quick-access buttons for File Manager, Terminal, Settings, Text Editor and Browser
- Fully windowed application with customizable resolution

### Taskbar
- **Menu Button** - System menu with application information
- **Application Launcher** - Quick access to open applications
- **System Tray** - Real-time clock, volume, battery, and network indicators
- Dark background 

### File Manager
- Browse local file system
- Create new folders
- Refresh folder contents
- Home directory navigation
- File and folder listing

### Terminal
- Green-on-black retro terminal design
- Built-in commands: `help`, `date`, `pwd`, `echo`, `clear`
- Command history
- Real-time command execution feedback

### Text Editor
- New, Open, Save file functionality
- Simple but functional text editing
- Supports .txt files and all file types
- Courier New font for code-like appearance

### Settings Panel
- Display settings (resolution, refresh rate)
- Sound controls with volume slider
- System information and version details
- Theme and animation toggles

## Browser
- fully working browser with serach machine
- you can open every site (if you do the right setup)

## Color Scheme

The application uses Linux Mint's signature colors:
- **Primary Green**: #4CAF50 (buttons, accents)
- **Dark Green**: #2d5016 (background)
- **Dark UI**: #1a3a0b (taskbar)
- **Terminal**: #000000 background with #00FF00 text

## Terminal Commands

```
help          - Display available commands
date          - Show current date and time
pwd           - Print working directory
echo TEXT     - Echo/print text
clear         - Clear terminal screen
```

## How It Works

1. **MainWindow.xaml.cs** - Controls the main desktop, taskbar, and window management
2. **FileManagerWindow** - Provides file browsing and folder creation using System.IO
3. **TerminalWindow** - Simulates a Linux terminal with command parsing and execution
4. **TextEditorWindow** - Full-featured text editor with file I/O operations
5. **SettingsWindow** - Displays system configuration and settings

## Future Enhancements

- 🔜 Actual file system integration for real file operations
- 🔜 Window drag-and-drop support
- 🔜 Application shortcuts on desktop
- 🔜 Sound effects and notifications
- 🔜 Network information integration
- 🔜 Multiple workspace support
- 🔜 Customizable themes and wallpapers

## License

This project is open source and available for educational purposes.

## Author

**Nikola Dimitro** - Created as a Linux Mint OS simulator in C# WPF

---

**Note**: This is a simulator/mockup of Linux Mint OS. It is not a real operating system but rather a GUI application that replicates the look and feel of Linux Mint desktop environment.
