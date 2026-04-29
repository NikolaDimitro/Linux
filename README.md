# Linux
This is a mini linux based on visual studio
# MiniLinux (C# / Visual Studio)

Това е мини Linux shell симулатор, написан на **C#** и подходящ за работа във **Visual Studio 2022**.

## Какво получаваш

- In-memory файлова система (без реален достъп до диска)
- Команди като в Linux: `ls`, `cd`, `pwd`, `cat`, `touch`, `mkdir`, `rm`, `echo`, `history`, `clear`, `exit`
- Готов проект, който можеш да стартираш веднага

## Структура на файловете

- `MiniLinux/MiniLinux.csproj` – .NET проект
- `MiniLinux/Program.cs` – входна точка
- `MiniLinux/MiniShell.cs` – парсване и изпълнение на команди
- `MiniLinux/MiniFileSystem.cs` – логика на виртуалната файлова система
- `MiniLinux/FileSystemNode.cs` – базов клас за node
- `MiniLinux/DirectoryNode.cs` – директория
- `MiniLinux/FileNode.cs` – файл

## Стартиране във Visual Studio

1. Отвори Visual Studio 2022.
2. `File -> Open -> Project/Solution`.
3. Избери `MiniLinux/MiniLinux.csproj`.
4. Натисни `F5` (или `Ctrl+F5`) за старт.

## Стартиране от терминал

```bash
dotnet run --project MiniLinux/MiniLinux.csproj
```

## Примерни команди

```text
pwd
ls
cat welcome.txt
mkdir projects
cd projects
touch todo.txt
echo "учи C#" > todo.txt
cat todo.txt
history
```

## Идеи за надграждане

- Поддръжка на `cp`, `mv`, `find`
- Права за достъп (`rwx`)
- Потребители (`root`, `user`)
- Pipe оператори (`|`) между команди
- Запазване на файловата система в JSON
