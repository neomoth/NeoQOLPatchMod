using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using GDWeave;
using GDWeave.Godot;
using GDWeave.Godot.Variants;
using NeoQOLPack.Mods;
using Newtonsoft.Json.Linq;
using Serilog;
// using System.Runtime.InteropServices;

namespace NeoQOLPack;

public class Mod : IMod
{
	public IModInterface modInterface;
	public Config Config;
	public ILogger Logger;

	private static readonly string versionTag = "Beta2";
	private static readonly string repo = "neomoth/NeoQOLPack";

	private bool injectUpdateNotice = false;
	
	public Mod(IModInterface modInterface) {
		this.modInterface = modInterface;
		this.Config = modInterface.ReadConfig<Config>();
		Logger = modInterface.Logger;
		_ = GetVersion();
		modInterface.RegisterScriptMod(new InventoryStackerItem(this));
		modInterface.RegisterScriptMod(new InventoryStackerPlayerData(this));
		modInterface.RegisterScriptMod(new InventoryStackerInventory(this));
		modInterface.RegisterScriptMod(new InventoryStackerSelect(this));
		modInterface.RegisterScriptMod(new CosmeticLoaderGlobals(this));
		modInterface.RegisterScriptMod(new CosmeticLoaderPlayer(this));
		modInterface.RegisterScriptMod(new CosmeticLoaderTitle());
		// modInterface.RegisterScriptMod(new ModScriptPatcher(this, versionTag, injectUpdateNotice));
		modInterface.RegisterScriptMod(new ShopPatcher());
		modInterface.RegisterScriptMod(new PlayerHudPatcher(this));
		modInterface.RegisterScriptMod(new ShopButtonPatcher(this));
		modInterface.RegisterScriptMod(new MenuPatcher(this, versionTag));
		if (injectUpdateNotice) ;
	}

	// [DllImport("user32.dll", SetLastError = true)]
	// private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint uType);
	//
	// private const uint MB_OK = 0x00000000;
	// private const uint MB_OKCANCEL = 0x00000001;
	// private const uint MB_YESNO = 0x00000004;
	// private const uint MB_ICONINFORMATION = 0x00000040;
	// private const uint MB_ICONWARNING = 0x00000030;
	// private const uint MB_ICONERROR = 0x00000010;
	// private const uint MB_ICONQUESTION = 0x00000020;
	// private const uint MB_SYSTEMMODAL = 0x00001000; // Make it a system modal dialog
	// private const uint MB_SETFOREGROUND = 0x00010000; // Bring the message box to the foreground
	//
	// private static int ShowMessage(string message, string title, uint buttonType, uint iconType)
	// {
	// 	uint options = buttonType | iconType | MB_SYSTEMMODAL | MB_SETFOREGROUND;
	// 	return MessageBox(IntPtr.Zero, message, title, options);
	// }
	
	private async Task GetVersion()
	{
		modInterface.Logger.Information("Neo's QOL Pack loaded!! :3");
		try
		{
			// modInterface.Logger.Information($"This should run before");
			JsonDocument? githubInfoRaw = await GetLatestRelease(modInterface.Logger);
			JsonElement? githubInfo = githubInfoRaw?.RootElement;
			var latestVersion = githubInfo?.GetProperty("tag_name").GetString();
			// modInterface.Logger.Information($"This should run after");
			modInterface.Logger.Information($"Latest version: {latestVersion}");
			if ((bool)latestVersion?.StartsWith("Beta"))
			{
				var newVer = Int32.Parse(Regex.Match(latestVersion, @"\d+").Value);
				var currVer = Int32.Parse(Regex.Match(versionTag, @"\d+").Value);

				injectUpdateNotice = newVer > currVer;
				// int result = ShowMessage("The version of the mod you are using is out of date.\nWould you like to download the latest release?","Mod out of date!", MB_YESNO, MB_ICONWARNING);
				// if (result == 0)
				// {
				// 	FileDownloader downloader = new FileDownloader(this);
				// 	var url = githubInfo?.GetProperty("assets").GetProperty("0").GetProperty("browser_download_url").GetString();
				// 	bool isWine = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WINEPREFIX"));
				// 	string path = isWine ? "/home/neomoth/Downloads/gwagwa" : "C:\\users\\steamuser\\Downloads\\gwagwa";
				// 	if (url is null)
				// 	{
				// 		modInterface.Logger.Warning("Failed to find release zip");
				// 		return;
				// 	}
				// 	await downloader.DownloadFileAsync(url, path);
				// 	var a = ShowMessage("New version downloaded. Please relaunch the game.","Success", MB_OK, MB_ICONINFORMATION);
				// }
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

		// logger.Debug("Testing dns...");
		await Dns.GetHostEntryAsync("api.github.com");
		// logger.Debug("creating HTTP Client...");
		try
		{
			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromSeconds(10);
			
			// logger.Debug("adding headers...");
			client.DefaultRequestHeaders.Add("User-Agent", "C# App");
			// logger.Debug("awaiting response...");
			HttpResponseMessage response = await client.GetAsync(apiUrl).ConfigureAwait(false);
			// logger.Debug("got response.");
			response.EnsureSuccessStatusCode();
			
			if (response.Headers.Contains("X-RateLimit-Remaining"))
			{
				string? remainingRequests = response.Headers.GetValues("X-RateLimit-Remaining").FirstOrDefault();
				// logger.Debug($"GitHub API Rate Limit Remaining: {remainingRequests}");
			}
			
			if (!response.IsSuccessStatusCode){
				logger.Error("Unable to connect to github.");
				return null; // Unable to connect to github for some reason
			}
			// logger.Debug("valid. getting response data...");
			string json = await response.Content.ReadAsStringAsync();
			
			// logger.Debug("parsing json...");
			return JsonDocument.Parse(json);
			// logger.Debug("retrieved info.");
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
