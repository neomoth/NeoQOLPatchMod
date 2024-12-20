using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class PlayerPatcher(Mod mod) : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		MultiTokenWaiter extendsWaiter = new MultiTokenWaiter([
			t=>t.Type is TokenType.PrExtends,
			t=>t.Type is TokenType.Newline
		], allowPartialMatch: true);
		
		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_ready"},
			t => t.Type is TokenType.Newline
		],allowPartialMatch: true);
		
		MultiTokenWaiter busyWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_get_input"},
			t => t.Type is TokenType.CfIf,
			t => t is IdentifierToken {Name: "busy"},
			t => t.Type is TokenType.Newline
		],allowPartialMatch: true);
		
		MultiTokenWaiter busyWaiter2 = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_get_input"},
			t => t.Type is TokenType.CfIf,
			t => t is IdentifierToken {Name: "busy"},
			t => t.Type is TokenType.ParenthesisOpen,
		],allowPartialMatch: true);
		
		MultiTokenWaiter elseWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken {Name: "_get_input"},
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

		MultiTokenWaiter inputWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "_get_input" },
			t => t.Type is TokenType.ParenthesisOpen,
			t => t.Type is TokenType.ParenthesisClose,
			t => t.Type is TokenType.Colon,
			t => t is IdentifierToken { Name: "_kiss" },
			t => t.Type is TokenType.Newline
		], allowPartialMatch: true);

		MultiTokenWaiter actionReleaseWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "Input" },
			t => t is IdentifierToken { Name: "is_action_just_released" },
			t => t is ConstantToken { Value: StringVariant { Value: "primary_action" } },
			t => t.Type is TokenType.Colon,
		], allowPartialMatch: true);
		
		MultiTokenWaiter actionHoldWaiter = new MultiTokenWaiter([
			t => t is IdentifierToken { Name: "Input" },
			t => t is IdentifierToken { Name: "is_action_pressed" },
			t => t is ConstantToken { Value: StringVariant { Value: "primary_action" } },
			t => t.Type is TokenType.Colon,
		], allowPartialMatch: true);

		MultiTokenWaiter equipWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "_equip_item"},
			t=>t.Type is TokenType.ParenthesisOpen,
			t=>t is IdentifierToken {Name: "item_data"},
			t=>t.Type is TokenType.Comma,
			t=>t is IdentifierToken {Name: "skip_anim"},
			t=>t is IdentifierToken {Name: "forced"},
			t=>t is IdentifierToken {Name: "set_prev"},
			t=>t.Type is TokenType.Colon,
			t=>t.Type is TokenType.CfIf,
			t=>t is IdentifierToken {Name: "state"},
			t=>t.Type is TokenType.OpAnd,
			t=>t.Type is TokenType.OpNot,
			t=>t is IdentifierToken {Name: "forced"},
			t=>t.Type is TokenType.ParenthesisClose,
		], allowPartialMatch: true);
		
		MultiTokenWaiter equipWaiter2 = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "_equip_item"},
			t=>t.Type is TokenType.ParenthesisOpen,
			t=>t is IdentifierToken {Name: "item_data"},
			t=>t.Type is TokenType.Comma,
			t=>t is IdentifierToken {Name: "skip_anim"},
			t=>t is IdentifierToken {Name: "forced"},
			t=>t is IdentifierToken {Name: "set_prev"},
			t=>t.Type is TokenType.Colon,
			t=>t.Type is TokenType.CfIf,
			t=>t is IdentifierToken {Name: "state"},
			t=>t.Type is TokenType.OpAnd,
			t=>t.Type is TokenType.OpNot,
			t=>t is IdentifierToken {Name: "forced"},
			t=>t is IdentifierToken {Name: "held_item"},
			t=>t is IdentifierToken {Name: "item_data"},
			t=>t.Type is TokenType.BracketClose,
		], allowPartialMatch: true);

		MultiTokenWaiter cameraUpdateWaiter = new MultiTokenWaiter([
			t=>t.Type is TokenType.PrVar,
			t=>t is IdentifierToken {Name: "desired_fov"},
			t=>t.Type is TokenType.OpAssign,
			t=>t is ConstantToken {Value: IntVariant {Value: 50}},
		], allowPartialMatch: true);
		
		MultiTokenWaiter equipWaiter3 = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "_equip_item"},
			t=>t.Type is TokenType.ParenthesisOpen,
			t=>t is IdentifierToken {Name: "item_data"},
			t=>t.Type is TokenType.Comma,
			t=>t is IdentifierToken {Name: "skip_anim"},
			t=>t is IdentifierToken {Name: "forced"},
			t=>t is IdentifierToken {Name: "set_prev"},
			t=>t.Type is TokenType.Colon,
			t=>t.Type is TokenType.CfIf,
			t=>t is IdentifierToken {Name: "state"},
			t=>t.Type is TokenType.OpAnd,
			t=>t.Type is TokenType.OpNot,
			t=>t is IdentifierToken {Name: "forced"},
			t=>t.Type is TokenType.Colon,
			t=>t.Type is TokenType.CfReturn,
			t=>t.Type is TokenType.Newline,
		], allowPartialMatch: true);

		MultiTokenWaiter updateCosmeticsWaiter = new MultiTokenWaiter([
			t=>t.Type is TokenType.CfFor,
			t=>t is IdentifierToken {Name: "c"},
			t=>t.Type is TokenType.OpIn,
			t=>t is IdentifierToken {Name: "data"},
			t=>t is IdentifierToken {Name: "keys"},
			t=>t is IdentifierToken {Name: "valid"},
			t=>t is ConstantToken {Value: BoolVariant {Value: false}},
		], allowPartialMatch: true);
		
		foreach (Token token in tokens)
		{
			if (extendsWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("set_fov");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new RealVariant(50));
				yield return new Token(TokenType.Newline);
			}
			else if (readyWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("bind_unequip"));
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("has_action");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("InputEventKey");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("new");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("scancode");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("KEY_QUOTELEFT");
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("add_action");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("action_add_event");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("bind_fov_up"));
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("has_action");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("InputEventKey");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("new");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("scancode");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("KEY_F8");
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("add_action");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("action_add_event");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("bind_fov_down"));
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("has_action");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("InputEventKey");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("new");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("scancode");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("KEY_F7");
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("add_action");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("action_add_event");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
				
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new StringVariant("bind_fov_reset"));
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("has_action");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.PrVar);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("InputEventKey");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("new");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("scancode");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("KEY_F9");
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("add_action");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 2);
				yield return new IdentifierToken("InputMap");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("action_add_event");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("action_name");
				yield return new Token(TokenType.Comma);
				yield return new IdentifierToken("key_event");
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 1);
			}

			else if (actionReleaseWaiter.Check(token) || actionHoldWaiter.Check(token))
			{
				yield return new Token(TokenType.OpAnd);
				yield return new IdentifierToken("hud");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("menu");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new IntVariant(0));
				yield return token;
			}
			
			else if (busyWaiter.Check(token))
			{
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
			}
			else if (elseWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.OpAnd);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("mouse_mode");
				yield return new Token(TokenType.OpNotEqual);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MOUSE_MODE_CONFINED");
			}
			else if (visibleWaiter.Check(token))
			{
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
			else if (inputWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("is_action_just_pressed");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("bind_unequip"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("_equip_item");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("FALLBACK_ITEM");
				yield return new Token(TokenType.ParenthesisClose);

				yield return new Token(TokenType.Newline, 1);
				
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("is_action_pressed");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("bind_fov_up"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("set_fov");
				yield return new Token(TokenType.OpAssignAdd);
				yield return new ConstantToken(new RealVariant(0.25));

				yield return new Token(TokenType.Newline, 1);
				
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("is_action_pressed");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("bind_fov_down"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("set_fov");
				yield return new Token(TokenType.OpAssignSub);
				yield return new ConstantToken(new RealVariant(0.25));

				yield return new Token(TokenType.Newline, 1);
				
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("is_action_pressed");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("bind_fov_reset"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("set_fov");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new RealVariant(50));

				yield return new Token(TokenType.Newline, 1);
			}
			else if (cameraUpdateWaiter.Check(token))
			{
				yield return new IdentifierToken("set_fov");
			}
			else if (equipWaiter.Check(token))
			{
				yield return token;
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.CfReturn);
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.OpAnd);
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new BoolVariant(false));
			}
			else if (equipWaiter2.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.ParenthesisClose);
			}
			else if (equipWaiter3.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("held_item");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("ref"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.OpEqual);
				yield return new IdentifierToken("item_data");
				yield return new Token(TokenType.BracketOpen);
				yield return new ConstantToken(new StringVariant("ref"));
				yield return new Token(TokenType.BracketClose);
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("item_data");
				yield return new Token(TokenType.OpAssign);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("FALLBACK_ITEM");

				yield return new Token(TokenType.Newline, 1);
			}
			else if (updateCosmeticsWaiter.Check(token))
			{
				yield return token;

				yield return new Token(TokenType.Newline, 4);
				
				yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextPrint);
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("owner_id");
				yield return new Token(TokenType.ParenthesisClose);
				
				yield return new Token(TokenType.Newline, 4);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.ParenthesisOpen);

				yield return new IdentifierToken("c");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("NeoQOLPack.eye_koni"));
				yield return new Token(TokenType.OpOr);
				
				yield return new IdentifierToken("c");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("NeoQOLPack.koni_antennae"));
				yield return new Token(TokenType.OpOr);
				
				yield return new IdentifierToken("c");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("NeoQOLPack.koni_cheeks"));
				yield return new Token(TokenType.OpOr);
				
				yield return new IdentifierToken("c");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("NeoQOLPack.koni_fluff"));
				yield return new Token(TokenType.OpOr);
				
				yield return new IdentifierToken("c");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("NeoQOLPack.koni_pattern"));
				yield return new Token(TokenType.OpOr);
				
				yield return new IdentifierToken("c");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("NeoQOLPack.koni_wings"));
				yield return new Token(TokenType.OpOr);
				
				yield return new IdentifierToken("c");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("NeoQOLPack.pcolor_koni1"));
				yield return new Token(TokenType.OpOr);
				
				yield return new IdentifierToken("c");
				yield return new Token(TokenType.OpEqual);
				yield return new ConstantToken(new StringVariant("NeoQOLPack.pcolor_koni2"));
				
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.OpAnd);
				yield return new IdentifierToken("owner_id");
				yield return new Token(TokenType.OpNotEqual);
				yield return new ConstantToken(new IntVariant(76561198244258834, is64: true));
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 5);
				yield return new IdentifierToken("valid");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new BoolVariant(false));
			}
			else yield return token;
		}
	}
}