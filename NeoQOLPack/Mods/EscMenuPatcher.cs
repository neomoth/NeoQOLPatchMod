using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NeoQOLPack.Mods;

public class EscMenuPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/HUD/Esc Menu/esc_menu.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		//$VBoxContainer.anchor_top = 0.35 if Network.PLAYING_OFFLINE else 0.32
		// $VBoxContainer.anchor_bottom = 0.65 if Network.PLAYING_OFFLINE else 0.68

		MultiTokenWaiter processWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "_process"},
			t=>t.Type is TokenType.Newline
		], allowPartialMatch:true);
		
		MultiTokenWaiter openWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "_open"},
			t=>t.Type is TokenType.Colon,
			t=>t.Type is TokenType.Newline
		], allowPartialMatch:true);
		
		MultiTokenWaiter closeWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "_close"},
			t=>t.Type is TokenType.Colon,
			t=>t.Type is TokenType.Newline
		], allowPartialMatch:true);

		foreach (Token token in tokens)
		{
			if (processWaiter.Check(token))
			{
				yield return token;

				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("VBoxContainer"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("anchor_top");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new RealVariant(0.35));
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("Network");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("PLAYING_OFFLINE");
				yield return new Token(TokenType.CfElse);
				yield return new ConstantToken(new RealVariant(0.32));
				yield return new Token(TokenType.Newline, 1);
				yield return new IdentifierToken("get_node");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("VBoxContainer"));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("anchor_bottom");
				yield return new Token(TokenType.OpAssign);
				yield return new ConstantToken(new RealVariant(0.65));
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("Network");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("PLAYING_OFFLINE");
				yield return new Token(TokenType.CfElse);
				yield return new ConstantToken(new RealVariant(0.68));
									
				yield return new Token(TokenType.Newline, 1);
			}
			else if (openWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("player_options");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("lockmouse");
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("set_mouse_mode");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MOUSE_MODE_VISIBLE");
				yield return new Token(TokenType.ParenthesisClose);
				
				yield return new Token(TokenType.Newline, 1);
			}
			else if (closeWaiter.Check(token))
			{
				yield return token;
				
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("player_options");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("lockmouse");
				yield return new Token(TokenType.Colon);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("set_mouse_mode");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new IdentifierToken("Input");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("MOUSE_MODE_CONFINED");
				yield return new Token(TokenType.ParenthesisClose);
				
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}