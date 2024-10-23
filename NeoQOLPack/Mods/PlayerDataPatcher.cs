using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class PlayerDataPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Singletons/playerdata.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter entryWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_add_item" },
			t => t is IdentifierToken { Name: "entry" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_ready" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		MultiTokenWaiter resetWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_reset" },
			t => t is IdentifierToken { Name: "player_options" },
			t => t.Type is TokenType.OpAssign,
			t => t.Type is TokenType.CurlyBracketOpen,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		MultiTokenWaiter storedSaveWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken { Name: "_load_save" },
			t => t.Type is TokenType.CfIf,
			t => t is IdentifierToken {Name: "VERSION_MATCH"},
			t => t.Type is TokenType.OpAnd,
			t => t.Type is TokenType.CfReturn,
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		MultiTokenWaiter loadedWaiter = new MultiTokenWaiter([
			t => t is ConstantToken { Value: StringVariant { Value: "Loaded Save" } },
			t => t.Type == TokenType.Newline
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (entryWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.Newline, 1);

				yield return new IdentifierToken("entry");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("entry");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stack_size"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("entry");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stacked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				
				yield return new Token(TokenType.Newline, 1);
			}
			else if (readyWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("FALLBACK_ITEM");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("FALLBACK_ITEM");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stack_size"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("FALLBACK_ITEM");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stacked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				
				yield return new Token(TokenType.Newline, 1);
				
			}
			else if (loadedWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.CfFor);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.OpIn);
				yield return new IdentifierToken("inventory");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("has");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("locked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("has");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("stack_size"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stack_size"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("has");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("stacked"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stacked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline, 1);

				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("tools_to_stack");
				yield return new Token(TokenType.OpAssign);
				yield return new Token(TokenType.BracketOpen);
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("items_marked_for_stack");
				yield return new Token(TokenType.OpAssign);
				yield return new Token(TokenType.BracketOpen);
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new Token(TokenType.CfFor);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.OpIn);
				yield return new IdentifierToken("inventory");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("file");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("Globals");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("item_data");
				yield return new Token(TokenType.BracketOpen);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("id"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("file"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("file");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("category");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("tool"));
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 3);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("found_item");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline, 3);
				yield return new Token(TokenType.CfFor);
				yield return new IdentifierToken("t_item");
				yield return new Token(TokenType.OpIn);
				yield return new IdentifierToken("tools_to_stack");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 4);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("id"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpEqual);
				yield return new IdentifierToken("t_item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("id"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 5);
				yield return new IdentifierToken("found_item");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(true));
				yield return new Token(TokenType.Newline, 5);
				yield return new IdentifierToken("t_item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stack_size"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssignAdd);
				yield return new ConstantToken(new IntVariant(1));
				yield return new Token(TokenType.Newline, 5);
				yield return new IdentifierToken("items_marked_for_stack");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("append");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 5);
				yield return new Token(TokenType.CfBreak);
				yield return new Token(TokenType.Newline, 3);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("found_item");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 4);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stack_size"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.Newline, 4);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("stacked"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.Newline, 4);
				yield return new IdentifierToken("tools_to_stack");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("append");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("item");
				yield return new Token(TokenType.ParenthesisClose);
				
				yield return new Token(TokenType.Newline, 1);
			} else if (storedSaveWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("stored_save");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("player_options"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("keys");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("has");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("lockmouse"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("stored_save");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("player_options"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("lockmouse"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new IntVariant(0));
				
				yield return new Token(TokenType.Newline, 1);
			} else if (resetWaiter.Check(token))
			{
				yield return token;

				yield return new ConstantToken(new StringVariant("lockmouse"));
				yield return new Token(TokenType.Colon);
				yield return new ConstantToken(new IntVariant(0));
				yield return new Token(TokenType.Comma);

				yield return new Token(TokenType.Newline, 2);
			}
			else yield return token;
		}
	}
}