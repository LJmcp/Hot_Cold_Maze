public class FallbackNode : ControlNode
{
    public override NodeState Evaluate()
    {
        foreach (BTNode child in children)
        {
            NodeState state = child.Evaluate();
            if (state == NodeState.RUNNING)
                return NodeState.RUNNING;
            if (state == NodeState.SUCCESS)
                return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
