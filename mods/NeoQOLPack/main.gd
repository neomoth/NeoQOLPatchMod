class_name NeoQOL
extends Node

#var version = "unknown"
#var checked_new_version = "unknown"

onready var lure = get_node("/root/SulayreLure")

func _ready():
	print("loaded")
	#_load_mod_resources()
	
	#lure.assign_pattern_texture("NeoQOLPack", "NeoQOLPack.koni_pattern", "species_cat", "")
	
	lure.add_content("NeoQOLPack", "koni_antennae", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_antennae.tres")
	lure.add_content("NeoQOLPack", "koni_wings", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_wings.tres")
	lure.add_content("NeoQOLPack", "koni_fluff", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_fluff.tres")
	lure.add_content("NeoQOLPack", "eye_koni", "res://mods/NeoQOLPack/Resources/Cosmetics/eye_koni.tres")
	lure.add_content("NeoQOLPack", "pcolor_koni1", "res://mods/NeoQOLPack/Resources/Cosmetics/pcolor_koni1.tres")
	lure.add_content("NeoQOLPack", "pcolor_koni2", "res://mods/NeoQOLPack/Resources/Cosmetics/pcolor_koni2.tres")
	lure.add_content("NeoQOLPack", "koni_cheeks", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_cheeks.tres")
	lure.add_content("NeoQOLPack", "koni_pattern", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_pattern.tres")
	lure.add_content("NeoQOLPack", "title_birdfucker", "mod://Resources/Titles/title_birdfucker.tres", [])
	lure.add_content("NeoQOLPack", "title_colonthreetimeseight", "mod://Resources/Titles/title_colonthreetimeseight.tres", [])
	lure.add_content("NeoQOLPack", "title_ihavestupidamountsofmoney", "mod://Resources/Titles/title_ihavestupidamountsofmoney.tres", [])
	lure.add_content("NeoQOLPack", "title_mothwoman", "mod://Resources/Titles/title_mothwoman.tres")
	lure.add_content("NeoQOLPack", "title_seventvowner", "mod://Resources/Titles/title_seventvowner.tres", [])
	lure.add_content("NeoQOLPack", "title_streamerman", "mod://Resources/Titles/title_streamerman.tres", [])
	
	get_tree().root.connect("child_entered_tree", self, "_on_root_child_enter")
	
	var a = preload("res://mods/NeoQOLPack/test.gd")

func _on_root_child_enter(node):
	print("node entered root")
	if node.name == "playerhud":
		print("playerhud found")
		if Network.STEAM_ID == 76561198244258834:
			PlayerData._unlock_cosmetic("NeoQOLPack.koni_antennae")
			PlayerData._unlock_cosmetic("NeoQOLPack.koni_wings")
			PlayerData._unlock_cosmetic("NeoQOLPack.koni_fluff")
			PlayerData._unlock_cosmetic("NeoQOLPack.title_mothwoman")
			PlayerData._unlock_cosmetic("NeoQOLPack.eye_koni")
			PlayerData._unlock_cosmetic("NeoQOLPack.pcolor_koni1")
			PlayerData._unlock_cosmetic("NeoQOLPack.pcolor_koni2")
			PlayerData._unlock_cosmetic("NeoQOLPack.koni_cheeks")
			PlayerData._unlock_cosmetic("NeoQOLPack.koni_pattern")

func get_clipping_ancestor(obj):
	var current = obj.get_parent()
	while current:
		if current.is_class("Control") and current.rect_clip_content:
			return current
		current = current.get_parent()
	return null

func is_clipped(obj, parent):
	var clipping_ancestor
	if parent == null: clipping_ancestor = get_clipping_ancestor(obj)
	else: clipping_ancestor = parent
	if clipping_ancestor == null:
		return false
	var ancestor_rect = Rect2(Vector2.ZERO, clipping_ancestor.rect_size)
	var global_rect = Rect2(obj.rect_global_position, obj.rect_size)
	
	ancestor_rect.position = clipping_ancestor.rect_global_position
	return not ancestor_rect.intersects(global_rect)

func _patch_inventory_scroll():
	pass

static func _stack_items():
	var tools_to_stack = []
	var items_marked_for_stack = []
	for item in PlayerData.inventory:
		var file = Globals.item_data[item["id"]]["file"]
		if file.category == "tool":
			var found_item = false
			for t_item in tools_to_stack:
				if item["id"] == t_item["id"]:
					found_item = true
					t_item["stack_size"] += 1
					items_marked_for_stack.append(item)
					break
			if not found_item:
				item["stack_size"] = 0
				item["stacked"] = false
				tools_to_stack.append(item)

func _append_version(parent,version = "unknown"):
	print("attaching label...")
	var label: RichTextLabel = RichTextLabel.new()
	parent.add_child(label)
	label.anchor_left = 0.01
	label.anchor_top =  0.946
	label.anchor_right = 0.4
	label.anchor_bottom = 0.985
	label.margin_left = 0
	label.margin_top =  0
	label.margin_right = 0
	label.margin_bottom = -1
	label.rect_position = Vector2(20,1022)
	label.rect_size = Vector2(748,40)
	label.fit_content_height = true
	label.scroll_active = false
	label.add_color_override("default_color", Color.white)
	label.bbcode_enabled = true
	label.bbcode_text = "[color=purple]NeoQOLPack[/color] "+str(version)

static func _apply_stack_visual(display_stacked,idata,stack_size):
	#print("######################## APPLYING VISUAL ######################## ")
	if not display_stacked:
		if idata["stack_size"]>0:
			stack_size.visible = true
			stack_size.text = "x"+str(idata["stack_size"]+1)
		else:
			stack_size.visible = false
	else: stack_size.visible = false

static func _attach_stack_size(parent):
	#print("######################## ATTACHING LABEL ########################")
	var stack_size_label: Label = preload("res://mods/NeoQOLPack/Scenes/HUD/StackSize.tscn").instance()
	parent.add_child(stack_size_label)
	stack_size_label.rect_position = Vector2(41.74,51)
	stack_size_label.rect_size = Vector2(41.26,34)
	stack_size_label.margin_left = 43
	stack_size_label.margin_top = 51
	stack_size_label.margin_right = -7
	stack_size_label.margin_bottom = -5
	stack_size_label.anchor_left = -0.18
	stack_size_label.anchor_top = 0
	stack_size_label.anchor_right = 1
	stack_size_label.anchor_bottom = 1
	#print(parent)
	return stack_size_label

static func _attach_lock(parent):
	var lock_icon: Panel = preload("res://mods/NeoQOLPack/Scenes/HUD/InventoryLock.tscn").instance()
	parent.add_child(lock_icon)
	lock_icon.margin_left = 6
	lock_icon.margin_top = 6
	lock_icon.margin_right = -6
	lock_icon.margin_bottom = -6
	lock_icon.rect_position = Vector2(6,6)
	lock_icon.rect_size = Vector2(78,78)
	lock_icon.anchor_left = 0
	lock_icon.anchor_top = 0
	lock_icon.anchor_right = 0.046
	lock_icon.anchor_bottom = 0.083
	return lock_icon

static func _append_entry(entry):
	print("######################## APPENDING ENTRY ########################")
	#"locked": locked, "stack_size": 0, "stacked": false
	entry["locked"] = false
	entry["stack_size"] = 0
	entry["stacked"] = false

static func _replace_player_label(title):
	var parent = title.get_child(0)
	var original = parent.get_child(1)
	var original_text
	if original is Label:
		original_text = original.text
	else:
		original_text = original.bbcode_text
	parent.remove_child(original)
	original.queue_free()
	var label:RichTextLabel = RichTextLabel.new()
#	label.add_font_override("normal_font", preload("res://mods/NeoQOLPack/Themes/player_title.tres"))
	label.bbcode_enabled = true
	label.name = "Label2"
	label.bbcode_text = original_text
	label.fit_content_height = true
	parent.add_child(label)
	label.margin_left = 0 
	label.margin_top = 936
	label.margin_right = 720
	label.margin_bottom = 960
	label.rect_clip_content = false
	#label.rect_position = Vector2(0,936)

static func _shorten_cost(cost):
	#print("shortening cost")
	if cost >= 10000:
		var shortened = cost / 1000
		return str(shortened)+"K"
	return str(cost)

static func _append_shop_buttons(parent,ref):
	var button = load("res://Scenes/HUD/Shop/ShopButtons/shop_button.tscn").instance()
	button.set_script(load("res://Scenes/HUD/Shop/ShopButtons/button_cosmetic_unlock.gd"))
	button.cosmetic_unlock = "NeoQOLPack.title_ihavestupidamountsofmoney"
	#button.cosmetic_unlock = item
	button.cost = 999999
	#button.cost = Globals.cosmetic_data[item]["file"].cost
	parent.add_child(button)
	button.hud = ref.get_node(ref.hud)
	button.connect("mouse_entered", ref, "_item_entered", [button])
	button.connect("_used", ref, "_slot_used")

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
