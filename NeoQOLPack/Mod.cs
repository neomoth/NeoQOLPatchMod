using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using GDWeave;
using GDWeave.Godot;
using GDWeave.Godot.Variants;
using NeoQOLPack.Mods;
using Serilog;
using System.IO;

namespace NeoQOLPack;

public class Mod : IMod
{
	public IModInterface modInterface;
	public Config Config;
	public ILogger Logger;

	private static string versionTag = "v0";
	private static readonly string repo = "neomoth/NeoQOLPack";

	private bool injectUpdateNotice = false;
	
	public Mod(IModInterface modInterface) {
		this.modInterface = modInterface;
		this.Config = modInterface.ReadConfig<Config>();
		Logger = modInterface.Logger;
		_ = GetVersion();
		modInterface.RegisterScriptMod(new PlayerDataPatcher(this));
		modInterface.RegisterScriptMod(new PlayerPatcher(this));
		modInterface.RegisterScriptMod(new ShopPatcher());
		modInterface.RegisterScriptMod(new PlayerHudPatcher(this));
		modInterface.RegisterScriptMod(new ShopButtonPatcher(this));
		modInterface.RegisterScriptMod(new MenuPatcher(this, versionTag));
		modInterface.RegisterScriptMod(new OptionsMenuPatcher(this));
		modInterface.RegisterScriptMod(new EscMenuPatcher());
		// modInterface.RegisterScriptMod(new CosmeticMenuPatcher());
		modInterface.RegisterScriptMod(new SellAllButtonPatcher());
		modInterface.RegisterScriptMod(new PlayerLabelPatcher());
		modInterface.RegisterScriptMod(new InventoryItemPatcher());
		if (injectUpdateNotice) ;
	}
	
	private async Task GetVersion()
	{
		modInterface.Logger.Information("Neo's QOL Pack loaded!! :3");

		try
		{
			string? currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			if (currentDir is null)
				throw new NullReferenceException(
					"if this gets thrown i have no fucking idea how you managed to do that.");
			string jsonPath = Path.Combine(currentDir, "manifest.json");
			if (!File.Exists(jsonPath))
				throw new FileNotFoundException("manifest.json was not found in the mod's directory.");
			string jsonContent = File.ReadAllText(jsonPath);
			JsonDocument document = JsonDocument.Parse(jsonContent);
			if (document.RootElement.GetProperty("Metadata").TryGetProperty("Version", out JsonElement version))
			{
				string? versionAsString = version.GetString();
				if (versionAsString is not null) versionTag = $"v{versionAsString}";
			}
			else throw new Exception("'version' property not found in manifest.json.");
		}
		catch (Exception e)
		{
			Logger.Error(e.Message);
		}
		
		try
		{
			JsonDocument? githubInfoRaw = await GetLatestRelease(modInterface.Logger);
			JsonElement? githubInfo = githubInfoRaw?.RootElement;
			var latestVersion = githubInfo?.GetProperty("tag_name").GetString();
			modInterface.Logger.Information($"Latest version: {latestVersion}");
			if ((bool)latestVersion?.StartsWith("Beta"))
			{
				var newVer = Int32.Parse(Regex.Match(latestVersion, @"\d+").Value);
				var currVer = Int32.Parse(Regex.Match(versionTag, @"\d+").Value);

				injectUpdateNotice = newVer > currVer;
			}
			else
			{
				Version latest = new Version(latestVersion.StartsWith('v') ? latestVersion[1..] : latestVersion);
				Version current = new Version(versionTag.StartsWith('v') ? versionTag[1..] : versionTag);

				int comparison = latest.CompareTo(current);

				injectUpdateNotice = comparison > 0;
			}
		}
		catch (Exception e)
		{
			modInterface.Logger.Error(e.Message);
		}
	}

	private static async Task<JsonDocument?> GetLatestRelease(ILogger logger)
	{
		logger.Information("Getting latest release...");
		string apiUrl = $"https://api.github.com/repos/neomoth/NeoQOLPatchMod/releases/latest";

		await Dns.GetHostEntryAsync("api.github.com");
		try
		{
			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromSeconds(10);
			client.DefaultRequestHeaders.Add("User-Agent", "C# App");
			HttpResponseMessage response = await client.GetAsync(apiUrl).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();
			
			// if (response.Headers.Contains("X-RateLimit-Remaining"))
			// {
			// 	string? remainingRequests = response.Headers.GetValues("X-RateLimit-Remaining").FirstOrDefault();
			// }
			
			if (!response.IsSuccessStatusCode){
				logger.Error("Unable to connect to github.");
				return null;
			}
			string json = await response.Content.ReadAsStringAsync();
			
			return JsonDocument.Parse(json);
		}
		catch (HttpRequestException e)
		{
			throw new Exception($"Http error: {e.Message}");
		}
		catch (TaskCanceledException e)
		{
			throw new Exception($"Task cancelled: {e.Message}");
		}
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
