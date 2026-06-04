namespace TrackerSuite.Core.Dto.Output;

public class JsonResponseDto
{
    public string Status { get; set; }
    public string Description { get; set; }
    public List<TaskItemOutputDto> Result { get; set; }

}