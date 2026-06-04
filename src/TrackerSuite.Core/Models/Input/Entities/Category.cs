using TrackerSuite.Core.Models.Input;

namespace TrackerSuite.Core.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
