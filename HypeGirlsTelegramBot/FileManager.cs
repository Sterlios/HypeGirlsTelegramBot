
using System;
using System.IO;

public class FileManager
{
    private static readonly object _lock = new object();
    private static FileManager _fileManager;
    private static Random _random = new Random();

    private string[] _welcomePictures;
    private string _picturesPath;
    private string[] _directories;

    private FileManager() { }

    public static FileManager GetInstance(string welcomePicturesPath, string picturesPath)
    {
        if (_fileManager is null)
            lock (_lock)
                if (_fileManager == null)
                    _fileManager = new FileManager();

        _fileManager._picturesPath = picturesPath;
        _fileManager._welcomePictures = Directory.GetFiles(welcomePicturesPath);
        _fileManager._directories = Directory.GetDirectories(picturesPath);

        return _fileManager;
    }

    public FileInfo GetWelcomePicture()
    {
        int pictureNumber = _random.Next(_welcomePictures.Length);
        FileInfo fileInfo = new FileInfo(_welcomePictures[pictureNumber]);

        return fileInfo;
    }

    public bool TryGetPicture(string name, out FileInfo fileInfo)
    {
        fileInfo = null;
        string directory = $"{_picturesPath}\\{name}\\";

        if (IsCorrectName(directory))
        {
            string[] files = Directory.GetFiles($"{_picturesPath}\\{name}\\");
            int pictureNumber = _random.Next(files.Length);
            fileInfo = new FileInfo(files[pictureNumber]);
            return true;
        }

        return false;
    }

    private bool IsCorrectName(string directoryName)
    {
        foreach (var directory in _directories)
        {
            if (directory.ToLower().Equals(directoryName.ToLower()))
                return true;
        }

        return false;
    }
}
