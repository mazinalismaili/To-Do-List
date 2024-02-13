namespace ToDoList.Models
{
    public class ToDoTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool completed { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public string StatusId { get; set; }
        public string PriorityId { get; set; }
        public string UserId { get; set; }


        public User User { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }

    }
}
