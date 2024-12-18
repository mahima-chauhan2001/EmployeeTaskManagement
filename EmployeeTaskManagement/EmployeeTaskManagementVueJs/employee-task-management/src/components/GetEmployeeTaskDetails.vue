<template>
  <div class="get-employee-container">
    <h2>Employee List</h2>

    <button @click="closeForm" class="button close-form">Close</button>
    <div>
    <h2 v-if="isTaskAvailable">No tasks are available for this employee</h2>
    </div>
    <v-data-table
      v-if="!isLoading && employees.length > 0"
      :headers="headers"
      :items="employees"
      item-key="id"
    >
      <template v-slot:body="{ items }">
        <tbody>
          <tr v-for="item in items" :key="item.id">
            <td>{{ item.title }}</td>
            <td>{{ item.description }}</td>
            <td>{{ item.assignedFromId }}</td>
            <td>{{ item.createdDate }}</td>
            <td>{{ item.dueDate }}</td>
            <td>
              {{ item.status || "N/A" }} 
              <v-btn @click="toggleStatus(item)" color="primary" icon small>
                <v-icon>mdi-pencil</v-icon>
              </v-btn>
            </td>
            <td>
              <v-btn
                v-if="employees.length > 0"
                @click="saveChanges(item)"
                color="red"
                class="save-btn"
              >
                Save Changes
              </v-btn>
            </td>

            <!-- <td>
              <v-btn @click="editUser(item)" color="primary">Edit</v-btn>
              <v-btn @click="deleteUser(item.userId)" color="error">Delete</v-btn>
            </td> -->
          </tr>
        </tbody>
      </template>
    </v-data-table>

    <!-- Loading state spinner -->
    <div v-if="isLoading" class="loading-spinner">
      <span>Loading...</span>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      employees: [],
      headers: [
        { text: "Task Title", align: "start", key: "title" },
        { text: "Task Description", align: "start", key: "description" },
        { text: "Assigned From", align: "start", key: "assignedFromId" },
        { text: "Created Date", align: "start", key: "createdDate" },
        { text: "Due Date", align: "start", key: "dueDate" },
        { text: "Task Status", align: "start", key: "status" },
        { text: "Actions", key: "actions", align: "end" },
      ],  
      isLoading: false,
      filteredTasks: null,
      isTaskAvailable: false
    };
  },
  methods: {
    async fetchEmployees() {
      console.log("API called");
      this.isLoading = true;
      try {
        const token = this.getAuthToken();
        var userId = localStorage.getItem("assignedFromId");
        console.log("userId", userId);
        if (!token) {
          console.error("Authorization token is missing");
          this.isLoading = false;
          return;
        }
        const response = await axios.get("https://localhost:7124/GetTasks", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        console.log("response", response.data);
        this.filteredTasks = response.data.filter(
          (task) => task.assignedToId == userId
        );
        if (this.filteredTasks.length == 0) {
          this.isTaskAvailable=true;
        }
        console.log("filteredTasks:", this.filteredTasks);
        this.employees = this.filteredTasks;
      } catch (error) {
        console.error("Error fetching employee data", error);
      } finally {
        this.isLoading = false;
      }
    },

    getAuthToken() {
      return localStorage.getItem("token");
    },

    closeForm() {
      this.$emit("close-form");
    },

    async toggleStatus(task) {
      const newStatus = task.status === "Pending" ? "Completed" : "Pending";
      task.status = newStatus;
    },

    async saveChanges(item) {
      try {
        const token = this.getAuthToken();
        if (!token) {
          console.error("Authorization token is missing");
          return;
        }

        const updateData = {
          AssignedToId: item.assignedToId, // New assigned user
          AssignedFromId: item.assignedFromId, // Original user
          Status: item.status, // Task status
        };
        console.log("Status", updateData.Status);
        const response = await axios.put(
          `https://localhost:7124/updateTask?id=${item.id}`,  
          updateData,
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );

        console.log("Task updated successfully:", response.data);
        alert("Changes saved successfully!");
      } catch (error) {
        console.error("Error saving changes", error);
        alert("Failed to save changes.");
      }
    },
  },

  // editUser(item) {
  //   console.log('Edit user:', item);
  // },

  // deleteUser(userId) {
  //   console.log('Delete user with ID:', userId);
  // },

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
.save-btn {
  margin-top: 15px;
  color: #2196f3;
  background-color: aqua;
}
</style>
