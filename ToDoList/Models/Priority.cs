namespace ToDoList.Models
{
    public class Priority
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<ToDoTask>? Tasks { get; set; }

    }
}
