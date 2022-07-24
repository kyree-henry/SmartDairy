using FluentValidation;
using SmartDiary.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDiary.Domain.Validators
{
    public class DiaryValidation : AbstractValidator<Diary>
    {
        public DiaryValidation()
        {
            RuleFor(x => x.Note).NotEmpty();
        }
    }
}
