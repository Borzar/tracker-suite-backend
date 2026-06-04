using TrackerSuite.Core.Models.Input;

namespace TrackerSuite.Core.Models.input;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
