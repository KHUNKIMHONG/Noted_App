<template>
  <div>
      <Navbar />
      <Notes class="pt-32" />
  </div>
</template>

<script>
import Navbar from "@/components/Navbar/Navbar.vue";
import Notes from "../../components/Noted/Notes.vue";

export default {
  components: {
    Navbar,
    Notes
  }
};
</script>



