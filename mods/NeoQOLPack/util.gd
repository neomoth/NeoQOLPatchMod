extends Reference
class_name NQOLUtil

static func _load_mod_resources():
	print("Loading NEOQOL Resources...")
	var resource_count = 0
	var files = []
	var subdirectories = []
	var dir = Directory.new()
	var path = "res://mods/NeoQOLPack/Resources/"
	
	if dir.open(path) != OK:
		print("Error loading resource directory.")
		return 
	dir.list_dir_begin(true, true)
	while true:
		var file = dir.get_next()
		if file == "":
			break
		elif dir.current_is_dir():
			subdirectories.append(file)
			print("Directory found: ", file)
	
	for directory in subdirectories:
		if dir.open(path + directory) != OK:
			print("Error loading resource subdirectory ", directory)
			break
		dir.list_dir_begin(true, true)
		while true:
			var file = dir.get_next()
			if file == "":
				break
			elif file.ends_with(".tres"):
				files.append([path + directory + "/" + file, file])
				resource_count += 1
	
	for file in files:
		_add_mod_resource(file[0], file[1])
	
	dir.list_dir_end()
	print(str(resource_count) + " Resoures Loaded from " + str(subdirectories.size()) + " Subdirectories.")

static func _add_mod_resource(file, file_name):
	file_name = file_name.replace(".tres", "")
	var read = load(file)
	if read.get("resource_type") == null:
		print("TRES file does not have resource type labeled.")
		return 
	var type = read.get("resource_type")
	
	var new = {}
	new["file"] = load(file)
	match type:
		"cosmetic": Globals.cosmetic_data[file_name] = new
		"item": Globals.item_data[file_name] = new
