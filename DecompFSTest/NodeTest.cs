using DecompFS.filesystem;
using DecompFS.filesystem.tree;

namespace DecompFSTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void EmptyNodeShouldBeRoot()
    {
        Node n = new();
        Assert.That(n.type, Is.EqualTo(NodeType.Root));
    }
    
    [Test]
    public void FileNodeShouldHaveFileData()
    {
        VirtualFile file = new VirtualFile(1234, "TestFile", "this/is/a/test/relative/path", null);
        Node n = new(file);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(n.type, Is.EqualTo(NodeType.File));
            Assert.That(n.fileData, Is.EqualTo(file));
        }
    }
    
    [Test]
    public void FileNodeShouldNotHaveChildren()
    {
        VirtualFile file = new VirtualFile(1234, "TestFile", "this/is/a/test/relative/path", null);
        Node n = new(file);

        Assert.That(n.children, Is.Null);
    }

    [Test]
    public void ChildNodeNesting()
    {
        Node root = new();
        Node child1 = new("Child1", NodeType.Folder);
        Node child2 = new("Child2", NodeType.Folder);
        
        root.addChild(child1);
        child1.addChild(child2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(root.children, Is.Not.Null);
            Assert.That(root.children, Has.Length.EqualTo(1));
            Assert.That(root.children[0], Is.EqualTo(child1));
            
            Assert.That(child1.children, Is.Not.Null);
            Assert.That(child1.children, Has.Length.EqualTo(1));
            Assert.That(child1.children[0], Is.EqualTo(child2));
            
            Assert.That(child2.children, Is.Null);
        }
    }
}