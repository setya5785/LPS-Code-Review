var rootNode = new TreeNode();
int loopCount = 0;

while (true)
{
    //periodically prune tree after 100 loop / iteration
    if (loopCount % 100 == 0)
    {
        rootNode.ClearChildren();
    }

    // create a new subtree of 10000 nodes  
    var newNode = new TreeNode();
    for (int i = 0; i < 10000; i++)
    {
        var childNode = new TreeNode();
        newNode.AddChild(childNode);
    }
    rootNode.AddChild(newNode);
}

class TreeNode
{
    private readonly LinkedList<TreeNode> _children = new LinkedList<TreeNode>();
    public void AddChild(TreeNode child)
    {
        _children.AddLast(child);
    }

    // Clear the children of this node
    public void ClearChildren()
    {
        _children.Clear();
    }
}