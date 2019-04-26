using System.ComponentModel;

namespace TaskManager.DAL.Models.Enums
{
    public enum Status
    {
        [Description("TO DO")]
        ToDo,
        [Description("Active")]
        Active,
        [Description("Reopened")]
        Reopened,
        [Description("Progress")]
        Progress,
        [Description("Closed")]
        Closed,
    }
}
