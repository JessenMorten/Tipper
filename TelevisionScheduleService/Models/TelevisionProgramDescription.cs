namespace TelevisionScheduleService.Models
{
    public class TelevisionProgramDescription
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public long StartTimeUnix { get; set; }

        public long StopTimeUnix { get; set; }
    }
}
