using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class PlayerLabelPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player_label.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter updateTitleWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_update_title" },
			t => t is IdentifierToken { Name: "KNOWN_CONTRIBUTORS"},
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (updateTitleWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("player_id");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new IntVariant(76561198244258834, is64: true));
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("_name");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("[rainbow freq=0.5][wave amp=50][LITERALLY A FUCKING MORON][/wave][/rainbow]\n"));
				yield return new Token(TokenType.OpAdd);
				yield return new IdentifierToken("_name");
				
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}