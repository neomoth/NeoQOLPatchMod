// namespace NeoQOLPack;
//
// public class FileDownloader(Mod mod)
// {
// 	private static readonly HttpClient HttpClient = new();
//
// 	public async Task DownloadFileAsync(string url, string outputPath)
// 	{
// 		try
// 		{
// 			Directory.CreateDirectory(outputPath);
// 			string fileName = Path.GetFileName(url);
// 			string destinationPath = Path.Combine(outputPath, fileName);
//
// 			using var response = await HttpClient.GetAsync(url);
// 			response.EnsureSuccessStatusCode();
// 			byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
// 			await File.WriteAllBytesAsync(destinationPath, fileBytes);
// 			mod.Logger.Information($"file {fileName} downloaded to {outputPath}");
// 		}
// 		catch (HttpRequestException e)
// 		{
// 			mod.Logger.Error(e.Message);
// 		}
// 		catch (Exception e)
// 		{
// 			mod.Logger.Error(e.Message);
// 		}
// 	}
// }