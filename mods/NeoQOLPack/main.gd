class_name NeoQOL
extends Node

#var version = "unknown"
#var checked_new_version = "unknown"

onready var lure = get_node("/root/SulayreLure")

func _ready():
	print("loaded")
	#_load_mod_resources()
	
	lure.assign_pattern_texture("NeoQOLPack", "NeoQOLPack.koni_pattern", "species_cat", "")
	
	lure.add_content("NeoQOLPack", "koni_antennae", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_antennae.tres",[])
	lure.add_content("NeoQOLPack", "koni_wings", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_wings.tres",[])
	lure.add_content("NeoQOLPack", "koni_fluff", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_fluff.tres",[])
	lure.add_content("NeoQOLPack", "eye_koni", "res://mods/NeoQOLPack/Resources/Cosmetics/eye_koni.tres",[])
	lure.add_content("NeoQOLPack", "pcolor_koni1", "res://mods/NeoQOLPack/Resources/Cosmetics/pcolor_koni1.tres",[])
	lure.add_content("NeoQOLPack", "pcolor_koni2", "res://mods/NeoQOLPack/Resources/Cosmetics/pcolor_koni2.tres",[])
	lure.add_content("NeoQOLPack", "koni_cheeks", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_cheeks.tres",[])
	lure.add_content("NeoQOLPack", "koni_pattern", "res://mods/NeoQOLPack/Resources/Cosmetics/koni_pattern.tres",[])
	lure.add_content("NeoQOLPack", "title_birdfucker", "mod://Resources/Titles/title_birdfucker.tres", [])
	lure.add_content("NeoQOLPack", "title_colonthreetimeseight", "mod://Resources/Titles/title_colonthreetimeseight.tres", [])
	lure.add_content("NeoQOLPack", "title_ihavestupidamountsofmoney", "mod://Resources/Titles/title_ihavestupidamountsofmoney.tres", [])
	lure.add_content("NeoQOLPack", "title_mothwoman", "mod://Resources/Titles/title_mothwoman.tres",[])
	lure.add_content("NeoQOLPack", "title_seventvowner", "mod://Resources/Titles/title_seventvowner.tres", [])
	lure.add_content("NeoQOLPack", "title_streamerman", "mod://Resources/Titles/title_streamerman.tres", [])
	
	get_tree().root.connect("child_entered_tree", self, "_on_root_child_enter")
	
	var a = preload("res://mods/NeoQOLPack/test.gd")

func _refresh_price(ref):
	ref.tooltip.header = "Sell All"
	var current_price = 0
	for item in PlayerData.inventory:
		var file = Globals.item_data[item["id"]]["file"]
		if file.unselectable or not file.can_be_sold or PlayerData.locked_refs.has(item["ref"]): continue
		current_price+=PlayerData._get_item_worth(item["ref"])
	ref.total_price = current_price
	print(current_price)
	print(ref.total_price)
	if ref.total_price < 1:
		ref.tooltip.body = "You have no sellable items."
		ref.btn.disabled = true
	else:
		ref.tooltip.body = "Sells all sellable items.\nTotal value: [color=green]$"+str(ref.total_price)+"[/color]."
		ref.btn.disabled = false
	Tooltip._update(ref.tooltip.header,ref.tooltip.body,null)

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
