using System;
using System.Collections.Generic;

public class ExecutionNode : BTNode
{
    private Func<NodeState> _action;
    public List<BlackboardKey> RequiredKeys {get; private set;} = new List<BlackboardKey>();

    public ExecutionNode(Func<NodeState> action, params BlackboardKey[] requiredKeys)
    {
        this._action = action;
        RequiredKeys.AddRange(requiredKeys);
    }

    public override NodeState Evaluate()
    {
        return _action();
    }
}
