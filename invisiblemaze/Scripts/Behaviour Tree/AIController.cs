public class AIController
{
    private BTNode _behaviourTree;
    private Blackboard _blackboard;

    public BTNode BehaviourTree
    {
        get => _behaviourTree;
        set => _behaviourTree = value;
    }

    public Blackboard Blackboard => _blackboard;

    public AIController()
    {
        _blackboard = new Blackboard();
    }

    public void EvaluateTree()
    {
        BehaviourTree.Evaluate();
    }
}
