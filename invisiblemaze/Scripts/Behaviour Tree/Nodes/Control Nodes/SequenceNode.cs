public class SequenceNode : ControlNode
{
    public override NodeState Evaluate()
    {
        foreach (BTNode child in children)
        {
            NodeState state = child.Evaluate();
            if (state == NodeState.RUNNING)
                return NodeState.RUNNING;
            if (state == NodeState.FAILURE)
                return NodeState.FAILURE;
        }
        return NodeState.SUCCESS;
    }
}
