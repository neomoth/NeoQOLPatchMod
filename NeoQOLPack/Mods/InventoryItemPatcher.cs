using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class InventoryItemPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/inventory_item.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter updateWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_update" },
			t => t is IdentifierToken { Name: "hotkey_panel" },
			t => t is IdentifierToken { Name: "visible" },
			t => t.Type is TokenType.Newline,
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (updateWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("/root/playerhud"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("menu");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new IntVariant(2));
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("Tooltip");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_update");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("root/tooltip_node"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("header");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("root/tooltip_node"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("body");
				yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new NilVariant());
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}