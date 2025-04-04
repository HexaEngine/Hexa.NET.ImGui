using Hexa.NET.ImGui;
using System.Numerics;

namespace ExampleFramework.ImGuiDemo
{
    public class OpenFileDialog
    {
        private bool shown;
        private DirectoryInfo currentDir;
        private readonly List<Item> files = new();
        private readonly List<Item> dirs = new();
        public string RootFolder;
        private string currentFolder;
        private string selectedFile = string.Empty;
        public List<string> AllowedExtensions = new();
        public bool OnlyAllowFolders;
        public bool OnlyAllowFilteredExtensions;
        public OpenFileResult Result;

        private readonly Stack<string> backHistory = new();
        private readonly Stack<string> forwardHistory = new();

        private struct Item
        {
            public string Path;
            public string Filename;
            public string Name;

            public Item(string name, string filename, string path)
            {
                Name = name;
                Filename = filename;
                Path = path;
            }
        }

#pragma warning disable CS8618 // Non-nullable field 'currentFolder' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.

        public OpenFileDialog()
#pragma warning restore CS8618 // Non-nullable field 'currentFolder' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        {
            string startingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (File.Exists(startingPath))
            {
                startingPath = Path.GetDirectoryName(startingPath) ?? string.Empty;
            }
            else if (string.IsNullOrEmpty(startingPath) || !Directory.Exists(startingPath))
            {
                startingPath = Environment.CurrentDirectory;
                if (string.IsNullOrEmpty(startingPath))
                {
                    startingPath = AppContext.BaseDirectory;
                }
            }

            currentDir = new DirectoryInfo(startingPath);
            RootFolder = startingPath;
            CurrentFolder = startingPath;
            OnlyAllowFolders = false;
        }

#pragma warning disable CS8618 // Non-nullable field 'currentFolder' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.

        public OpenFileDialog(string startingPath)
#pragma warning restore CS8618 // Non-nullable field 'currentFolder' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        {
            if (File.Exists(startingPath))
            {
                startingPath = Path.GetDirectoryName(startingPath) ?? string.Empty;
            }
            else if (string.IsNullOrEmpty(startingPath) || !Directory.Exists(startingPath))
            {
                startingPath = Environment.CurrentDirectory;
                if (string.IsNullOrEmpty(startingPath))
                {
                    startingPath = AppContext.BaseDirectory;
                }
            }

            currentDir = new DirectoryInfo(startingPath);
            RootFolder = startingPath;
            CurrentFolder = startingPath;
            OnlyAllowFolders = false;
        }

#pragma warning disable CS8618 // Non-nullable field 'currentFolder' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.

        public OpenFileDialog(string startingPath, string? searchFilter = null)
#pragma warning restore CS8618 // Non-nullable field 'currentFolder' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        {
            if (File.Exists(startingPath))
            {
                startingPath = Path.GetDirectoryName(startingPath) ?? string.Empty;
            }
            else if (string.IsNullOrEmpty(startingPath) || !Directory.Exists(startingPath))
            {
                startingPath = Environment.CurrentDirectory;
                if (string.IsNullOrEmpty(startingPath))
                {
                    startingPath = AppContext.BaseDirectory;
                }
            }

            if (searchFilter != null)
            {
                if (AllowedExtensions != null)
                {
                    AllowedExtensions.Clear();
                }
                else
                {
                    AllowedExtensions = new List<string>();
                }

                AllowedExtensions.AddRange(searchFilter.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
            }

            currentDir = new DirectoryInfo(startingPath);
            RootFolder = startingPath;
            CurrentFolder = startingPath;
            OnlyAllowFolders = false;
        }

        public bool Shown => shown;

        public string FullPath
        {
            get => Path.Combine(currentFolder, selectedFile);
            set
            {
                currentFolder = Path.GetDirectoryName(value) ?? string.Empty;
                selectedFile = Path.GetFileName(value);
            }
        }

        public string SelectedFile { get => selectedFile; set => selectedFile = value; }

        public string CurrentFolder
        {
            get => currentFolder;
            set
            {
                currentFolder = value;
                Refresh();
            }
        }

        public void Show()
        {
            shown = true;
        }

        public void Hide()
        {
            shown = false;
        }

        public unsafe bool Draw()
        {
            if (!shown)
            {
                return false;
            }

            if (ImGui.Begin("File picker", ref shown, ImGuiWindowFlags.NoDocking))
            {
                ImGui.SetWindowFocus();
                if (ImGui.Button("\xe80f"))
                {
                    CurrentFolder = RootFolder;
                }
                ImGui.SameLine();
                if (ImGui.Button("\xe72b"))
                {
                    TryGoBack();
                }
                ImGui.SameLine();
                if (ImGui.Button("\xe72a"))
                {
                    TryGoForward();
                }
                ImGui.SameLine();
                if (ImGui.Button("\xe72c"))
                {
                    Refresh();
                }
                ImGui.SameLine();
                ImGui.InputText("Path", ref currentFolder, 1024);

                float footerHeightToReserve = ImGui.GetStyle().ItemSpacing.Y + ImGui.GetFrameHeightWithSpacing();

                float widthDrives = 100 + ImGui.GetStyle().ItemSpacing.X * 2;
                float width = ImGui.GetContentRegionAvail().X - ImGui.GetStyle().ItemSpacing.X - widthDrives;
                if (ImGui.BeginChild(1, new Vector2(widthDrives, -footerHeightToReserve), ImGuiWindowFlags.HorizontalScrollbar))
                {
                    void Display(string? rel, string str)
                    {
                        if (File.Exists(str))
                        {
                            return;
                        }

                        if (ImGui.TreeNodeEx(rel != null ? Path.GetRelativePath(rel, str) : str, ImGuiTreeNodeFlags.OpenOnArrow))
                        {
                            if (Directory.Exists(str))
                            {
                                foreach (var item in GetFileSystemEntries(str))
                                {
                                    Display(str, item);
                                }
                            }

                            ImGui.TreePop();
                        }
                        if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
                        {
                            SetFolder(str);
                        }
                    }
                    if (ImGui.TreeNodeEx("Computer", ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.DefaultOpen))
                    {
                        string profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        foreach (var drive in GetSpecialDirs())
                        {
                            Display(profilePath, drive);
                        }
                        foreach (var drive in GetDrives())
                        {
                            Display(null, drive);
                        }
                        ImGui.TreePop();
                    }
                }
                ImGui.EndChild();

                ImGui.SameLine();
                if (ImGui.BeginChild(2, new Vector2(width, -footerHeightToReserve), ImGuiChildFlags.Borders, 0))
                {
                    if (currentDir.Exists)
                    {
                        if (currentDir.Parent != null)
                        {
                            ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1.0f, 0.87f, 0.37f, 1.0f));
                            if (ImGui.Selectable("../", false, ImGuiSelectableFlags.NoAutoClosePopups))
                            {
                                SetFolder(currentDir.Parent.FullName);
                            }

                            ImGui.PopStyleColor();
                        }

                        for (int i = 0; i < dirs.Count; i++)
                        {
                            var dir = dirs[i];
                            ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1.0f, 0.87f, 0.37f, 1.0f));
                            if (ImGui.Selectable(dir.Name, false, ImGuiSelectableFlags.NoAutoClosePopups))
                            {
                                SetFolder(dir.Path);
                            }

                            ImGui.PopStyleColor();
                        }

                        for (int i = 0; i < files.Count; i++)
                        {
                            var file = files[i];

                            bool isSelected = selectedFile == file.Path;
                            if (ImGui.Selectable(file.Name, isSelected, ImGuiSelectableFlags.NoAutoClosePopups))
                            {
                                selectedFile = file.Filename;
                            }

                            if (ImGui.IsItemClicked(0) && ImGui.IsMouseDoubleClicked(0))
                            {
                                Result = OpenFileResult.Ok;
                                ImGui.EndChild();
                                ImGui.End();
                                shown = false;
                                return true;
                            }
                        }
                    }
                }
                ImGui.EndChild();

                ImGui.InputText("Selected", ref selectedFile, 1024);

                ImGui.SameLine();
                if (ImGui.Button("Cancel"))
                {
                    Result = OpenFileResult.Cancel;
                    ImGui.End();
                    shown = false;
                    return true;
                }

                if (OnlyAllowFolders)
                {
                    ImGui.SameLine();
                    if (ImGui.Button("Open"))
                    {
                        Result = OpenFileResult.Ok;
                        selectedFile = CurrentFolder;
                        ImGui.End();
                        shown = false;
                        return true;
                    }
                }
                else if (selectedFile != null)
                {
                    ImGui.SameLine();
                    if (ImGui.Button("Open"))
                    {
                        Result = OpenFileResult.Ok;
                        ImGui.End();
                        shown = false;
                        return true;
                    }
                }
            }

            ImGui.End();
            return false;
        }

        public void SetFolder(string path)
        {
            backHistory.Push(CurrentFolder);
            CurrentFolder = path;
            forwardHistory.Clear();
        }

        public void GoHome()
        {
            CurrentFolder = RootFolder;
        }

        public void TryGoBack()
        {
            if (backHistory.TryPop(out var historyItem))
            {
                forwardHistory.Push(CurrentFolder);
                CurrentFolder = historyItem;
            }
        }

        public void TryGoForward()
        {
            if (forwardHistory.TryPop(out var historyItem))
            {
                backHistory.Push(CurrentFolder);
                CurrentFolder = historyItem;
            }
        }

        private static string[] GetDrives()
        {
            return Directory.GetLogicalDrives();
        }

        private static string[] GetSpecialDirs()
        {
            return new string[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
            };
        }

        public void Refresh()
        {
            currentDir = new DirectoryInfo(CurrentFolder);
            files.Clear();
            dirs.Clear();

            foreach (var fse in Directory.GetFileSystemEntries(currentFolder, string.Empty))
            {
                if (File.GetAttributes(fse).HasFlag(FileAttributes.System))
                {
                    continue;
                }

                if (File.GetAttributes(fse).HasFlag(FileAttributes.Hidden))
                {
                    continue;
                }

                if (File.GetAttributes(fse).HasFlag(FileAttributes.Device))
                {
                    continue;
                }

                if (Directory.Exists(fse))
                {
                    dirs.Add(new("\xe8b7" + Path.GetFileName(fse), Path.GetFileName(fse), fse));
                }
                else if (!OnlyAllowFolders)
                {
                    if (OnlyAllowFilteredExtensions)
                    {
                        var ext = Path.GetExtension(fse);
                        if (AllowedExtensions.Contains(ext))
                        {
                            files.Add(new("\xe8a5" + Path.GetFileName(fse), Path.GetFileName(fse), fse));
                        }
                    }
                    else
                    {
                        files.Add(new("\xe8a5" + Path.GetFileName(fse), Path.GetFileName(fse), fse));
                    }
                }
            }
        }

        private List<string> GetFileSystemEntries(string fullName)
        {
            var files = new List<string>();
            var dirs = new List<string>();
            foreach (var fse in Directory.GetFileSystemEntries(fullName, string.Empty))
            {
                if (File.GetAttributes(fse).HasFlag(FileAttributes.System))
                {
                    continue;
                }

                if (File.GetAttributes(fse).HasFlag(FileAttributes.Hidden))
                {
                    continue;
                }

                if (File.GetAttributes(fse).HasFlag(FileAttributes.Device))
                {
                    continue;
                }

                if (Directory.Exists(fse))
                {
                    dirs.Add(fse);
                }
                else if (!OnlyAllowFolders)
                {
                    if (OnlyAllowFilteredExtensions)
                    {
                        var ext = Path.GetExtension(fse);
                        if (AllowedExtensions.Contains(ext))
                        {
                            files.Add(fse);
                        }
                    }
                    else
                    {
                        files.Add(fse);
                    }
                }
            }

            var ret = new List<string>(dirs);
            ret.AddRange(files);

            return ret;
        }
    }
}