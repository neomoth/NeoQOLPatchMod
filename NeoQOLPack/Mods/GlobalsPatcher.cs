// using GDWeave.Godot;
// using GDWeave.Godot.Variants;
// using GDWeave.Modding;
//
// namespace NeoQOLPack.Mods;
//
// public class GlobalsPatcher(Mod mod) : IScriptMod
// {
// 	public bool ShouldRun(string path) => path == "res://Scenes/Singletons/globals.gdc";
//
// 	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
// 	{
// 		MultiTokenWaiter readyWaiter = new MultiTokenWaiter([
// 			t => t is IdentifierToken {Name: "_ready"},
// 			// t => t is IdentifierToken {Name: "_load_resources"},
// 			t => t.Type == TokenType.Newline
// 		], allowPartialMatch:true);
//
// 		foreach (Token token in tokens)
// 		{
// 			if (readyWaiter.Check(token))
// 			{
// 				yield return token;
// 				
// 				yield return new Token(TokenType.Newline, 1);
// 			}
// 			else yield return token;
// 		}
// 	}
// }