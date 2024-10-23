using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class ShopCategoryPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/Shop/ShopSetups/shop_category.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter extendsWaiter = new MultiTokenWaiter([
			t => t.Type is TokenType.PrExtends,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (extendsWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("show_sellall");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				
				yield return new Token(TokenType.Newline);
			}
			else yield return token;
		}
	}
}