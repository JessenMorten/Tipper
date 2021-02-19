using System.Text.Json.Serialization;

namespace TelevisionScheduleService.Models
{
    internal class ProgramSchedule
    {
        [JsonPropertyName("id")]
        public string ChannelId { get; set; }

        [JsonPropertyName("programs")]
        public ProgramDescription[] ProgramDescriptions { get; set; }
    }
}
