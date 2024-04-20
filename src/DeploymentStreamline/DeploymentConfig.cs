using System.Text.Json.Serialization;

namespace DeploymentStreamline
{
    internal class DeploymentConfig
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("resourceGroup")]
        public string ResourceGroup { get; set; }

        [JsonPropertyName("templateFile")]
        public string TemplateFile { get; set; }

        [JsonPropertyName("parametersFile")]
        public string ParametersFile { get; set; }
    }
}
