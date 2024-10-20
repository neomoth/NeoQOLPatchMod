using System.Text.Json.Serialization;

namespace NeoQOLPack;

public class Config {
    [JsonInclude] public bool SomeSetting = true;
}
