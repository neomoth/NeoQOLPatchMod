using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class ItemSelectPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/ItemSelect/item_select.gdc";
	
	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter unselectableWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "unselectable"}
		], allowPartialMatch: true);
		MultiTokenWaiter addWaiter = new ([
			t => t is IdentifierToken {Name: "_ready"},
			t => t.Type == TokenType.PrVar,
			t => t is IdentifierToken {Name: "i"},
			t => t.Type == TokenType.Newline
		], allowPartialMatch: true);
		
		foreach (Token token in tokens)
		{
			if (addWaiter.Check(token))
			{
				yield return token;

				yield return new IdentifierToken("i");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("display_stacked");
				yield return new Token(type: TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(true));
				yield return new Token(TokenType.Newline, 2);
			}
			else if (unselectableWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.OpOr);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
			}
			else yield return token;
		}
	}
}