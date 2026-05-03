# Linux
This is a mini linux based on visual studio
# MiniLinux (C# / Visual Studio)

This is a mini Linux shell simulator written in **C#** and suitable for working in **Visual Studio 2022**.

## What you get

- In-memory file system (no real disk access)
- Linux-like commands: `ls`, `cd`, `pwd`, `cat`, `touch`, `mkdir`, `rm`, `echo`, `history`, `clear`, `exit`
- Ready-made project that you can run immediately

## File structure

- `MiniLinux/MiniLinux.csproj` – .NET project
- `MiniLinux/Program.cs` – entry point
- `MiniLinux/MiniShell.cs` – parsing and executing commands
- `MiniLinux/MiniFileSystem.cs` – virtual file system logic
- `MiniLinux/FileSystemNode.cs` – base class for node
- `MiniLinux/DirectoryNode.cs` – directory
- `MiniLinux/FileNode.cs` – file

## Launch in Visual Studio

1. Open Visual Studio 2022.
2. `File -> Open -> Project/Solution`.
3. Select `MiniLinux/MiniLinux.csproj`.
4. Press `F5` (or `Ctrl+F5`) to start.

## Start from terminal

```bash
dotnet run --project MiniLinux/MiniLinux.csproj
```

## Sample commands

```text
pwd
ls
cat welcome.txt
mkdir projects
cd projects
touch todo.txt
echo "learn C#" > todo.txt
cat todo.txt
history
```

## Upgrade ideas

- Support for `cp`, `mv`, `find`
- Permissions (`rwx`)
- Users (`root`, `user`)
