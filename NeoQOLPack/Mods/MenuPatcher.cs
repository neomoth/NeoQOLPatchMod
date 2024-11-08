using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class MenuPatcher(Mod mod, string version) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Menus/Main Menu/main_menu.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "_ready"},
			t=>t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		foreach (Token token in tokens)
		{
			if (readyWaiter.Check(token))
			{
				yield return token;
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_append_version");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.Self);
				yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new StringVariant(version));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}