using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class SellAllButtonPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/Shop/sell_all_button.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter signalWaiter = new MultiTokenWaiter([
			t => t.Type is TokenType.PrSignal,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		// MultiTokenWaiter pressedWaiter = new MultiTokenWaiter([
		// 	t => t is IdentifierToken { Name: "_on_sell_all_mouse_entered" },
		// 	t => t.Type is TokenType.Colon
		// ], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (signalWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("tooltip");
				yield return new Token(TokenType.Newline);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("total_price");
				yield return new Token(TokenType.Newline);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("btn");
				yield return new Token(TokenType.Newline);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("neoqol");
				yield return new Token(TokenType.Newline);

				yield return new Token(TokenType.PrFunction);
				yield return new IdentifierToken("_ready");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("neoqol");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("btn");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("get_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new IntVariant(1));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("get_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("tooltip");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("TooltipNode");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("new");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("btn");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("add_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("tooltip");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("tooltip");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("anchor_bottom");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(1));
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("tooltip");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("anchor_left");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("tooltip");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("anchor_right");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(1));
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("tooltip");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("anchor_top");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("neoqol");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_refresh_price");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.Self);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("btn");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("connect");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("mouse_entered"));
				yield return new Token(TokenType.Comma);
				yield return new Token(TokenType.Self);
				yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new StringVariant("_on_sell_all_mouse_entered"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline);
				yield return new Token(TokenType.PrFunction);
				yield return new IdentifierToken("_on_sell_all_mouse_entered");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("neoqol");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_refresh_price");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.Self);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline);
			}
			// else if (pressedWaiter.Check(token))
			// {
			// 	yield return token;
			//
			// 	yield return new Token(TokenType.Newline, 1);
			// }
			else yield return token;
		}
	}
}