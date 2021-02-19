using System.Text.Json.Serialization;

namespace TelevisionScheduleService.Models
{
    internal class GetChannelsResponse
    {
        [JsonPropertyName("channels")]
        public GetChannelsResponseItem[] Channels { get; set; }
    }
}
