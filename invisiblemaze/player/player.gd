extends CharacterBody3D

## Defines constants for movement
@export var SPEED = 5.0
#const JUMP_VELOCITY = 4.5

## allows the camera to move
## creates variables for neck and camera
@onready var neck := $Neck
@onready var camera := $Neck/Camera3D

## Camera Movement
func _unhandled_input(event: InputEvent) -> void: 
	## Checks for button inputs and hides the mouse if so
	if event is InputEventMouseButton:
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	
	## Checks for esc key and shows the mouse 
	elif event.is_action_pressed("escape"):
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	
	## Checks for mouse is moving
	if Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		## moves camera relative to mouse
		if event is InputEventMouseMotion:
			neck.rotate_y(-event.relative.x * 0.001)
			camera.rotate_x(-event.relative.y * 0.001)
			camera.rotation.x = clamp(camera.rotation.x, deg_to_rad(-30), deg_to_rad(60))

func _physics_process(delta: float) -> void:
	## We dont really need gravity? If we do re-enable 
	# Add the gravity.
	#if not is_on_floor():
	#	velocity += get_gravity() * delta

	##disabled jump bc we dont need that
	#Handle jump. 
	#if Input.is_action_just_pressed("ui_accept") and is_on_floor():
	#	velocity.y = JUMP_VELOCITY

	## Get the input direction and handle the movement/deceleration.
	var input_dir := Input.get_vector("back", "forward", "left", "right")
	var direction = (neck.transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	if direction:
		velocity.x = direction.x * SPEED
		velocity.z = direction.z * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		velocity.z = move_toward(velocity.z, 0, SPEED)

	move_and_slide()


func _on_area_3d_body_entered(body: Node3D) -> void:
	if body.name == "Monster":
		print("YOU GOT CAUGHT")
