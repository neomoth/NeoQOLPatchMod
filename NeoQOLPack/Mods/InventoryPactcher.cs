using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class InventoryPactcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/inventory.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter refreshWaiter = new([
			t => t is IdentifierToken {Name: "_refresh"},
			t => t is IdentifierToken {Name: "refs"},
			t => t.Type == TokenType.CurlyBracketClose,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		MultiTokenWaiter skipperWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_refresh"},
			t => t is IdentifierToken {Name: "data"},
			t => t is IdentifierToken {Name: "item_is_hidden"},
			t => t.Type is TokenType.CfContinue,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		foreach (Token token in tokens)
		{
			if (refreshWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_stack_items");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}
			
			else if (skipperWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stacked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 3);
				yield return new IdentifierToken("index");
				yield return new Token(TokenType.OpAssignAdd);
				yield return new ConstantToken(new IntVariant(1));
				yield return new Token(TokenType.Newline, 3);
				yield return new Token(TokenType.CfContinue);
				
				yield return new Token(TokenType.Newline, 2);
			}
			else yield return token;
		}
	}
}