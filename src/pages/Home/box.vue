<template>
    <div class="min-h-screen bg-gray-100 flex flex-col items-center p-6">
        <!-- Top Bar -->
        <div class="w-full max-w-4xl flex flex-wrap gap-4 justify-between items-center">
            <!-- Search Input -->
            <input v-model="searchQuery" type="text" placeholder="Search by title or content"
                class="w-full sm:w-auto flex-1 p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-emerald-400" />

            <!-- Date Filter -->
            <input v-model="searchDate" type="date"
                class="p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-emerald-400" />

            <!-- Sort by Newest/Oldest -->
            <select v-model="sortOrder"
                class="p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-emerald-400">
                <option value="desc">Newest First</option>
                <option value="asc">Oldest First</option>
            </select>

            <!-- Add Note Button -->
            <button @click="showAddModal = true"
                class="px-4 py-2 bg-emerald-500 text-white rounded-md shadow hover:bg-emerald-600 transition">
                + Add Note
            </button>
        </div>

        <!-- Notes Grid -->
        <div class="w-full max-w-4xl mt-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
            <!-- Display message if user doesn't have notes yet -->
            <div v-if="notes.length === 0"
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
                class="p-6 rounded-md shadow-lg relative border border-emerald-300 overflow-hidden transition-transform duration-300 hover:scale-[1.02] hover:shadow-2xl">
                <!-- Note Title -->
                <h3 class="text-2xl font-bold text-white mb-2">
                    {{ note.title }}
                </h3>

                <!-- Note Content -->
                <p class="text-gray-100 text-base mt-1 line-clamp-2">
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

                <!-- date -->
                <div class="mt-4 text-gray-100 text-sm italic">
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
                <h2 class="text-lg font-bold">Are you sure?</h2>
                <p class="text-gray-600">This action cannot be undone.</p>
                <div class="mt-4 flex justify-center space-x-2">
                    <button @click="showDeleteModal = false"
                        class="px-4 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600">
                        Cancel
                    </button>
                    <button @click="deleteNote" class="px-4 py-2 bg-red-500 text-white rounded-md hover:bg-red-600">
                        Delete
                    </button>
                </div>
            </div>
        </div>

        <!-- Toast Notification -->
        <div v-if="toast.message" :class="toast.type === 'success' ? 'bg-green-500' : 'bg-red-500'"
            class="fixed top-5 right-5 text-white px-4 py-2 rounded shadow-lg transition">
            {{ toast.message }}
        </div>
    </div>
</template>

<script>
import { computed } from "vue";

export default {
    data() {
        return {
            showAddModal: false,
            showEditModal: false,
            showViewModal: false,
            showDeleteModal: false,
            sortOrder: "desc",
            searchQuery: "",
            searchDate: "",
            colorList: [], // Store unique colors
            notes: [
                {
                    id: 1,
                    title: "Finish the task",
                    content: "Complete all remaining tasks",
                    date: "22.01.2022",
                },
                {
                    id: 2,
                    title: "Buy groceries",
                    content: "Milk, bread, and eggs",
                    date: "21.01.2023",
                },
                {
                    id: 3,
                    title: "Finish the task",
                    content: "Complete all remaining tasks",
                    date: "22.01.2024",
                },
                {
                    id: 4,
                    title: "Buy groceries",
                    content: "Milk, bread, and eggs",
                    date: "21.01.2025",
                },
            ],
            currentNote: { id: null, title: "", content: "" },
            noteToDelete: null,
            toast: { message: "", type: "" },
        };
    },
    computed: {
        // Generates unique colors for notes
        generateColors() {
            return this.shuffleColors(this.generateRandomColors(this.notes.length));
        },

        // Filters and sorts notes
        filteredNotes() {
            return this.notes
                .filter((note) => {
                    // Filter by search query
                    const matchesSearch = this.searchQuery
                        ? note.title
                            .toLowerCase()
                            .includes(this.searchQuery.toLowerCase()) ||
                        note.content
                            .toLowerCase()
                            .includes(this.searchQuery.toLowerCase())
                        : true;

                    // Filter by date
                    const matchesDate = this.searchDate
                        ? note.date === this.searchDate
                        : true;

                    return matchesSearch && matchesDate;
                })
                .slice() // Prevent modifying original array before sorting
                .sort((a, b) => {
                    // Sort by newest or oldest
                    return this.sortOrder === "asc"
                        ? new Date(a.date) - new Date(b.date) // Oldest first
                        : new Date(b.date) - new Date(a.date); // Newest first
                });
        },
    },

    methods: {
        viewNote(note) {
            this.currentNote = { ...note };
            this.showViewModal = true;
        },
        editNote(note) {
            this.currentNote = { ...note };
            this.showEditModal = true;
        },
        confirmDelete(id) {
            this.noteToDelete = id;
            this.showDeleteModal = true;
        },
        deleteNote() {
            this.notes = this.notes.filter((note) => note.id !== this.noteToDelete);
            this.showDeleteModal = false;
            this.showToast("Note deleted!", "success");
        },
        saveNote() {
            if (!this.currentNote.title || !this.currentNote.content) {
                this.showToast("Title and content cannot be empty!", "error");
                return;
            }

            if (this.showEditModal) {
                const index = this.notes.findIndex(
                    (note) => note.id === this.currentNote.id
                );
                if (index !== -1) this.notes[index] = { ...this.currentNote };
                this.showToast("Note updated!", "success");
            } else {
                this.notes.push({
                    id: Date.now(),
                    ...this.currentNote,
                    date: new Date().toLocaleDateString(),
                });
                this.showToast("Note added!", "success");
            }

            this.closeModals();
        },
        closeModals() {
            this.showAddModal = this.showEditModal = this.showViewModal = false;
            this.currentNote = { id: null, title: "", content: "" };
        },
        showToast(message, type) {
            this.toast = { message, type };
            setTimeout(() => {
                this.toast.message = "";
            }, 3000);
        },
        // Generate N random colors
        generateRandomColors(count) {
            const colors = [];
            for (let i = 0; i < count; i++) {
                const randomColor = `hsl(${Math.floor(Math.random() * 360)}, 70%, 80%)`;
                colors.push(randomColor);
            }
            return colors;
        },

        // Shuffle colors to avoid patterns
        shuffleColors(array) {
            return array.sort(() => Math.random() - 0.5);
        },

        // Assign a unique color to each note
        getNoteColor(index) {
            return this.generateColors[index] || "#f3f3f3"; // Default color if missing
        },
    },
};
</script>
