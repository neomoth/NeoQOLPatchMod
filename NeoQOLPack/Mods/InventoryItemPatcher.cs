using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class InventoryItemPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/inventory_item.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter extendsWaiter = new MultiTokenWaiter([
			t => t.Type is TokenType.PrExtends,
			t => t.Type is TokenType.Newline,
		], allowPartialMatch: true);

		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_ready" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		MultiTokenWaiter updateWaiter = new MultiTokenWaiter([
			t => t.Type is TokenType.PrFunction,
			t => t is IdentifierToken { Name: "_update" },
			t => t is IdentifierToken { Name: "hotkey_panel" },
			t => t is IdentifierToken { Name: "visible" },
			t => t.Type is TokenType.Newline,
		], allowPartialMatch: true);

		MultiTokenWaiter setupWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_setup_item" },
			t => t is IdentifierToken { Name: "ref" }
		], allowPartialMatch: true);

		MultiTokenWaiter setupBodyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_setup_item" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		foreach (Token token in tokens)
		{
			if (extendsWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("display_stacked");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline);
				yield return new Token(TokenType.PrOnready);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("stack_size");
				yield return new Token(TokenType.OpAssign);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_attach_stack_size");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("root"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline);
				yield return new Token(TokenType.PrOnready);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("locked_panel");
				yield return new Token(TokenType.OpAssign);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_attach_lock");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.Self);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("is_on_hotbar");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline);
				
				yield return new Token(TokenType.PrFunction);
				yield return new IdentifierToken("_on_item_gui_input");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("event");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("is_on_hotbar");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.CfReturn);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("idata");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_find_item_code");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("saved_ref");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("Globals");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("item_data");
				yield return new Token(TokenType.BracketOpen);
				yield return new IdentifierToken("idata");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("id"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("file"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("can_be_sold");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.CfReturn);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("event");
				yield return new Token(TokenType.PrIs);
				yield return new IdentifierToken("InputEventMouseButton");
				yield return new Token(TokenType.OpAnd);
				yield return new IdentifierToken("event");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("pressed");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("event");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("button_index");
				yield return new Token(TokenType.OpEqual);
				yield return new IdentifierToken("BUTTON_RIGHT");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 3);
				yield return new IdentifierToken("idata");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("idata");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Newline, 3);
				yield return new IdentifierToken("GlobalAudio");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_play_sound");
				// yield return new IdentifierToken("emit_signal");
				yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("_play_sfx"));
				// yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new StringVariant("tb_close"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 3);
				yield return new IdentifierToken("_update");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline);
			}
			else if (readyWaiter.Check(token))
			{
				yield return token;

				// yield return new IdentifierToken("owner");
				// yield return new Token(TokenType.Period);
				yield return new IdentifierToken("connect");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("gui_input"));
				yield return new Token(TokenType.Comma);
				yield return new Token(TokenType.Self);
				yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new StringVariant("_on_item_gui_input"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}
			else if (updateWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_apply_stack_visual");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("display_stacked");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("idata");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("stack_size");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("is_on_hotbar");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("locked_panel");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("visible");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("idata");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("data");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("can_be_sold");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 3);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("idata");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("desc");
				yield return new Token(TokenType.OpAssignAdd);
				yield return new ConstantToken(new StringVariant("\n[color=#b48141](Right click to prevent item from being sold)[/color]"));
				yield return new Token(TokenType.Newline, 3);
				yield return new Token(TokenType.CfElse);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("desc");
				yield return new Token(TokenType.OpAssignAdd);
				yield return new ConstantToken(new StringVariant("\n[color=#b48141](Right click to allow item to be sold)[/color]"));
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfElse);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("locked_panel");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("visible");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline, 1);
				
				//$root / tooltip_node.body = desc + sizetext + worthtext
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("root/tooltip_node"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("body");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("desc");
				yield return new Token(TokenType.OpAdd);
				yield return new IdentifierToken("sizetext");
				yield return new Token(TokenType.OpAdd);
				yield return new IdentifierToken("worthtext");
				yield return new Token(TokenType.Newline, 1);
				
				//Tooltip._update($root/tooltip_node.header, $root/tooltip_node.body, null)
				yield return new IdentifierToken("Tooltip");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_update");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("root/tooltip_node"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("header");
				yield return new Token(TokenType.Comma);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("root/tooltip_node"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("body");
				yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new NilVariant());
				yield return new Token(TokenType.ParenthesisClose);
			}
			else if (setupWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("hotbar");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
			}
			else if (setupBodyWaiter.Check(token))
			{
				yield return token;

				yield return new IdentifierToken("is_on_hotbar");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("hotbar");

				yield return new Token(TokenType.Newline, 1);
			}
			else
			{
				yield return token;
			}
		}
	}
}