using SmartDiary.Models.EF;
using SmartDiary.Models.SQLQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDiary.Services.Interfaces
{
    public interface ISmartDiarySQLRepository
    {
        Task<QueryResult> AddEntryAsync(Diary entry);
        Task<IEnumerable<Diary>> GetDiaryEntriesAsync();
        Task<QueryResult> DeleteEntry(int id);
        Task<QueryResult> UpdateEntryAsync(Diary entry);
        Task<Diary> GetDiaryByIdAsync(Guid id);

        float Prediction(string note);
    }
}
