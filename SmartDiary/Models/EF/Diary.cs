using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDiary.Models.EF
{
    public class Diary : AppAudit
    {
        [Display(Name = "On this Day")][Required]
        public string Note { get; set; }
        public float Sentiment { get; set; }
    }
}
