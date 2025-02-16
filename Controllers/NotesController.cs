using API_BackEnd.Repositories;
using API_BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;

        public NotesController(NoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        // ✅ 1. Create a Note 
        // POST /api/notes/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateNoteAsync([FromBody] NoteModel note)
        {
            if (note == null || string.IsNullOrWhiteSpace(note.Title))
                return BadRequest("Note title is required.");

            var noteId = await _noteRepository.CreateNoteAsync(note);
            if (noteId <= 0)
                return BadRequest("Failed to create note.");

            return Ok(new { message = "Note created successfully", noteId });
        }



        // ✅ 2. Get All Notes 
        [HttpGet("all_Notes")]
        public async Task<IActionResult> GetAllNotesAsync()
        {
            var notes = await _noteRepository.GetAllNotesAsync();
            return Ok(notes);
        }

        // ✅ 3. Get a Single Note by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteByIdAsync(int id)
        {
            var note = await _noteRepository.GetNoteByIdAsync(id);
            if (note == null) return NotFound();

            return Ok(note);
        }

        // ✅ 4. Update a Note
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNoteAsync(int id, [FromBody] NoteModel note)
        {
            if (note == null || string.IsNullOrWhiteSpace(note.Title))
                return BadRequest("Title is required.");

            // Update the note in the database
            var updated = await _noteRepository.UpdateNoteAsync(id, note.Title, note.Content);

            if (!updated)
                return NotFound();  // Return 404 if the note was not found

            // Fetch the updated note to return it to the frontend
            var updatedNote = await _noteRepository.GetNoteByIdAsync(id);

            if (updatedNote == null)
                return NotFound();  // Handle case where the note still doesn't exist after update

            // Return the updated note data
            return Ok(new
            {
                id = updatedNote.Id,
                title = updatedNote.Title,
                content = updatedNote.Content,
                updatedAt = updatedNote.UpdatedAt
            });
        }


        // ✅ 5. Delete a Note
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNoteAsync(int id)
        {
            var deleted = await _noteRepository.DeleteNoteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}



