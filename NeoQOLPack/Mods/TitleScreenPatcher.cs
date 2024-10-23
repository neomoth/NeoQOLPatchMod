using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class TitleScreenPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player_label.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter labelWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name:"_process"},
			t=>t is IdentifierToken {Name:"title"},
			t=>t.Type is TokenType.Newline
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (labelWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("VBoxContainer"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("get_child");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new IntVariant(1));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("bbcode_text");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("[center]"));
				yield return new Token(TokenType.OpAdd);
				yield return new IdentifierToken("title");
				yield return new Token(TokenType.OpAdd);
				yield return new ConstantToken(new StringVariant("[/center]"));

				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}