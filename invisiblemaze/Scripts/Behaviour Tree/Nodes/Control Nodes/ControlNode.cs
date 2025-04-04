using System.Collections.Generic;

public abstract class ControlNode : BTNode
{
    protected List<BTNode> children = new List<BTNode>();

    public IReadOnlyList<BTNode> Children => children;

    public void AddChild(BTNode child)
    {
        children.Add(child);
    }
}
