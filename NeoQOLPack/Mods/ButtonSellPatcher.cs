using GDWeave.Godot;
using GDWeave.Modding;
using GDWeave.Godot.Variants;

namespace NeoQOLPack.Mods;

public class ButtonSellPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/Shop/ShopButtons/button_sell.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		// when not waiting for this the error goes away but then i get another error saying "item" isn't valid in this scope at the same line number?????
		MultiTokenWaiter setupItemWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_setup" },
			t => t is IdentifierToken { Name: "slot_desc" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		// i thought maybe i wasn't getting the right spot? no fucking clue
		MultiTokenWaiter customUpdateWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_custom_update" },
			t => t is IdentifierToken { Name: "linked_ref" },
			t => t is IdentifierToken { Name: "has" },
			t => t.Type is TokenType.OpAssign,
			t => t is ConstantToken { Value: BoolVariant { Value: true } },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		MultiTokenWaiter customPurchaseWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_custom_purchase" },
			t => t is IdentifierToken { Name: "linked_ref" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (setupItemWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_find_item_code");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("linked_ref");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("locked");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(true));
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("slot_desc");
				yield return new Token(TokenType.OpAssignAdd);
				yield return new ConstantToken(new StringVariant("\n[color=red]This item is locked and cannot be sold.[/color]"));
				
				yield return new Token(TokenType.Newline);
			}
			else if (customUpdateWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.Newline, 3);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 4);
				yield return new IdentifierToken("locked");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(true));
				
				yield return new Token(TokenType.Newline, 1);
			}
			else if (customPurchaseWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_find_item_code");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("linked_ref");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("locked");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.CfReturn);
				
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}