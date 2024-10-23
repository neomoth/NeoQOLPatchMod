using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class InventoryItemPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/inventory_item.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter varWaiter = new([
			t => t.Type is TokenType.PrExtends,
			t => t.Type is TokenType.Newline,
		], allowPartialMatch: true);
		//hotkey_panel.visible = hotkey.size() > 0
		MultiTokenWaiter updateWaiter = new([
			t => t.Type is TokenType.PrFunction,
			t => t is IdentifierToken { Name: "_update" },
			t => t is IdentifierToken { Name: "hotkey_panel" },
			t => t is IdentifierToken { Name: "visible" },
			t => t.Type is TokenType.Newline,
		], allowPartialMatch: true);
		// this is unfinished ^^^
		foreach (Token token in tokens)
		{
			if (varWaiter.Check(token))
			{
				// mod.Logger.Information("#################### FOUND VAR SPOT ######################"); // C
				yield return token;

				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("display_stacked");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline);
				yield return new Token(TokenType.PrOnready);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("stack_size");
				yield return new Token(TokenType.OpAssign);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_attach_stack_size");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("root"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline);
			}
			else if (updateWaiter.Check(token))
			{
				// mod.Logger.Information("#################### FOUND UPDATE FUNC ######################"); // C
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_apply_stack_visual");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("display_stacked");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("idata");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("stack_size");
				yield return new Token(TokenType.ParenthesisClose);
			}
			else
			{
				yield return token;
			}
		}
	}
}