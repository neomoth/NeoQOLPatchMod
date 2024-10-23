using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class PlayerPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_ready"},
			t => t.Type is TokenType.Newline
		],allowPartialMatch: true);
		
		MultiTokenWaiter busyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_get_input"},
			t => t.Type is TokenType.CfIf,
			t => t is IdentifierToken {Name: "busy"},
			// t => t is IdentifierToken {Name: "MOUSE_MODE_VISIBLE"},
			t => t.Type is TokenType.Newline
		],allowPartialMatch: true);
		
		MultiTokenWaiter busyWaiter2 = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_get_input"},
			t => t.Type is TokenType.CfIf,
			t => t is IdentifierToken {Name: "busy"},
			// t => t is IdentifierToken {Name: "MOUSE_MODE_VISIBLE"},
			t => t.Type is TokenType.ParenthesisOpen,
			// t => t.Type is TokenType.Newline
		],allowPartialMatch: true);
		
		MultiTokenWaiter elseWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_get_input"},
			//if Input.mouse_mode != Input.MOUSE_MODE_VISIBLE
			t => t.Type is TokenType.CfIf,
			t => t is IdentifierToken {Name: "Input"},
			t => t.Type is TokenType.Period,
			t => t is IdentifierToken {Name: "mouse_mode"},
			t => t.Type is TokenType.OpNotEqual,
			t => t is IdentifierToken {Name: "Input"},
			t => t.Type is TokenType.Period,
			t => t is IdentifierToken {Name: "MOUSE_MODE_VISIBLE"},
		],allowPartialMatch: true);
		
		MultiTokenWaiter visibleWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_get_input"},
			t => t.Type is TokenType.CfIf,
			t => t is IdentifierToken {Name: "Input"},
			t => t.Type is TokenType.Period,
			t => t is IdentifierToken {Name: "mouse_mode"},
			t => t.Type is TokenType.OpNotEqual,
			t => t is IdentifierToken {Name: "Input"},
			t => t.Type is TokenType.Period,
			t => t is IdentifierToken {Name: "MOUSE_MODE_VISIBLE"},
			t => t.Type is TokenType.ParenthesisClose,
			t => t.Type is TokenType.Newline
		],allowPartialMatch: true);

		//"/iamweest": PlayerData._unlock_cosmetic("title_streamerman")
		// "/colonthreetimeseight": PlayerData._unlock_cosmetic("title_colonthreetimeseight")
		// "/hithisisaveryhardstringtotrytoguesslol": PlayerData._unlock_cosmetic("title_seventvowner")
		
		//if busy:
		// 	if hud.menu == hud.MENUS.ESC: Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		// 	else: Input.set_mouse_mode(Input.MOUSE_MODE_CONFINED if PlayerData.player_options.lockmouse else Input.MOUSE_MODE_VISIBLE)
		// 	return 
		// 
		// if Input.is_action_pressed("secondary_action") or camera_zoom <= 0.0:
		// 	if Input.mouse_mode != Input.MOUSE_MODE_CAPTURED:
		// 		PlayerData.original_mouse_position = get_tree().get_nodes_in_group("world_viewport")[0].get_mouse_position()
		// 		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		// else :
		// 	if Input.mouse_mode != Input.MOUSE_MODE_VISIBLE and Input.mouse_mode != Input.MOUSE_MODE_CONFINED:
		// 		if hud.menu == hud.MENUS.ESC: Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		// 		else: Input.set_mouse_mode(Input.MOUSE_MODE_CONFINED if PlayerData.player_options.lockmouse else Input.MOUSE_MODE_VISIBLE)
		// 		Input.warp_mouse_position(PlayerData.original_mouse_position)
		
		foreach (Token token in tokens)
		{
			// mod.Logger.Information(token.ToString());
			if (readyWaiter.Check(token))
			{
				yield return token;
				yield return new Token(TokenType.Dollar);
				yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_replace_player_label");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("title");
				yield return new Token(TokenType.ParenthesisClose);

				yield return new Token(TokenType.Newline, 1);
			}

			else if (busyWaiter.Check(token))
			{
				mod.Logger.Information("found busy");
				yield return token;
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("hud");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("menu");
				yield return new Token(TokenType.OpEqual);
				yield return new IdentifierToken("hud");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MENUS");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("ESC");
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("set_mouse_mode");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MOUSE_MODE_VISIBLE");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.CfElse);
				yield return new Token(TokenType.Colon);
				
				yield return new Token(TokenType.Newline, 3);
			}
			else if (busyWaiter2.Check(token))
			{
				yield return token;
				
				// yield return new IdentifierToken("Input");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("set_mouse_mode");
				// yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MOUSE_MODE_CONFINED");
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("player_options");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("lockmouse");
				yield return new Token(TokenType.CfElse);
				// yield return new IdentifierToken("Input");
				// yield return new Token(TokenType.Period);
				// yield return new IdentifierToken("MOUSE_MODE_VISIBLE");
				// yield return new Token(TokenType.ParenthesisClose);
				
				// yield return new Token(TokenType.Newline, 2);
			}
			else if (elseWaiter.Check(token))
			{
				mod.Logger.Information("found else");
				yield return token;
				
				yield return new Token(TokenType.OpAnd);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("mouse_mode");
				yield return new Token(TokenType.OpNotEqual);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MOUSE_MODE_CONFINED");

				// yield return new Token(TokenType.Newline, 1);
			}
			else if (visibleWaiter.Check(token))
			{
				mod.Logger.Information("found visible");
				yield return token;
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("hud");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("menu");
				yield return new Token(TokenType.OpEqual);
				yield return new IdentifierToken("hud");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MENUS");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("ESC");
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("set_mouse_mode");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MOUSE_MODE_VISIBLE");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 3);
				yield return new Token(TokenType.CfElse);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("set_mouse_mode");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MOUSE_MODE_CONFINED");
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("player_options");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("lockmouse");
				yield return new Token(TokenType.CfElse);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MOUSE_MODE_VISIBLE");
				yield return new Token(TokenType.ParenthesisClose);
				
				yield return new Token(TokenType.Newline, 3);
			}
			else yield return token;
		}
	}
}