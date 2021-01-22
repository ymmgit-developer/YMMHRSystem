namespace YMMHRSystemLogic
{
    public class DtoTask
    {
        public long TaskId { get; set; }
        public string Name { get; set; }
        public long TaskCategoryId { get; set; }
        public bool hasCheck { get; set; } = false;
    }
}
