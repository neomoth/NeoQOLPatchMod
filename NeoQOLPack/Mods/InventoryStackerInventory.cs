using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class InventoryStackerInventory(Mod mod) : IScriptMod
{
	private Mod mod = mod;

	public bool ShouldRun(string path) => path == "res://Scenes/HUD/inventory.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		mod.Logger.Information("hi im loaded probably");
		MultiTokenWaiter addWaiter = new ([
			t => t is IdentifierToken {Name: "box"},
			t => t is IdentifierToken {Name: "add_child"},
			t => t.Type == TokenType.Newline
		], allowPartialMatch: true);
		
		MultiTokenWaiter refreshWaiter = new([
			t => t is IdentifierToken {Name: "_refresh"},
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		/*
		 *
		 * var data = Globals.item_data[item["id"]]["file"]
		   if data.item_is_hidden or not refs.keys().has(data.category):
		   	index += 1
		   	continue
		   
		   if item["stacked"]:
		   	index += 1
		   	continue
		 * 
		 */

		MultiTokenWaiter skipperWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_refresh"},
			t => t is IdentifierToken {Name: "data"},
			t => t is IdentifierToken {Name: "item_is_hidden"},
			t => t.Type is TokenType.CfContinue,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		foreach (Token token in tokens)
		{
			mod.Logger.Information("wawa");
			mod.Logger.Information(token.ToString());
			mod.Logger.Information(refreshWaiter.Check(token).ToString());
			if (refreshWaiter.Check(token))
			{
				yield return token;
				
				mod.Logger.Information("#################### FOUND REFRESH FUNC ######################");
				
				foreach (var t in mod.GetMod()) yield return t;
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_stack_items()");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}
			else if (addWaiter.Check(token))
			{
				mod.Logger.Information("#################### FOUND ADD FUNC ######################");
				yield return token;
				
				yield return new IdentifierToken("i");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("display_stacked");
				yield return new Token(type: TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(true));
				yield return new Token(TokenType.Newline, 1);
			}
			else if (skipperWaiter.Check(token))
			{
				mod.Logger.Information("#################### FOUND SKIP FUNC ######################");
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