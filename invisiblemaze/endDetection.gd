extends Area3D


func _on_body_entered(body: Node3D) -> void:
	## When colliding with the palyer
	if body.name == "player":
		## Change scene to end screen
		get_tree().change_scene_to_file("res://win.tscn")
