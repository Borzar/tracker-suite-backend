namespace TrackerSuite.Core.Models.Output;

public class JsonResponse
{
    public string Status { get; set; }
    public string Description { get; set; }
    public List<TaskItemOutput> Result { get; set; }

}