extends Button

var total_price = 0
var tooltip: TooltipNode

func _ready():
	tooltip = $TooltipNode
	_refresh_price()
	

	
func _refresh_price():
	var current_price = 0
	for item in PlayerData.inventory:
		var file = Globals.item_data[item["id"]]["file"]
		if file.unselectable or not file.can_be_sold or item["locked"]: continue
		current_price+=PlayerData._get_item_worth(item["ref"])
	total_price = current_price
	if total_price < 1:
		tooltip.body = "You have no sellable items."
		disabled = true
	else:
		tooltip.body = "This will sell every sellable item you have for about [color=green]$"+str(total_price)+"[/color]."
		disabled = false
	Tooltip._update(tooltip.header,tooltip.body,null)


func _on_Button2_mouse_entered():
	_refresh_price()


func _on_Button2_pressed():
	var items = []
	PlayerData.emit_signal("_play_sfx", "cash" + str(randi() % 2 + 1))
	for item in PlayerData.inventory:
		var file = Globals.item_data[item["id"]]["file"]
		if file.unselectable or not file.can_be_sold or item["locked"]: continue
		items.push_back(item)
		for key in PlayerData.hotbar.keys():
			if PlayerData.hotbar[key] == item["ref"]: PlayerData.hotbar[key] = 0
			
	for item in items:
		var ref = item["ref"]
		PlayerData.money+=PlayerData._get_item_worth(item["ref"])
		PlayerData.inventory.erase(item)
		PlayerData.emit_signal("_item_sold", ref)
		PlayerData.emit_signal("_item_removal", ref)
		
	PlayerData.emit_signal("_shop_update")
	PlayerData.emit_signal("_inventory_refresh")
	PlayerData.emit_signal("_hotbar_refresh")
	_refresh_price()
