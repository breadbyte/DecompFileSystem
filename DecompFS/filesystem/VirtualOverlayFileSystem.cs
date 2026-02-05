using DecompFS.filesystem.tree;

namespace DecompFS.filesystem;

public class VirtualOverlayFileSystem
{
    private bool _mutable = true;
    private Node rootFs = new Node();
    
    public bool CreateFile(string path, VirtualFile file)
    {
        if (!_mutable)
            throw new InvalidOperationException("Cannot create file on a read-only filesystem.");

        __InternalCreateFile(path, file);
        return false;
    }
    
    public bool GetDirectory(string path, out Node directory)
    {
        var pathSegments = PathUtils.GetSegments(path);
        var currentNode = rootFs;

        for (int i = 0; i < pathSegments.Length; i++)
        {
            var segment = pathSegments[i];

            if (currentNode.children == null)
            {
                directory = default;
                return false;
            }

            var nextNode = currentNode.children.FirstOrDefault(n => n.path == segment);

            if (nextNode == null || nextNode.type != NodeType.Folder)
            {
                directory = default;
                return false;
            }

            if (i == pathSegments.Length - 1)
            {
                directory = nextNode;
                return true;
            }
            else
            {
                currentNode = nextNode;
            }
        }

        directory = default;
        return false;
    }
    
    public bool GetFile(string path, out VirtualFile file)
    {
        var pathSegments = PathUtils.GetSegments(path);
        var currentNode = rootFs;

        for (int i = 0; i < pathSegments.Length; i++)
        {
            var segment = pathSegments[i];

            if (currentNode.children == null)
            {
                file = default;
                return false;
            }

            var nextNode = currentNode.children.FirstOrDefault(n => n.path == segment);

            if (nextNode == null)
            {
                file = default;
                return false;
            }

            if (i == pathSegments.Length - 1)
            {
                if (nextNode.type != NodeType.File)
                {
                    file = default;
                    return false;
                }

                file = nextNode.fileData.Value;
                return true;
            }
            else
            {
                if (nextNode.type != NodeType.Folder)
                {
                    file = default;
                    return false;
                }

                currentNode = nextNode;
            }
        }

        file = default;
        return false;
    }

    private bool __InternalCreateFile(string path, VirtualFile file)
    {
        var pathSegments = PathUtils.GetSegments(path);
        var currentNode = rootFs;

        if (pathSegments.Length == 1) {
            // Single segment, create file node directly under root
            // todo: check if node with the same name already exists
            // todo: overwrite if file, do nothing if folder exists
            
            rootFs.addChild(new Node(file));
        }
        
        for (int i = 0; i < pathSegments.Length; i++)
        {
            var segment = pathSegments[i];

            if (i == pathSegments.Length - 1)
            {
                // Last segment, create file node
                currentNode.addChild(new Node(file));
                return true;
            }
            else
            {
                // Intermediate segment, create folder node if it doesn't exist
                var folderNode = new Node(segment);
                currentNode.addChild(folderNode);
                currentNode = folderNode;
            }
        }

        return false;
    }
}