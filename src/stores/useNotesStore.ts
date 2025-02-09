import { defineStore } from 'pinia';
import api from '../services/api.ts';

export const useNotesStore = defineStore('notes', {
  state: () => ({
    notes: [] as Array<{ id: number; title: string; content: string; createdAt: string; updatedAt: string }>,
  }),
  actions: {
    async fetchNotes() {
      const { data } = await api.get('/notes');
      this.notes = data;
    },
    async createNote(note: { title: string; content: string }) {
      const { data } = await api.post('/notes', note);
      this.notes.push(data);
    },
    async updateNote(id: number, note: { title: string; content: string }) {
      await api.put(`/notes/${id}`, note);
      this.fetchNotes();
    },
    async deleteNote(id: number) {
      await api.delete(`/notes/${id}`);
      this.notes = this.notes.filter((note) => note.id !== id);
    },
  },
});
