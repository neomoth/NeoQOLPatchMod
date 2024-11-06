using GDWeave.Godot;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class CosmeticMenuPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/CosmeticMenu/cosmetic_menu.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "_ready"},
			t=>t is IdentifierToken {Name: "add_child"},
			t=>t.Type is TokenType.Newline
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (readyWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("model");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("owner_id");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("Network");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("STEAM_ID");

				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}