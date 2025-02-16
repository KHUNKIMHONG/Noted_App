using API_BackEnd.Data;
using API_BackEnd.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace API_BackEnd.Repositories
{
    public class NoteRepository
    {
        private readonly IDbConnection _dbConnection;

        public NoteRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        // ✅ Create a new note (No userId)
        public async Task<int> CreateNoteAsync(NoteModel note)
        {
            string query = @"
                INSERT INTO Notes (Title, Content, CreatedAt, UpdatedAt) 
                VALUES (@Title, @Content, @CreatedAt, @UpdatedAt); 
                SELECT SCOPE_IDENTITY();";

            var parameters = new
            {
                Title = note.Title,
                Content = note.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _dbConnection.ExecuteScalarAsync<int>(query, parameters);
        }

        // ✅ Get all notes (No userId)
        public async Task<IEnumerable<NoteModel>> GetAllNotesAsync()
        {
            string query = "SELECT * FROM Notes";
            return await _dbConnection.QueryAsync<NoteModel>(query);
        }

        // ✅ Get a specific note by ID
        public async Task<NoteModel> GetNoteByIdAsync(int id)
        {
            string query = "SELECT * FROM Notes WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<NoteModel>(query, new { Id = id });
        }

        // ✅ Update a note (No userId)
        public async Task<bool> UpdateNoteAsync(int id, string title, string content)
        {
            string query = @"
                UPDATE Notes 
                SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt 
                WHERE Id = @Id";

            var affectedRows = await _dbConnection.ExecuteAsync(query, new
            {
                Id = id,
                Title = title,
                Content = content,
                UpdatedAt = DateTime.UtcNow
            });

            return affectedRows > 0;
        }

        // ✅ Delete a note (No userId)
        public async Task<bool> DeleteNoteAsync(int id)
        {
            string query = "DELETE FROM Notes WHERE Id = @Id";
            var affectedRows = await _dbConnection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}




