using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class ShopPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/Shop/shop.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter replaceWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "replace" },
			t => t is ConstantToken { Value: StringVariant { Value: "_slot_used" } },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		//if(node.name=="titles"): get_node("/root/Main")._append_shop_buttons(grid,self)
		foreach (Token token in tokens)
		{
			if (replaceWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.Newline, 3);
				
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("node");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("name");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("titles"));
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_append_shop_buttons");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("grid");
				yield return new Token(TokenType.Comma);
				yield return new Token(TokenType.Self);
				yield return new Token(TokenType.ParenthesisClose);

				yield return new Token(TokenType.Newline, 2);
			}
			else yield return token;
		}
	}
}