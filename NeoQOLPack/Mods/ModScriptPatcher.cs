using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class ModScriptPatcher(Mod mod, string version, bool shouldNotify) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://mods/NeoQOLPack/main.gdc";

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
				yield return new IdentifierToken("version");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant(version));
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}