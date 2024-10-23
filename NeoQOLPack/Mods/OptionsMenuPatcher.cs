using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class OptionsMenuPatcher : IScriptMod
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
				
				yield return new Token(TokenType.Newline, 1);
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
			}
			else yield return token;
		}
	}
}