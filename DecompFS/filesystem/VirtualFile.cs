using DecompFS.filesystem.tree;

namespace DecompFS.filesystem;

public struct VirtualFile
{
    // File offset, if applicable.
    public ulong? Offset;
    
    // Path to the source file on disk
    // If the file's representation is on a container, this is the path to the container file.
    // If the file is not on a container, this is the path to the file itself.
    public string? FileContainerPath;

    public string FileName;
    public string FilePath;
    public string FileExt;
    
    // File size
    public ulong Size;

    public bool IsInContainer;

    // todo: compression and encryption should probably reference functions that do the comp/decomp and enc/dec
    public bool IsCompressed;
    public bool IsEncrypted;
    
    /// <summary>
    /// Constructor for objects that are not inside containers
    /// </summary>
    /// <param name="size">The size of the object.</param>
    /// <param name="name">The name of the object.</param>
    /// <param name="path">The path of the object.</param>
    /// <param name="ext">The extension of the object. Optional.</param>
    public VirtualFile(ulong size, string name, string path, string? ext)
    {
        FileContainerPath = path;
        Size = size; 
        IsInContainer = false;
        
        FileName = name;
        FilePath = path;
        FileExt = ext ?? "";
        
        IsCompressed = false;
        IsEncrypted = false;
    }

    /// <summary>
    /// Constructor for objects that are inside containers.
    /// </summary>
    /// <param name="offset">The object offset within the specified container</param>
    /// <param name="size">The object size within the specified container</param>
    /// <param name="fileContainerPath">The path of the container this object is in</param>
    /// <param name="name">The name of the object specified in the container</param>
    /// <param name="path">The path of the object specified in the container</param>
    /// <param name="ext">The file extension of the object. Optional, unless the container preserves file extensions</param>
    public VirtualFile(ulong offset, ulong size, string fileContainerPath, string name, string path, string? ext)
    {
        Offset = offset;
        FileContainerPath = fileContainerPath;
        Size = size; 
        IsInContainer = true;
        
        FileName = name;
        FilePath = path;
        FileExt = ext ?? "";
        
        IsCompressed = false;
        IsEncrypted = false;
    }
}