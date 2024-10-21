using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class CosmeticLoaderGlobals(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Singletons/globals.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		mod.Logger.Information("loaded globals.gdc");
		MultiTokenWaiter extendsWaiter = new MultiTokenWaiter([
			t=>t.Type == TokenType.PrExtends,
			t => t.Type == TokenType.Newline,
		], allowPartialMatch: true);
		
		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_ready"},
			t => t is IdentifierToken {Name: "_load_resources"},
			t => t.Type == TokenType.Newline
		], allowPartialMatch:true);

		foreach (Token token in tokens)
		{
			if (extendsWaiter.Check(token))
			{
				//var nqolutil = preload("res://mods/NeoQOLPack/util.gd")
				yield return token;
				yield return new Token(TokenType.PrFunction);
				yield return new IdentifierToken("_get_mod");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 1);
				// yield return new Token(TokenType.PrVar);
				// yield return new IdentifierToken("neo_mod");
				// yield return new Token(TokenType.OpAssign);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_load_mod_resources");
				// yield return new IdentifierToken("load");
				yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("res://mods/NeoQOLPack/util.gd"));
				yield return new Token(TokenType.ParenthesisClose);

				yield return new Token(TokenType.Newline);
			}
			else if (readyWaiter.Check(token))
			{
				mod.Logger.Information("found ready func");
				yield return token;
				
				yield return new Token(TokenType.Newline, 1);
				// yield return new Token(TokenType.Dollar);
				// yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				// yield return new IdentifierToken("print");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new ConstantToken(new StringVariant("balls"));
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Newline, 1);
				// yield return new IdentifierToken("print");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new IdentifierToken("nqolutil");
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("call_deferred");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("_get_mod"));
				// yield return new IdentifierToken("_get_mod");
				yield return new Token(TokenType.ParenthesisClose);
				
				// yield return new IdentifierToken("nqolutil");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("_load_mod_resources");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new Token(TokenType.ParenthesisClose);
				
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}