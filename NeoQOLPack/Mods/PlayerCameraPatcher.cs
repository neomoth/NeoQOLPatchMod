using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class PlayerCameraPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter extendsWaiter = new MultiTokenWaiter([
			t=>t.Type is TokenType.PrExtends,
			t=>t.Type is TokenType.Newline
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			yield return token;

			yield return new Token(TokenType.PrFunction);
			yield return new IdentifierToken("_process");
			yield return new Token(TokenType.ParenthesisOpen);
			yield return new IdentifierToken("delta");
			yield return new Token(TokenType.ParenthesisClose);
			yield return new Token(TokenType.Colon);
			yield return new Token(TokenType.Newline, 1);

			yield return new IdentifierToken("fov");
			yield return new Token(TokenType.OpAssign);
			
		}
	}
}