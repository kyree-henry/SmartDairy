using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using SentimentAnalyzing.Services;
using SmartDiary.Data;
using SmartDiary.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDiary.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppDBContext _context;
        public IndexModel(ILogger<IndexModel> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public Diary Entry { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(Diary entry)
        {
            if (ModelState.IsValid)
            {
                MLContext mLContext = new MLContext();
                var result = mLContext.GetSentiment(entry.Note);

                entry.Sentiment = result.Probability;
                _context.Diaries.Add(entry);
                await _context.SaveChangesAsync();
                return RedirectToPage(nameof(Index));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(Guid? id)
        {
            if (id == null)
                return BadRequest();

            var data = await _context.Diaries.SingleOrDefaultAsync(a => a.Id == id);
            if (data == null)
                return NotFound();

            _context.Diaries.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToPage(nameof(Index));
        }
    }
}
