<template>
    <div class="min-h-screen bg-gray-300 flex flex-col items-center">
        <!-- Top Bar -->
        <div class="w-full max-w-4xl flex flex-wrap gap-4 justify-between items-center">
            <!-- Search Input -->
            <input v-model="searchQuery" type="text" placeholder="Search by title or content"
                class="w-full sm:w-auto flex-1 p-2 border-2 border-blue-500 rounded-md focus:outline-none hover:border-2 hover:border-emerald-400" />

            <!-- Date Filter -->
            <input v-model="searchDate" type="date"
                class="p-2 border-2 border-blue-500 rounded-md focus:outline-none hover:border-2 hover:border-emerald-400" />

            <!-- Sort by Newest/Oldest -->
            <select v-model="sortOrder"
                class="p-2 border-2 border-blue-500 rounded-md focus:outline-none hover:border-2 hover:border-emerald-400">
                <option value="desc">Newest First</option>
                <option value="asc">Oldest First</option>
            </select>

            <!-- Add Note Button -->
            <button @click="showAddModal = true"
                class="px-4 py-2 bg-emerald-500 text-white rounded-md shadow-lg hover:bg-emerald-600 transition">
                + Add Note
            </button>
        </div>

        <!-- Notes Grid -->
        <div class="w-full max-w-4xl mt-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
            <!-- Display message if user doesn't have notes yet -->
            <div v-if="notesStore.notes.length === 0"
                class="col-span-1 sm:col-span-2 lg:col-span-3 flex flex-col items-center justify-center text-center text-gray-500 min-h-[200px]">
                <p class="text-2xl font-semibold text-gray-700 mb-4">
                    You have no notes yet
                </p>
                <p class="text-sm mb-6">
                    Start creating new notes by clicking the + Add button above.
                </p>
            </div>

            <!-- Message for no results (only when there are notes, but none match the filter) -->
            <div v-else-if="filteredNotes.length === 0"
                class="col-span-1 sm:col-span-2 lg:col-span-3 flex flex-col items-center justify-center text-center text-gray-500 min-h-[200px]">
                No notes found matching your criteria.
            </div>

            <!-- Display notes -->
            <div v-for="(note, index) in filteredNotes" :key="note.id" :style="{ backgroundColor: getNoteColor(index) }"
                class="p-6 rounded-md shadow-lg relative border border-blue-300 overflow-hidden transition-transform duration-300 hover:scale-[1.02] hover:shadow-2xl">
                <!-- Note Title -->
                <h3 class="text-2xl font-bold text-gray-700 mb-2">
                    {{ note.title }}
                </h3>

                <!-- Note Content -->
                <p class="text-gray-600 text-base mt-1 line-clamp-2">
                    {{ note.content }}
                </p>

                <!-- Action Icons -->
                <div class="absolute top-1 right-4 flex space-x-3 text-white">
                    <button @click="viewNote(note)" class="hover:text-emerald-400 transition-all">
                        <i class="mdi mdi-eye text-xl"></i>
                    </button>
                    <button @click="editNote(note)" class="hover:text-yellow-400 transition-all">
                        <i class="mdi mdi-pencil-outline text-xl"></i>
                    </button>
                    <button @click="confirmDelete(note.id)" class="hover:text-red-400 transition-all">
                        <i class="mdi mdi-delete text-xl"></i>
                    </button>
                </div>

                <!-- Date -->
                <div class="mt-4 text-gray-400 text-sm italic">
                    <span>{{ note.date }}</span>
                </div>
            </div>
        </div>

        <!-- Add/Edit Note Modal -->
        <div v-if="showAddModal || showEditModal"
            class="fixed inset-0 bg-black bg-opacity-30 flex justify-center items-center z-50">
            <div
                class="bg-white p-6 rounded-md shadow-xl w-[700px] max-h-[100vh] relative transform transition-all scale-95 hover:scale-100">
                <!-- Close Button -->
                <button @click="closeModals" class="absolute top-3 right-3 text-gray-400 hover:text-gray-600 text-2xl">
                    &times;
                </button>

                <!-- Modal Title -->
                <h2 class="text-2xl font-bold text-gray-800 mb-4 text-center">
                    {{ showEditModal ? "Edit Note" : "Add New Note" }}
                </h2>

                <!-- Title Input -->
                <input v-model="currentNote.title" type="text" placeholder="Enter Note Title"
                    class="w-full p-3 border border-gray-300 rounded-lg text-gray-700 bg-gray-100 focus:outline-none focus:ring-2 focus:ring-emerald-400 mb-3" />

                <!-- Content Textarea -->
                <textarea v-model="currentNote.content" placeholder="Write your note here..."
                    class="w-full p-3 border border-gray-300 rounded-lg text-gray-700 bg-gray-100 focus:outline-none focus:ring-2 focus:ring-emerald-400 resize-none h-32"></textarea>

                <!-- Action Buttons -->
                <div class="flex justify-end space-x-3 mt-4">
                    <button @click="closeModals"
                        class="px-5 py-2 rounded-lg text-gray-600 border border-gray-300 hover:bg-gray-200 transition">
                        Cancel
                    </button>
                    <button @click="saveNote"
                        class="px-5 py-2 bg-emerald-500 text-white rounded-lg hover:bg-emerald-600 transition">
                        {{ showEditModal ? "Update" : "Save" }}
                    </button>
                </div>
            </div>
        </div>

        <!-- View Note Modal -->
        <div v-if="showViewModal"
            class="fixed inset-0 bg-black bg-opacity-30 flex justify-center items-center z-50 p-4">
            <div class="bg-white p-6 rounded-lg shadow-xl w-[700px] max-h-[100vh] relative flex flex-col">
                <!-- Modal Header -->
                <h2 class="text-2xl font-bold text-gray-800 mb-4 text-center">
                    📝 Note Details
                </h2>

                <!-- Note Title -->
                <div class="w-full bg-gray-100 p-3 rounded-md text-lg font-semibold text-gray-800 cursor-default">
                    {{ currentNote.title }}
                </div>

                <!-- Large Scrollable Content Box -->
                <div class="mt-4 p-4 bg-gray-100 rounded-md text-gray-700 leading-relaxed max-h-[300px] overflow-y-auto text-lg"
                    style="min-height: 150px; white-space: pre-wrap;">
                    {{ currentNote.content }}
                </div>

                <!-- Footer Actions -->
                <div class="mt-6 flex justify-end space-x-4">
                    <button @click="showViewModal = false" class="text-gray-500 hover:text-gray-700">CANCEL</button>
                </div>
            </div>
        </div>

        <!-- Delete Confirmation -->
        <div v-if="showDeleteModal" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center">
            <div class="bg-white p-6 rounded-lg shadow-lg w-96 text-center">
                <h3 class="text-lg font-bold text-gray-800 mb-4">Are you sure you want to delete this note?</h3>
                <div class="flex justify-center gap-4">
                    <button @click="deleteNote" class="px-4 py-2 bg-red-500 text-white rounded-md hover:bg-red-600">
                        Yes, Delete
                    </button>
                    <button @click="showDeleteModal = false"
                        class="px-4 py-2 bg-gray-300 text-gray-700 rounded-md hover:bg-gray-400">
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useNotesStore } from '@/stores/notesStore'; // Pinia store

const notesStore = useNotesStore();

const searchQuery = ref('');
const searchDate = ref('');
const sortOrder = ref('desc');
const showAddModal = ref(false);
const showEditModal = ref(false);
const showViewModal = ref(false);
const showDeleteModal = ref(false);
const currentNote = ref({ title: '', content: '', id: null });
const filteredNotes = computed(() => {
  let filtered = notesStore.notes;

  if (searchQuery.value) {
    filtered = filtered.filter(note =>
      note.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      note.content.toLowerCase().includes(searchQuery.value.toLowerCase())
    );
  }

  if (searchDate.value) {
    filtered = filtered.filter(note => note.date === searchDate.value);
  }

  if (sortOrder.value === 'asc') {
    filtered = filtered.sort((a, b) => new Date(a.date) - new Date(b.date));
  } else {
    filtered = filtered.sort((a, b) => new Date(b.date) - new Date(a.date));
  }

  return filtered;
});

const getNoteColor = (index) => {
  return index % 2 === 0 ? '#F3F4F6' : '#FFFFFF';
};

const viewNote = (note) => {
  currentNote.value = note;
  showViewModal.value = true;
};

const editNote = (note) => {
  currentNote.value = { ...note };
  showEditModal.value = true;
};

const closeModals = () => {
  showAddModal.value = false;
  showEditModal.value = false;
  showViewModal.value = false;
};

const saveNote = async () => {
  if (showEditModal.value) {
    await notesStore.updateNote(currentNote.value);
  } else {
    await notesStore.addNote(currentNote.value);
  }
  closeModals();
};

const confirmDelete = (noteId) => {
  currentNote.value.id = noteId;
  showDeleteModal.value = true;
};

const deleteNote = async () => {
  await notesStore.deleteNote(currentNote.value.id);
  showDeleteModal.value = false;
};

onMounted(() => {
  notesStore.fetchNotes(); // Fetch notes from API when component is mounted
});
</script>