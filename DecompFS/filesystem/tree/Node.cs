using System.Buffers;

namespace DecompFS.filesystem.tree;

public class Node
{
    private int id;
    public string path;

    public NodeType type;
    public Node[]? children;

    public VirtualFile? fileData;
    
    public void setFileData(VirtualFile fileData)
    {
        if (type != NodeType.File)
            throw new InvalidOperationException("Cannot set file data on a non-file node.");

        this.fileData = fileData;
    }
    
    public void addChild(Node children) {
        if (type is not (NodeType.Root or NodeType.Folder))
            throw new InvalidOperationException("Cannot add a child to a non-folder node.");

        // Only folders can have children, so this node is now a child
        this.type = NodeType.Folder;
        
        // If this is the first child, create a new children array
        if (this.children == null)
            this.children = [children];
        else
        {
            // Create a new array with one more slot than the current children array
            var newChildren = ArrayPool<Node>.Shared.Rent(this.children.Length + 1);
            
            // Copy the existing children to the new array  
            this.children.CopyTo(newChildren, 0);
            
            // Add the new child to the end of the new array
            newChildren[^1] = children;
            
            // Replace the old children array with the new one
            this.children = newChildren;
            ArrayPool<Node>.Shared.Return(newChildren);
        }
    }

    public Node()
    {
        this.type = NodeType.Root;
    }
    
    public Node(VirtualFile file, NodeType type = NodeType.File)
    {
        this.type = NodeType.File;
        this.fileData = file;
    }
    
    public Node(string folderName, NodeType type = NodeType.Folder) {
        this.type = type;
        this.path = Path.Combine(Path.GetFullPath(folderName));
    }
}