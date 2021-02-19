using System.Text.Json.Serialization;

namespace TelevisionScheduleService.Models
{
    internal class GetChannelsResponseItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("icon")]
        public string IconUrl { get; set; }

        [JsonPropertyName("logo")]
        public string LogoUrl { get; set; }

        [JsonPropertyName("svgLogo")]
        public string SvgLogoUrl { get; set; }

        [JsonPropertyName("sort")]
        public int Sort { get; set; }

        [JsonPropertyName("lang")]
        public string Language { get; set; }
    }
}
