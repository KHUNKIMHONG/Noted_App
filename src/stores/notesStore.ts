import { defineStore } from 'pinia';
import api from '../services/api.ts';

// Define the Note type for better type safety
interface Note {
  id: number;
  title: string;
  content: string;
  createdAt: string;
  updatedAt: string;
}

export const useNotesStore = defineStore('notes', {
  state: () => ({
    notes: [] as Note[],  // Use the Note type here
  }),
  actions: {
    // Fetch all notes and handle errors
    async fetchNotes() {
      try {
        const { data } = await api.get('/notes');
        this.notes = data;
      } catch (error) {
        console.error('Error fetching notes:', error);
      }
    },

    // Create a new note
    async createNote(note: Omit<Note, 'id' | 'createdAt' | 'updatedAt'>) {
      try {
        const { data } = await api.post('/notes', note);
        this.notes.push(data); // Add new note to the notes array
      } catch (error) {
        console.error('Error creating note:', error);
      }
    },

    // Update an existing note
    async updateNote(id: number, note: { title: string; content: string }) {
      try {
        const { data } = await api.put(`/notes/${id}`, note);
        
        // Find the note and update it in the array instead of refetching all notes
        const index = this.notes.findIndex((n) => n.id === id);
        if (index !== -1) {
          this.notes[index] = { ...this.notes[index], ...data };
        }
      } catch (error) {
        console.error('Error updating note:', error);
      }
    },

    // Delete a note
    async deleteNote(id: number) {
      try {
        await api.delete(`/notes/${id}`);
        this.notes = this.notes.filter((note) => note.id !== id); // Remove the note from the state
      } catch (error) {
        console.error('Error deleting note:', error);
      }
    },
  },
});

