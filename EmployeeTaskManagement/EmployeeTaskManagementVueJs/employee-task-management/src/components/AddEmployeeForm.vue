<template>
  <div class="form-container">
    <h2>Add Employee</h2>

    <button @click="closeForm" class="button close-form">Close Form</button>

    <form @submit.prevent="handleSubmit" v-if="!formSubmitted">
      <div>
        <label for="firstName">First Name</label>
        <input
          type="text"
          id="firstName"
          v-model="form.firstName"
          required
          @blur="validateField('firstName')"
          :class="{ 'input-error': errors.firstName }"
        />
        <span v-if="errors.firstName" class="error">{{
          errors.firstName
        }}</span>
      </div>

      <div>
        <label for="lastName">Last Name</label>
        <input
          type="text"
          id="lastName"
          v-model="form.lastName"
          required
          @blur="validateField('lastName')"
          :class="{ 'input-error': errors.lastName }"
        />
        <span v-if="errors.lastName" class="error">{{ errors.lastName }}</span>
      </div>

      <div>
        <label for="email">Email</label>
        <input
          type="email"
          id="email"
          v-model="form.email"
          required
          @blur="validateField('email')"
          :class="{ 'input-error': errors.email }"
        />
        <span v-if="errors.email" class="error">{{ errors.email }}</span>
      </div>

      <div>
        <label for="password">Password</label>
        <input
          type="password"
          id="password"
          v-model="form.password"
          required
          @blur="validateField('password')"
          :class="{ 'input-error': errors.password }"
        />
        <span v-if="errors.password" class="error">{{ errors.password }}</span>
      </div>

      <div>
        <label for="role">Role</label>
        <select
          v-model="form.role"
          id="role"
          required
          @blur="validateField('role')"
          :class="{ 'input-error': errors.role }"
        >
          <option value="" disabled>Select role</option>
          <option value="Admin">Admin</option>
          <option value="Employee">Employee</option>
        </select>
        <span v-if="errors.role" class="error">{{ errors.role }}</span>
      </div>

      <button type="submit" :disabled="isSubmitting">Submit</button>
    </form>

    <div v-else>
      <p>Employee added successfully!</p>
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
        firstName: "",
        lastName: "",
        email: "",
        role: "",
        password: "",
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
      this.validateField("firstName");
      this.validateField("lastName");
      this.validateField("email");
      this.validateField("password");
      this.validateField("role");
 
      if (Object.values(this.errors).some((error) => error)) {
        console.log("Form has errors:", this.errors);
        return;
      }
 
      this.isSubmitting = true;
      this.errorMessage = "";

      try {
        const token = this.getAuthToken();
        console.log("Token:", token);
        if (!token) {
          this.errorMessage = "Authorization token is missing or invalid.";
          this.isSubmitting = false;
          return;
        }
        const payload = {
          firstName: this.form.firstName,
          lastName: this.form.lastName,
          email: this.form.email,
          password: this.form.password, 
          role: this.form.role,
        };
        const response = await axios.post(
          "https://localhost:7124/api/Employee/CreateEmployee",
          payload,
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${token}`,
            },
          }
        );

        console.log("rrrrrrrrrr", response.data);  
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
      // Reset the form after successful submission
      this.form = {
        firstName: "",
        lastName: "",
        email: "",
        role: "",
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
.form-container {
  max-width: 500px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
  background-color: #f9f9f9;  
}

 
input,
select {
  width: 100%;
  padding: 12px;
  margin: 8px 0;   
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
}

input:focus,
select:focus {
  outline: none;
  border-color: #4caf50;  
  box-shadow: 0 0 5px rgba(76, 175, 80, 0.3); /* Added focus effect */
}
 
button {
  background-color: #4caf50;
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  margin-top: 10px;
  width: 100%;  
  font-size: 16px;
}

button:disabled {
  background-color: #ccc;
}

button:hover {
  background-color: #45a049;
}

 
.error {
  color: red;
  font-size: 12px;
  margin-top: 4px;  
}

.input-error {
  border-color: red;  
}

 
.loading-spinner {
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 16px;
  color: #4caf50;  
}

 
button.close-form {
  background-color: #f44336; 
  font-size: 14px;  
}

button.close-form:hover {
  background-color: #e53935;
}

h2 {
  text-align: center;
  font-size: 24px;
  margin-bottom: 20px; 
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

