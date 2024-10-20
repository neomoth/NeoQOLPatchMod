using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class InventoryStackerPlayerData(Mod mod) : IScriptMod
{
	private Mod mod = mod;

	public bool ShouldRun(string path) => path == "res://Scenes/Singletons/playerdata.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter entryWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_add_item" },
			t => t is IdentifierToken { Name: "entry" },
			t => t.Type == TokenType.Newline
		], allowPartialMatch: true);

		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_ready" },
			t => t.Type == TokenType.Newline
		], allowPartialMatch: true);

		MultiTokenWaiter loadedWaiter = new MultiTokenWaiter([
			t => t is ConstantToken { Value: StringVariant { Value: "Loaded Save" } },
			t => t.Type == TokenType.Newline
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (entryWaiter.Check(token))
			{
				mod.Logger.Information("#################### FOUND ENTRY FUNC ######################");
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				foreach (var t in mod.GetMod()) yield return t;
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_append_entry");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("entry");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}
			else if (readyWaiter.Check(token))
			{
				mod.Logger.Information("#################### FOUND READY FUNC ######################");
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				foreach (var t in mod.GetMod()) yield return t;
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_append_entry");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("FALLBACK_ITEM");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}
			else if (loadedWaiter.Check(token))
			{
				mod.Logger.Information("#################### FOUND LOAD FUNC ######################");
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				foreach (var t in mod.GetMod()) yield return t;
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_initialize_keys");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				foreach (var t in mod.GetMod()) yield return t;
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_stack_items()");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}