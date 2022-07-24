using Microsoft.EntityFrameworkCore;
using SmartDiary.Data;
using SmartDiary.Models.EF;
using SmartDiary.Models.SQLQuery;
using SmartDiary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDiary.Services.Repositories
{
    public class SmartDiarySQLRepository : ISmartDiarySQLRepository
    {
        private readonly AppDBContext _context;


        public SmartDiarySQLRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<QueryResult> AddEntryAsync(Diary entry)
        {
            try
            {
                var query = await _context.Diaries.AddAsync(entry);
                var result = await _context.SaveChangesAsync();

                return result > 0 ? new QueryResult() { Succeeded = true } : new QueryResult()
                {
                    Succeeded = false,
                    Errors = new List<QueryError>()
                     {
                         new QueryError()
                         {
                             Code = "",
                             Description = "An error occured when inserting to database"
                         }
                     }
                };
            }
            catch (Exception ex)
            {
                // dont forget to log this exception message
                return new QueryResult()
                {
                   Succeeded = false,
                   Errors = new List<QueryError>()
                   {
                       new QueryError()
                       {
                           Code = "",
                           Description = ex.Message
                       }
                   }
                };
            }
        }

        public async Task<QueryResult> DeleteEntry(int id)
        {
            try
            {
                var data = await _context.Diaries.FindAsync(id);
                if (data != null)
                {
                    _context.Diaries.Remove(data);
                    var result = _context.SaveChanges();
                    return result > 0 ? new QueryResult() { Succeeded = true } : new QueryResult()
                    {
                        Succeeded = false,
                        Errors = new List<QueryError>()
                     {
                         new QueryError()
                         {
                             Code = "",
                             Description = "An error occured when removing from database"
                         }
                     }
                    };

                }
                else
                {
                    return new QueryResult()
                    {
                        Succeeded = false,
                        Errors = new List<QueryError>()
                        {
                            new QueryError()
                            {
                                Code = "",
                                Description = "An error occured when removing from database"
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                // dont forget to log this exception message
                return new QueryResult()
                {
                    Succeeded = false,
                    Errors = new List<QueryError>()
                    {
                        new QueryError()
                        {
                            Code = "",
                            Description = ex.Message
                        }
                    }
                };
            }
        }

        public async Task<Diary> GetDiaryByIdAsync(Guid id)
        {
            return await _context.Diaries.FindAsync(id);
        }

        public async Task<IEnumerable<Diary>> GetDiaryEntriesAsync()
        {
            return await _context.Diaries.OrderByDescending(x => x.CreatedOn).ToListAsync();
        }

        public float Prediction(string note)
        {
            throw new NotImplementedException();
        }

        public async Task<QueryResult> UpdateEntryAsync(Diary entry)
        {
            try
            {
                var query = _context.Diaries.Update(entry);
                var result = await _context.SaveChangesAsync();

                return result > 0 ? new QueryResult() { Succeeded = true } : new QueryResult()
                {
                    Succeeded = false,
                    Errors = new List<QueryError>()
                     {
                         new QueryError()
                         {
                             Code = "",
                             Description = "An error occured when updating changes"
                         }
                     }
                };
            }
            catch (Exception ex)
            {
                // dont forget to log this exception message
                return new QueryResult()
                {
                    Succeeded = false,
                    Errors = new List<QueryError>()
                   {
                       new QueryError()
                       {
                           Code = "",
                           Description = ex.Message
                       }
                   }
                };
            }
        }
    }
}
