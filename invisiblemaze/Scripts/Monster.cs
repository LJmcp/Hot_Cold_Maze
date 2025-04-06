using Godot;

public partial class Monster : CharacterBody3D
{
	[Export]
	private NavigationAgent3D navAgent;

	[Export]
	private Node3D targetPlayer;

	[Export]
	private bool isHunting = false;

	[Export]
	private float searchBoundsMinX = -10f;

	[Export]
	private float searchBoundsMaxX = 10f;

	[Export]
	private float searchBoundsMinZ = -10f;

	[Export]
	private float searchBoundsMaxZ = 10f;

	private Vector3 searchTarget = Vector3.Zero;
	private Vector3 oldSearchTarget = Vector3.Zero;
	private bool hasSearchTarget = false;

	private AIController _aiController;
	[Export]
	private Timer _PositionTimer;

	[Export]
	private float SPEED = 9f; // Adjusted for realistic 3D movement

	private RayCast3D raycast;
	
	private void OnPositionTimeout()
	{
		float randomX = (float)GD.RandRange(searchBoundsMinX, searchBoundsMaxX);
			float randomZ = (float)GD.RandRange(searchBoundsMinZ, searchBoundsMaxZ);
			searchTarget = new Vector3(randomX, GlobalTransform.Origin.Y, randomZ);
			hasSearchTarget = true;
			SetTarget(searchTarget);
	}

	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		raycast = GetNode<RayCast3D>("RayCast3D");

		_aiController = new AIController();
		_aiController.BehaviourTree = CreateHuntingNode();
		_PositionTimer.Start();
	}

	public override void _Process(double delta)
	{
		if (searchTarget  == oldSearchTarget)
		{
			//_PositionTimer.Start();
		}
		_aiController.EvaluateTree();
	}

[Export] public Node3D rotatingVisual; // Drag in the mesh or node to rotate

public override void _PhysicsProcess(double delta)
{
	if (navAgent.IsNavigationFinished())
		return;

	Vector3 nextPathPos = navAgent.GetNextPathPosition();
	Vector3 direction = (nextPathPos - GlobalTransform.Origin).Normalized();
	Vector3 velocity = direction * SPEED;

	navAgent.Velocity = velocity;
	Velocity = velocity;
	MoveAndSlide();

	if (velocity.LengthSquared() > 0.01f)
	{
		float angle = Mathf.Atan2(velocity.X, velocity.Z);

		// Apply rotation only to the visual object
		if (rotatingVisual != null)
{
	float currentY = rotatingVisual.Rotation.Y;
	float smoothY = Mathf.LerpAngle(currentY, angle, (float)delta * 5);
	rotatingVisual.Rotation = new Vector3(0, smoothY, 0);
}

	}
}




	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			// Note: In 3D you need a raycast from camera to world position to get clicked point
			// You can set this externally or with a RayCast3D
		}
	}

	public void SetTarget(Vector3 targetPos)
	{
		navAgent.TargetPosition = targetPos;
	}

	private BTNode CreateHuntingNode()
	{
		ControlNode root = new FallbackNode();

		ControlNode huntingPath = new SequenceNode();

		ControlNode huntingChecks = new FallbackNode();

		huntingChecks.AddChild(new ExecutionNode(CanSeePlayer));
		huntingChecks.AddChild(new ExecutionNode(IsHunting));

		huntingPath.AddChild(huntingChecks);
		huntingPath.AddChild(new ExecutionNode(HuntPlayer));

		root.AddChild(huntingPath);
		root.AddChild(new ExecutionNode(SearchForPlayer));

		return root;
	}

	private NodeState CanSeePlayer()
	{
		if (targetPlayer == null || raycast == null)
			return NodeState.FAILURE;

		Vector3 direction = (targetPlayer.GlobalTransform.Origin - GlobalTransform.Origin).Normalized();
		float distanceToPlayer = (targetPlayer.GlobalTransform.Origin - GlobalTransform.Origin).Length();

		raycast.TargetPosition = direction * distanceToPlayer;
		raycast.ForceRaycastUpdate();

		if (raycast.IsColliding())
		{
			var collider = raycast.GetCollider();
			if (collider == targetPlayer)
				return NodeState.SUCCESS;
		}

		return NodeState.FAILURE;
	}

	private NodeState IsHunting()
	{
		return isHunting ? NodeState.SUCCESS : NodeState.FAILURE;
	}

	private NodeState HuntPlayer()
	{
		isHunting = true;
		hasSearchTarget = true;
		searchTarget = targetPlayer.GlobalTransform.Origin;
		SetTarget(searchTarget);

		if (CanSeePlayer() == NodeState.FAILURE)
			isHunting = false;

		return NodeState.SUCCESS;
	}

	private NodeState SearchForPlayer()
	{
		if (!hasSearchTarget)
		{
			float randomX = (float)GD.RandRange(searchBoundsMinX, searchBoundsMaxX);
			float randomZ = (float)GD.RandRange(searchBoundsMinZ, searchBoundsMaxZ);
			searchTarget = new Vector3(randomX, GlobalTransform.Origin.Y, randomZ);
			hasSearchTarget = true;
			SetTarget(searchTarget);
		}

		if (GlobalTransform.Origin.DistanceTo(searchTarget) < 1.3f)
		{
			hasSearchTarget = false;
		}

		return NodeState.SUCCESS;
	}
}
