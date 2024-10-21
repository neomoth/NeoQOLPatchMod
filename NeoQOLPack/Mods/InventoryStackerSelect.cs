using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class InventoryStackerSelect(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/ItemSelect/item_select.gdc";
	
	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
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
				for (var i = 0; i < 20; i++)
				{
					mod.Logger.Information("PENIS");
				}

				mod.Logger.Information("#################### FOUND ADD FUNC ######################");
				yield return token;

				yield return new IdentifierToken("i");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("display_stacked");
				yield return new Token(type: TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(true));
				yield return new Token(TokenType.Newline, 2);
			}
			else yield return token;
		}
	}
}