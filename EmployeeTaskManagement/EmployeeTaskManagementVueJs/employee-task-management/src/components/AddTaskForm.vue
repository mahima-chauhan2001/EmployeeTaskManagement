<template>
  <div class="form-container">
    <h2>Add Task</h2>

    <button @click="closeForm" class="button close-form">Close Form</button>

    <form @submit.prevent="handleSubmit" v-if="!formSubmitted">
      <div>
        <label for="taskTitle">Task Title</label>
        <input
          type="text"
          id="taskTitle"
          v-model="form.taskTitle"
          required
          @blur="validateField('taskTitle')"
          :class="{ 'input-error': errors.taskTitle }"
        />
        <span v-if="errors.taskTitle" class="error">{{ errors.taskTitle }}</span>
      </div>

      <div>
        <label for="description">Description</label>
        <input
          type="text"
          id="description"
          v-model="form.description"
          required  
          @blur="validateField('description')"
          :class="{ 'input-error': errors.description }"
        />
        <span v-if="errors.description" class="error">{{ errors.description }}</span>
      </div>
   
      <button type="submit" :disabled="isSubmitting">Submit</button>
    </form>

    <div v-else>
      <p>Tasks added successfully!</p>
      <button @click="resetForm">Add Another</button>
    </div>
    <div v-if="isSubmitting" class="loading-spinner">
      <span>Submitting...</span>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      form: {
        taskTitle: "",
        description: "", 
      },
      errors: {},
      isSubmitting: false,
      formSubmitted: false,
      errorMessage: "",
    };
  },
  methods: {
    validateField(field) { 
      if (!this.form[field]) {
        this.errors[field] = `${field} is required.`;
      } else {
        this.errors[field] = "";
      } 
    },

    async handleSubmit() { 
      console.log("Form submission started");
      this.validateField("taskTitle");
      this.validateField("description") 
      
      if (Object.values(this.errors).some((error) => error)) {
        console.log("Form has errors:", this.errors);
        return;
      }
 
      this.isSubmitting = true;
      this.errorMessage = "";

      try {
       const assignedFromId = localStorage.getItem("assignedFromId");
        const token = this.getAuthToken();
        console.log("Token:", token);
        if (!token) {
          this.errorMessage = "Authorization token is missing or invalid.";
          this.isSubmitting = false;
          return;
        }
        const payload = {
          title: this.form.taskTitle,
          description: this.form.description, 
          assignedFromId: assignedFromId
        };
        const response = await axios.post(
          "https://localhost:7124/admin/createTasks",
          payload,
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${token}`,
            },
          }
        );

        console.log("Task response:", response.data);  
        this.formSubmitted = true;
      } catch (error) {
        console.error(error);
        this.errorMessage = "Something went wrong, please try again later.";
      } finally {
        this.isSubmitting = false;
      }
    },

    getAuthToken() {
      return localStorage.getItem("token");
    },

    resetForm() { 
      this.form = {
        taskTitle: "",
        description: "", 
      };
      this.errors = {};
      this.formSubmitted = false;
    },

    closeForm() {
       this.$emit("close-form");
    },
  },
};
</script>

<style scoped>
/* Styling for the form */
.form-container {
  max-width: 500px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
  background-color: #fff; /* White background for better visibility */
}

input,
select {
  width: 100%;
  padding: 12px; /* Increased padding for visibility */
  margin: 8px 0;  /* Spacing between input fields */
  box-sizing: border-box;
  border: 1px solid #ccc; /* Add a border for the input fields */
  border-radius: 4px; /* Rounded corners */
  font-size: 14px; /* Set font size for better readability */
}

input:focus,
select:focus {
  outline: none;
  border-color: #4caf50; /* Green border on focus */
  box-shadow: 0 0 5px rgba(76, 175, 80, 0.3); /* Focus shadow */
}

button {
  background-color: #4caf50;
  color: white;
  padding: 8px 14px; /* Adjust padding for a smaller button */
  border: none;
  border-radius: 5px;
  cursor: pointer;
  margin-top: 10px;
  font-size: 14px; /* Smaller font size for the button */
}

button:disabled {
  background-color: #ccc;
}

button:hover {
  background-color: #45a049;
}

/* Styling for error messages */
.error {
  color: red;
  font-size: 12px;
  margin-top: 4px;  /* Space above error messages */
}

.input-error {
  border-color: red; /* Red border for inputs with errors */
}

/* Loading spinner */
.loading-spinner {
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 16px;
  color: #4caf50; /* Green spinner text */
}

/* Styling for close form button */
button.close-form {
  background-color: #f44336;
  padding: 8px 14px; /* Smaller padding for close button */
  font-size: 14px; /* Smaller font size for close button */
}

button.close-form:hover {
  background-color: #e53935;
}

h2 {
  text-align: center;
  font-size: 24px;
  margin-bottom: 20px; /* Space below header */
}

form {
  display: flex;
  flex-direction: column;
}

form > div {
  margin-bottom: 16px;
}

form > div label {
  font-weight: bold;
  margin-bottom: 5px;
}
</style>
