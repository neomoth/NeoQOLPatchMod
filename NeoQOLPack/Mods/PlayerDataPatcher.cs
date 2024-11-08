using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class PlayerDataPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Singletons/playerdata.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter resetWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_reset" },
			t => t is IdentifierToken { Name: "player_options" },
			t => t.Type is TokenType.OpAssign,
			t => t.Type is TokenType.CurlyBracketOpen,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		MultiTokenWaiter storedSaveWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken { Name: "_load_save" },
			t => t.Type is TokenType.CfIf,
			t => t is IdentifierToken {Name: "VERSION_MATCH"},
			t => t.Type is TokenType.OpAnd,
			t => t.Type is TokenType.CfReturn,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (storedSaveWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("stored_save");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("player_options"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("keys");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("has");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("lockmouse"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("stored_save");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("player_options"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("lockmouse"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(0));
				
				yield return new Token(TokenType.Newline, 1);
			} else if (resetWaiter.Check(token))
			{
				yield return token;

				yield return new ConstantToken(new StringVariant("lockmouse"));
				yield return new Token(TokenType.Colon);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.Comma);

				yield return new Token(TokenType.Newline, 2);
			}
			else yield return token;
		}
	}
}