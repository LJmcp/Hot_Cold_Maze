extends Window

var mazeview = preload("Maze.tscn")

func _ready():
	#get_viewport().set_embedding_subwindows(false)
	var d = mazeview.instantiate()
	#d.add_visible = true
	#d.position = Vector2(1280,720)
	#d.title = "Maze view"
	#d.size = Vector2(1280,720)
