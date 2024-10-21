class_name NeoQOL
extends Node

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

static func _append_entry(entry):
	print("######################## APPENDING ENTRY ########################")
	#"locked": locked, "stack_size": 0, "stacked": false
	entry["locked"] = false
	entry["stack_size"] = 0
	entry["stacked"] = false

static func _initialize_keys():
	print("######################## INITIALIZING KEYS ########################")
	for item in PlayerData.inventory:
		if !item.has("locked"): item["locked"] = false
		if !item.has("stack_size"): item["stack_size"] = 0 # note: stack size is zero indexed, 0 = 1, 1 = 2, etc
		if !item.has("stacked"): item["stacked"] = false # tracks if item is considered stacked with another item

static func _stack_items():
	print("######################## STACKING ITEMS ########################")
	var tools_to_stack = []
	var items_marked_for_stack = []
	
	# Required to ensure everyone's save is updated with the new dictionary keys
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
	
	for item in items_marked_for_stack:
		item["stacked"] = true
