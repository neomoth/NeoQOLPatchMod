using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class ShopButtonPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/Shop/ShopButtons/shop_button.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		//price_label.text = prefix + str(cost)
		MultiTokenWaiter setWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "price_label"},
			t => t.Type is TokenType.Period,
			t=> t is IdentifierToken {Name: "text"},
			t=>t.Type is TokenType.OpAssign,
			t=> t is IdentifierToken {Name: "prefix"},
			t => t.Type is TokenType.OpAdd,
			t => t is IdentifierToken { Name: "cost"},
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		foreach (Token token in tokens)
		{
			if (setWaiter.Check(token))
			{
				mod.Logger.Debug("awawawawawa loaded found whatever kys");
				yield return token;
				
				//#price_label.text = get_node("/root/Main")._shorten_cost(cost)

				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("price_label");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("text");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_shorten_cost");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("cost");
				yield return new Token(TokenType.ParenthesisClose);
				
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}