<template>
  <div class="login-container">
    <h2>Login</h2>
    <form @submit.prevent="login">
      <div class="input-group">
        <label for="email">Email:</label>
        <input
          type="text"
          id="email"
          v-model="email"
          required
          placeholder="Enter email"
        />
      </div>
      <div class="input-group">
        <label for="password">Password:</label>
        <input
          type="password"
          id="password"
          v-model="password"
          required
          placeholder="Enter password"
        />
      </div>
      <button type="submit" :disabled="isLoading" class="submit-btn">Login</button>
      <div v-if="isLoading" class="loading">Loading...</div>
      <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
    </form>
 
    <transition name="fade">
      <div v-if="showLogoutDialog" class="confirmation-dialog">
        <div class="dialog-box">
          <p>Are you sure you want to log out?</p>
          <button @click="logout" class="confirm">Yes</button>
          <button @click="cancelLogout" class="cancel">Cancel</button>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      email: "",
      password: "",
      errorMessage: "",
      isLoading: false,
      showLogoutDialog: false,
    };
  },
  methods: {
    async login() {
      sessionStorage.setItem("isLogout", false)
      console.error("errorrr")
      this.isLoading = true;
      this.errorMessage = "";
      const credentials = { email: this.email, password: this.password };

      try {
        console.log("aaaaaaaaaaa",credentials);
        
        if (credentials.email == '' && credentials.password == ''){
          console.log("credentials", credentials);
          return ;
        }
        const { data } = await axios.post("https://localhost:7124/api/Auth/login", credentials);
        if (data.token) {
          localStorage.setItem("token", data.token);
          localStorage.setItem("role", data.role);
          localStorage.setItem("message", data.message);
          localStorage.setItem("assignedFromId", data.assignedFromId);
          console.log("assignedFromId", data.assignedFromId);
          
          this.$router.push(data.role === "Admin" ? "/admin-dashboard" : "/employee-dashboard");
        } else {
             this.$router.push(data.role === "Employee" ? "/employee-dashboard" : "/admin-dashboard" );
        } 
      } catch (error) {
        console.error(error);
        this.errorMessage = "Something went wrong, please try again later.";
      } finally {
        this.isLoading = false;
      }
    },

    confirmLogout() {
      this.showLogoutDialog = true;
    },

    logout() {
      localStorage.removeItem("token");
      this.$router.push("/");
    },

    cancelLogout() {
      this.showLogoutDialog = false;
    },
  },
  mounted(){
    this.login()
     sessionStorage.getItem("isLogout");
  }
};
</script>

<style scoped> 
.login-container {
  width: 100%;
  max-width: 400px;
  margin: 0 auto;
  padding: 40px;
  background-color: #fff;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  text-align: center;
  font-family: 'Arial', sans-serif;
}

h2 {
  font-size: 24px;
  color: #333;
  margin-bottom: 30px;
}

.input-group {
  margin-bottom: 20px;
  text-align: left;
}

.input-group label {
  display: block;
  color: #333;
  font-size: 14px;
  margin-bottom: 5px;
}

input {
  width: 100%;
  padding: 12px;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 16px;
  outline: none;
}

input:focus {
  border-color: #4CAF50;
}

button {
  width: 100%;
  padding: 12px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

button:hover:not(:disabled) {
  background-color: #45a049;
}

.loading {
  font-size: 14px;
  color: #4CAF50;
  margin-top: 10px;
}

.error-message {
  font-size: 14px;
  color: red;
  margin-top: 10px;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}

/* Confirmation dialog styling */
.confirmation-dialog {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 100;
}

.dialog-box {
  background-color: white;
  padding: 20px;
  border-radius: 8px;
  width: 300px;
  text-align: center;
}

.dialog-box button {
  margin-top: 10px;
  padding: 8px 16px;
  cursor: pointer;
}

.dialog-box button.cancel {
  background-color: #ccc;
}

.dialog-box button.confirm {
  background-color: #f44336;
  color: white;
}
</style>
