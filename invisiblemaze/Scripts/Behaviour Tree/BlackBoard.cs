using System.Collections.Generic;

public enum BlackboardKey
{
    TargetPosition,
    WaitTime,
    TimePassed,
    TargetObject,
    SeenPickups,
    TargetLocations,
    CurrentTargetIndex,
    AnimationState
}

public class Blackboard
{
    private Dictionary<BlackboardKey, object> _blackboardData = new Dictionary<BlackboardKey, object>();
    public HashSet<BlackboardKey> UsedKeys { get; private set; } = new HashSet<BlackboardKey>();

    public void SetValue<T>(BlackboardKey key, T value)
    {
        _blackboardData[key] = value;
        UsedKeys.Add(key);
    }

    public T GetValue<T>(BlackboardKey key)
    {
        UsedKeys.Add(key);
        return _blackboardData.TryGetValue(key, out var value) ? (T)value : default;
    }

    public bool ContainsKey(BlackboardKey key) => _blackboardData.ContainsKey(key);
}
