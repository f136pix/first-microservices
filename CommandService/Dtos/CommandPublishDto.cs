namespace CommandService.Dtos;

public class CommandPublishDto
{

    public string Id { get; set; }

    public string HowTo { get; set; }

    public string CommandLine { get; set; }

    public int PlatformId { get; set; }

    public String Event { get; set; }
}
