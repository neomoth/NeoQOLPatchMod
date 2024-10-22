using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class PlayerHudPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter chatWaiter = new MultiTokenWaiter([
			t => t.Type == TokenType.CfMatch,
			t => t is IdentifierToken { Name: "line" },
			t=>t.Type is TokenType.Colon,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: false);
		
		foreach (Token token in tokens)
		{
			if (chatWaiter.Check(token))
			{
				mod.Logger.Debug("HIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII");
				yield return token;
				yield return new Token(TokenType.Newline, 4);
				yield return new ConstantToken(new StringVariant("/iamweest"));
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_unlock_cosmetic");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("title_streamerman"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 4);
				yield return new ConstantToken(new StringVariant("/colonthreetimeseight"));
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_unlock_cosmetic");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("title_colonthreetimeseight"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 4);
				yield return new ConstantToken(new StringVariant("/hithisisaveryhardstringtotrytoguesslol"));
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_unlock_cosmetic");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("title_seventvowner"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 4);
			}
			else yield return token;
		}
	}
}