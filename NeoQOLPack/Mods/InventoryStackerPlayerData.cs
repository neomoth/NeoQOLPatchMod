using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class InventoryStackerPlayerData(Mod mod) : IScriptMod
{
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
				// mod.Logger.Information("#################### FOUND ENTRY FUNC ######################");
				yield return token;
				//if $"/root/MyModId":
				
				yield return new Token(TokenType.Newline, 1);
				// yield return new Token(TokenType.Dollar);
				// yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("_append_entry");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new IdentifierToken("entry");
				// yield return new Token(TokenType.ParenthesisClose);
				
				// entry["locked"] = false
				// entry["stack_size"] = 0
				// entry["stacked"] = false

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
				// mod.Logger.Information("#################### FOUND READY FUNC ######################");
				yield return token;

				yield return new Token(TokenType.Newline, 1);
				// yield return new Token(TokenType.Dollar);
				// yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("_append_entry");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new IdentifierToken("FALLBACK_ITEM");
				// yield return new Token(TokenType.ParenthesisClose);
				
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
				// mod.Logger.Information("#################### FOUND LOAD FUNC ######################");
				yield return token;

				// for item in PlayerData.inventory:
				// if !item.has("locked"): item["locked"] = false
				// if !item.has("stack_size"): item["stack_size"] = 0 # note: stack size is zero indexed, 0 = 1, 1 = 2, etc
				// if !item.has("stacked"): item["stacked"] = false # tracks if item is considered stacked with another item

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
				
				
				// yield return new Token(TokenType.Newline, 1);
				// yield return new Token(TokenType.Dollar);
				// yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("_initialize_keys");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new Token(TokenType.ParenthesisClose);
				// yield return new Token(TokenType.Newline, 1);
				// yield return new Token(TokenType.Dollar);
				// yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("_stack_items");
				// yield return new Token(TokenType.ParenthesisOpen);
				// yield return new Token(TokenType.ParenthesisClose);
				
				
				

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
				// var tools_to_stack = []
				// var items_marked_for_stack = []
	
				// # Required to ensure everyone's save is updated with the new dictionary keys
				// for item in PlayerData.inventory:
					// var file = Globals.item_data[item["id"]]["file"]
					// if file.category == "tool":
						// var found_item = false
						// for t_item in tools_to_stack:
							// if item["id"] == t_item["id"]:
								// found_item = true
								// t_item["stack_size"] += 1
								// items_marked_for_stack.append(item)
								// break
						// if not found_item:
						// item["stack_size"] = 0
						// item["stacked"] = false
						// tools_to_stack.append(item)
				//
				
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}