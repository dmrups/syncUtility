using System;

namespace WindowsFormsApp1.Entities
{
    class TaskReport
    {
        public DateTime Date { get; set; }

        public TimeSpan Duration { get; set; }

        public string TaskId { get; set; }

        public string Description { get; set; }

        public string ProjectName { get; set; }
    }
}
