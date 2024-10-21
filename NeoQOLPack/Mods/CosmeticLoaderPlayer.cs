using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class CosmeticLoaderPlayer(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_ready"},
			t => t.Type == TokenType.Newline
		],allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (readyWaiter.Check(token))
			{
				yield return token;
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_replace_player_label");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("title");
				yield return new Token(TokenType.ParenthesisClose);
				
				yield return new Token(TokenType.Newline, 1);
				
			} else yield return token;
		}
	}
}