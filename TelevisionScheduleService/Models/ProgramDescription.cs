using System.Text.Json.Serialization;

namespace TelevisionScheduleService.Models
{
    internal class ProgramDescription
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("start")]
        public long StartTimeUnix { get; set; }

        [JsonPropertyName("stop")]
        public long StopTimeUnix { get; set; }

        [JsonPropertyName("categories")]
        public string[] Categories { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("availableAsVod")]
        public bool IsAvailableAsVod { get; set; }

        [JsonPropertyName("rerun")]
        public bool IsRerun { get; set; }

        [JsonPropertyName("premiere")]
        public bool IsPremiere { get; set; }

        [JsonPropertyName("live")]
        public bool IsLive { get; set; }
    }
}
