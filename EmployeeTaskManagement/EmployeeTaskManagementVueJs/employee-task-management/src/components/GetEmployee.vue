<template>
  <div class="get-employee-container">
    <h2>Employee List</h2>

    <button @click="closeForm" class="button close-form">Close</button>
 
    <v-data-table
      v-if="!isLoading && employees.length > 0"
      :headers="headers"
      :items="employees"
      item-key="userId"
    > 
      <template v-slot:body="{ items }">
        <tbody>
          <tr v-for="item in items" :key="item.userId">
            <td>{{ item.firstName }}</td>
            <td>{{ item.lastName }}</td>
            <td>{{ item.email }}</td>
            <td>{{ item.role }}</td>
            <td>
              <!-- <v-btn @click="editUser(item)" color="primary">Edit</v-btn> -->
              <v-btn @click="deleteUser(item.userId)" color="error">Delete</v-btn>
            </td>
          </tr>
        </tbody>
      </template>
    </v-data-table>
 
    <div v-if="isLoading" class="loading-spinner">
      <span>Loading...</span>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      employees: [],
      headers: [
        { text: 'First Name', align: 'start', key: 'firstName' },
        { text: 'Last Name', align: 'start', key: 'lastName' },
        { text: 'Email', align: 'start', key: 'email' },
        { text: 'Role', align: 'start', key: 'role' },
        { text: 'Actions', key: 'actions', align: 'end' },  
      ],
      isLoading: false,
    };
  },
  methods: {
    async fetchEmployees() {
      console.log("API called");
      this.isLoading = true;
      try {
        const token = this.getAuthToken();
        if (!token) {
          console.error('Authorization token is missing');
          this.isLoading = false;
          return;
        }
        const response = await axios.get('https://localhost:7124/api/Employee/GetUsers', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        console.log("Employees:", response.data);
        this.employees = response.data;
      } catch (error) {
        console.error('Error fetching employee data', error);
      } finally {
        this.isLoading = false;
      }
    },

    getAuthToken() {
      return localStorage.getItem('token');
    },

    closeForm() {
      this.$emit('close-form');
    },

    editUser(item) {
      console.log('Edit user:', item);
    },

    async deleteUser(userId) {
      console.log("deleteUseruserId", typeof(userId))
      try{
       const token = this.getAuthToken();
       if (!token) {
      console.error("Authorization token is missing");
      return;
    } 
     const response = await axios.delete(
      `https://localhost:7124/api/Employee/DeleteEmployee?Userid=${userId}`,  
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    ); 
    console.log("Task deleted successfully:", response.data);
    alert("Task deleted successfully!");
     this.employees = this.employees.filter((employee) => employee.userId !== userId);
      }
      catch(error){
 console.error("Error in deleting task", error);
    alert("Failed to delete task.");
      } 
    },
  },
  mounted() {
    this.fetchEmployees();
  },
};
</script>

<style scoped>
.get-employee-container {
  margin: 20px;
}

button.close-form {
  background-color: #f44336;
}

button.close-form:hover {
  background-color: #e53935;
}

.loading-spinner {
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 16px;
}

.edit-btn {
  background-color: #2196f3;  
  color: white;  
}
</style>
