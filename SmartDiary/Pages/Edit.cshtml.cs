using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using SentimentAnalyzing.Services;
using SmartDiary.Data;
using SmartDiary.Models.EF;

namespace SmartDiary.Pages
{
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly AppDBContext _context;
        public EditModel(ILogger<EditModel> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public Diary Entry { get; set; }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
                return BadRequest();

            var data = await _context.Diaries.SingleOrDefaultAsync(a => a.Id == id);

            if (data == null)
                return NotFound();

            Entry = data;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            MLContext mLContext = new MLContext();
            var result = mLContext.GetSentiment(Entry.Note);

            Entry.Sentiment = result.Probability;
            _context.Diaries.Update(Entry);
            await _context.SaveChangesAsync();
            return RedirectToPage(nameof(Index));
        }
    }
}
