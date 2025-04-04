public abstract class DecoratorNode : BTNode
{
    protected BTNode child;

    public DecoratorNode(BTNode child)
    {
        this.child = child;
    }
}
