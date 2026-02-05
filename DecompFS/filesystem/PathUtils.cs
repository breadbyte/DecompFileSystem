namespace DecompFS.filesystem;

public static class PathUtils
{
    public static string[] GetSegments(string path)
    {
        // C:/Users/Path/To/File.txt should return ["C:", "Users", "Path", "To", "File.txt"]
        return path.Split(['/', '\\'], StringSplitOptions.RemoveEmptyEntries);
    }
}