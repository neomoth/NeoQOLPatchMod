using GDWeave;
using GDWeave.Godot;
using GDWeave.Godot.Variants;
using NeoQOLPack.Mods;
using Serilog;

namespace NeoQOLPack;

public class Mod : IMod {
	public Config Config;
	public ILogger Logger;

	public Mod(IModInterface modInterface) {
		this.Config = modInterface.ReadConfig<Config>();
		Logger = modInterface.Logger;
		modInterface.Logger.Information("Hello, world!");
		modInterface.RegisterScriptMod(new InventoryStackerItem(this));
		modInterface.RegisterScriptMod(new InventoryStackerPlayerData(this));
		modInterface.RegisterScriptMod(new InventoryStackerInventory(this));
		modInterface.RegisterScriptMod(new InventoryStackerSelect(this));
		modInterface.RegisterScriptMod(new CosmeticLoaderGlobals(this));
		modInterface.RegisterScriptMod(new CosmeticLoaderPlayer(this));
		modInterface.RegisterScriptMod(new CosmeticLoaderTitle());
	}

	public IEnumerable<Token> GetMod()
	{
		yield return new Token(TokenType.Dollar);
		yield return new ConstantToken(new StringVariant("/root/NeoQOLPack"));
	}	
	
	public void Dispose() {
		// Cleanup anything you do here
	}
}
