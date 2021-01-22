using System.Collections.Generic;

namespace YMMHRSystemLogic
{
    public class DtoPermission
    {
        public long TaskCategoryId { get; set; }
        public string Name { get; set; }
        public long FatherCategoryId { get; set; }
        public string Icon { get; set; }  
        public string Url { get; set; }
        public List<DtoTask> TaskList  { get; set; } = new List<DtoTask>();
            
    }
}
