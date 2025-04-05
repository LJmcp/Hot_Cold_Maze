public abstract class BTNode
{
    public abstract NodeState Evaluate();
}

public enum NodeState //ToDo: Move to it's own enums folder???
{
    RUNNING,
    SUCCESS,
    FAILURE,
}
