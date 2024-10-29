using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class MenuPatcher(Mod mod, string version) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Menus/Main Menu/main_menu.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter extendsWaiter = new MultiTokenWaiter([
			t=>t.Type is TokenType.PrExtends,
			t=>t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		MultiTokenWaiter buttonWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "Panel"},
			t => t.Type is TokenType.OpDiv,
			t => t is IdentifierToken {Name: "Panel"},
			t => t.Type is TokenType.OpDiv,
			t => t is IdentifierToken {Name: "HBoxContainer"},
			t => t.Type is TokenType.OpDiv,
			t => t is IdentifierToken {Name: "Button"},
			t => t.Type is TokenType.Period,
			t => t is IdentifierToken {Name: "disabled"},
			t => t.Type is TokenType.OpAssign,
			// t=>t is ConstantToken{Value:StringVariant{Value:"lobby_browser/Panel/Panel/HBoxContainer/Button"}},
			// t=> t.Type is TokenType.OpAssign,
			// t=>t.Type is TokenType.Newline
		], allowPartialMatch:false);
		
		MultiTokenWaiter disabledWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_process"},
			t => t is IdentifierToken {Name: "Panel"},
			t => t is IdentifierToken {Name: "Panel"},
			t => t is IdentifierToken {Name: "Panel"},
			// t => t.Type is TokenType.OpDiv,
			// t => t is IdentifierToken {Name: "Panel"},
			// t => t.Type is TokenType.OpDiv,
			// t => t is IdentifierToken {Name: "HBoxContainer"},
			// t => t.Type is TokenType.OpDiv,
			// t => t is IdentifierToken {Name: "Button"},
			// t => t.Type is TokenType.Period,
			// t => t is IdentifierToken {Name: "disabled"},
			// t => t.Type is TokenType.OpAssign,
			// t => t.Type is TokenType.ParenthesisOpen,
			t=>t is IdentifierToken {Name: "disabled"},
			t=>t.Type is TokenType.OpOr,
			t=>t is IdentifierToken {Name: "refreshing"},
			// t=>t is IdentifierToken {Name: "_process"},
			// t=>t is IdentifierToken {Name: "hovered_button"},
			// t=>t.Type is TokenType.ParenthesisClose,
			// t=>t.Type is TokenType.Newline
		], allowPartialMatch:true);
		
		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "_ready"},
			t=>t.Type is TokenType.Newline
		], allowPartialMatch: true);

		//$lobby_browser / Panel / Panel / HBoxContainer / Button.disabled = (disabled or refreshing) and !offline_selected
		//"%serv_options".connect("item_selected", self, "on_select")
		
		foreach (Token token in tokens)
		{
			if (extendsWaiter.Check(token))
			{
				yield return token;
				if (mod.modInterface.LoadedMods.Contains("WebfishingPlus")) continue;
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("offline_selected");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline);
				yield return new Token(TokenType.PrFunction);
				yield return new IdentifierToken("on_select");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("index");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 1);
				// yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextPrint);
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("coooooooooooooooooooooooooock"));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("offline_selected");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(true));
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("index");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new IntVariant(2));
				yield return new Token(TokenType.CfElse);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextPrint);
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("offline_selected");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.Newline);
			}
			else if (readyWaiter.Check(token))
			{
				yield return token;

				// yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextPrint);
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("peeeeeeeeeeeeeeeeeeeeeeeeeeeeeenis"));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_append_version");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.Self);
				yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new StringVariant(version));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				if (mod.modInterface.LoadedMods.Contains("WebfishingPlus")) continue;
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("%serv_options"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("connect");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("item_selected"));
				yield return new Token(TokenType.Comma);
				yield return new Token(TokenType.Self);
				yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new StringVariant("on_select"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}
			else if (buttonWaiter.Check(token))
			{
				yield return token;
				if (mod.modInterface.LoadedMods.Contains("WebfishingPlus")) continue;
				yield return new Token(TokenType.ParenthesisOpen);
			}
			else if (disabledWaiter.Check(token))
			{
				yield return token;
				if (mod.modInterface.LoadedMods.Contains("WebfishingPlus")) continue;
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.OpAnd);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("offline_selected");
				yield return new Token(TokenType.Newline, 1);

				// $lobby_browser / Panel / Panel / HBoxContainer / Button.disabled = (disabled or refreshing) #and !offline_selected
				// yield return new IdentifierToken("get_node");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("lobby_browser/Panel/Panel/HBoxContainer/Button"));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("disabled");
				// yield return new Token(TokenType.OpAssign);
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new IdentifierToken("disabled");
				// yield return new Token(TokenType.OpOr);
				// yield return new IdentifierToken("refreshing");
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.OpAnd);
				// yield return new Token(TokenType.OpNot);
				// yield return new IdentifierToken("offline_selected");
			}
			else yield return token;
		}
	}
}