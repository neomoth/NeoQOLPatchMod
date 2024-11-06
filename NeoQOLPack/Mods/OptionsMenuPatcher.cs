using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class OptionsMenuPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Singletons/OptionsMenu/options_menu.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter extendsWaiter = new MultiTokenWaiter([
			t => t.Type is TokenType.PrExtends,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_ready" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		MultiTokenWaiter setSelectionWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_set_selections_to_save" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		MultiTokenWaiter resetWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_reset" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		MultiTokenWaiter applySelectionsWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_apply_selections" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		foreach (Token token in tokens)
		{
			if (extendsWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("lock_mouse_node");

				yield return new Token(TokenType.Newline);
				
				// yield return new Token(TokenType.PrVar);
				// yield return new IdentifierToken("fov_slider");
				//
				// yield return new Token(TokenType.Newline);
				//
				// yield return new Token(TokenType.PrFunction);
				// yield return new IdentifierToken("_on_fov_value_changed");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new IdentifierToken("value");
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Colon);
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("fov_slider");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("get_parent");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("get_child");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new IntVariant(0));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("text");
				// yield return new Token(TokenType.OpAssign);
				// yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextStr);
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new IdentifierToken("value");
				// yield return new Token(TokenType.OpMul);
				// yield return new ConstantToken(new RealVariant(100.0));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.OpAdd);
				// yield return new ConstantToken(new StringVariant("%"));
				
				yield return new Token(TokenType.Newline);
			}
			else if (readyWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("lock_mouse_div");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("Control/Panel/tabs_main/main/ScrollContainer/HBoxContainer/VBoxContainer/punchable"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("duplicate");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);

				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("lock_mouse_label");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("lock_mouse_div");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("get_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("lock_mouse_node");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("lock_mouse_div");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("get_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new IntVariant(1));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("lock_mouse_node");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("name");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("lockmouse"));
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("lock_mouse_label");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("text");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("Lock Mouse to Window: "));
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("Control/Panel/tabs_main/main/ScrollContainer/HBoxContainer/VBoxContainer"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("add_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("lock_mouse_div");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("Control/Panel/tabs_main/main/ScrollContainer/HBoxContainer/VBoxContainer"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("move_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("lock_mouse_div");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("Control/Panel/tabs_main/main/ScrollContainer/HBoxContainer/VBoxContainer"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("get_child_count");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.OpSub);
				yield return new ConstantToken(new IntVariant(4));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("lock_mouse_node");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("add_item");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("Unlocked"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("lock_mouse_node");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("add_item");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("Locked"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("lock_mouse_label");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("get_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("header");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("Lock Mouse to Window"));
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("lock_mouse_label");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("get_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("body");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("When set to Locked, the mouse will be locked within the game window unless the game is paused."));
				yield return new Token(TokenType.Newline, 1);
				
				// yield return new Token(TokenType.PrVar);
				// yield return new IdentifierToken("fov_option_node");
				// yield return new Token(TokenType.OpAssign);
				// yield return new IdentifierToken("get_node");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("Control/Panel/tabs_main/main/ScrollContainer/HBoxContainer/VBoxContainer/main_vol"));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("duplicate");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new Token(TokenType.ParenthesisClose);
				//
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new Token(TokenType.PrVar);
				// yield return new IdentifierToken("label");
				// yield return new Token(TokenType.OpAssign);
				// yield return new IdentifierToken("fov_option_node");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("get_child");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new IntVariant(0));
				// yield return new Token(TokenType.ParenthesisClose);
				//
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new Token(TokenType.PrVar);
				// yield return new IdentifierToken("label2");
				// yield return new Token(TokenType.OpAssign);
				// yield return new IdentifierToken("fov_option_node");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("get_child");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new IntVariant(1));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("get_child");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new IntVariant(0));
				// yield return new Token(TokenType.ParenthesisClose);
				//
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("fov_slider");
				// yield return new Token(TokenType.OpAssign);
				// yield return new IdentifierToken("fov_option_node");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("get_child");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new IntVariant(1));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("get_child");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new IntVariant(1));
				// yield return new Token(TokenType.ParenthesisClose);
				//
				// yield return new Token(TokenType.Newline, 1);
				//
				// mod.Logger.Information("MADE IT HERE");
				//
				// yield return new IdentifierToken("fov_option_node");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("name");
				// yield return new Token(TokenType.OpAssign);
				// yield return new ConstantToken(new StringVariant("fov"));
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("label");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("text");
				// yield return new Token(TokenType.OpAssign);
				// yield return new ConstantToken(new StringVariant("Field of View"));
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("label2");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("name");
				// yield return new Token(TokenType.OpAssign);
				// yield return new ConstantToken(new StringVariant("fov_scale"));
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("get_node");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("Control/Panel/tabs_main/main/ScrollContainer/HBoxContainer/VBoxContainer"));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("add_child");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new IdentifierToken("fov_option_node");
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new Token(TokenType.PrVar);
				// yield return new IdentifierToken("separator");
				// yield return new Token(TokenType.OpAssign);
				// yield return new IdentifierToken("get_node");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("Control/Panel/tabs_main/main/ScrollContainer/HBoxContainer/VBoxContainer"));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("add_child");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new IdentifierToken("HSeparator");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("new");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.ParenthesisClose);
				//
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("separator");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("self_modulate");
				// yield return new Token(TokenType.OpAssign);
				// yield return new IdentifierToken("Color");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new RealVariant(0));
				// yield return new Token(TokenType.Comma);
				// yield return new ConstantToken(new RealVariant(0));
				// yield return new Token(TokenType.Comma);
				// yield return new ConstantToken(new RealVariant(0));
				// yield return new Token(TokenType.Comma);
				// yield return new ConstantToken(new RealVariant(0));
				// yield return new Token(TokenType.ParenthesisClose);
				//
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("separator");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("rect_min_size");
				// yield return new Token(TokenType.OpAssign);
				// yield return new IdentifierToken("Vector2");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new RealVariant(0));
				// yield return new Token(TokenType.Comma);
				// yield return new ConstantToken(new RealVariant(80));
				// yield return new Token(TokenType.ParenthesisClose);
				//
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("fov_slider");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("disconnect");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("value_changed"));
				// yield return new Token(TokenType.Comma);
				// yield return new Token(TokenType.Self);
				// yield return new Token(TokenType.Comma);
				// yield return new ConstantToken(new StringVariant("_on_main_vol_value_changed"));
				// yield return new Token(TokenType.ParenthesisClose);
				//
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("fov_slider");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("connect");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("value_changed"));
				// yield return new Token(TokenType.Comma);
				// yield return new Token(TokenType.Self);
				// yield return new Token(TokenType.Comma);
				// yield return new ConstantToken(new StringVariant("_on_fov_value_changed"));
				// yield return new Token(TokenType.ParenthesisClose);
				//
				// yield return new Token(TokenType.Newline, 1);
				//
				// yield return new IdentifierToken("fov_slider");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("connect");
				// yield return new Token(TokenType.OpAssign);
				// yield return new ConstantToken(new IntVariant(50));
				
				yield return new Token(TokenType.Newline, 1);
				
				//func _append_fov_slider(parent):
				// var target = parent.get_node("Control/Panel/tabs_main/main/ScrollContainer/HBoxContainer/VBoxContainer/main_vol")
				// var fov_node = target.duplicate()
				// var label = fov_node.get_child(0)
				// var label2 = fov_node.get_child(1).get_child(0)
				// var slider = fov_node.get_child(1).get_child(1)
				// fov_node.name = "fov"
				// label.text = "Field of View"
				// label2.name = "fov_scale"
				// (target as Node).get_parent().add_child(fov_node)
				// var separator = target.get_parent().add_child(HSeparator.new())
				// separator.self_modulate = Color(1,1,1,0)
				// separator.rect_min_size = Vector2(0,50)
				// 
				// slider.disconnect("value_changed", parent, "_on_main_vol_value_changed")
				// slider.connect("value_changed", self, "_on_fov_slider_changed")
				// fov_slider = fov_node
				// return slider
			}
			else if (setSelectionWaiter.Check(token))
			{
				yield return token;

				yield return new IdentifierToken("lock_mouse_node");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("selected");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("player_options");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("lockmouse");

				yield return new Token(TokenType.Newline, 1);
				
				// yield return new IdentifierToken("fov_slider");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("value");
				// yield return new Token(TokenType.OpAssign);
				// yield return new IdentifierToken("PlayerData");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("player_options");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("fovscale");
				//
				// yield return new Token(TokenType.Newline, 1);
			}
			else if (resetWaiter.Check(token))
			{
				yield return token;

				yield return new IdentifierToken("lock_mouse_node");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("selected");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(0));

				yield return new Token(TokenType.Newline, 1);
				
				// yield return new IdentifierToken("fov_slider");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("value");
				// yield return new Token(TokenType.OpAssign);
				// yield return new ConstantToken(new IntVariant(50));
				//
				// yield return new Token(TokenType.Newline, 1);
			}
			else if (applySelectionsWaiter.Check(token))
			{
				yield return token;

				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("player_options");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("lockmouse");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("lock_mouse_node");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("selected");

				yield return new Token(TokenType.Newline, 1);
				
				// yield return new IdentifierToken("PlayerData");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("player_options");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("fovscale");
				// yield return new Token(TokenType.OpAssign);
				// yield return new IdentifierToken("fov_slider");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("value");
				//
				// yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}