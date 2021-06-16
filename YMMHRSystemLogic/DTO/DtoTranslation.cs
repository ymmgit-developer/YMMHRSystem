using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoTranslation
    {
        public long TranslationId { get; set; }
        public string Associate { get; set; }
        public string Issue { get; set; }
        public string Description { get; set; }
        public string ComplexityLevel { get; set; }
        public int Status { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? FinalDate { get; set; }
        public DateTime? DateAdded { get; set; }
        public long CreatedBy { get; set; }
        public List<DtoTranslationAttachment> AttachmentList { get; set; } = new List<DtoTranslationAttachment>();
        public List<DtoTranslationAttachment> TranslatedList { get; set; } = new List<DtoTranslationAttachment>();
    }
}
