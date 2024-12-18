<template>
  <div class="get-employee-container">
    <h2>Tasks List</h2>

    <button @click="closeForm" class="button close-form">Close</button>

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
            <td> 
              <v-select
                v-model="item.assignedToId"
                :items="users"
                item-value="userId"
                item-text="fullName"
                label="Assign User"
             @change="markAsChanged(item)"  
              ></v-select>
            </td>
            <td>{{ item.createdDate }}</td>
            <td>{{ item.dueDate }}</td>
            <td>{{ item.status }}</td>
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
            <td>
              <!-- <v-btn @click="editUser(item)" color="primary">Edit</v-btn> -->
             <v-btn @click="deleteUser(item)" color="error">Delete</v-btn>
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
import axios from "axios";

export default {
  data() {
    return {
      employees: [],
      users: [],
      headers: [
        { text: "Task Title", align: "start", key: "title" },
        { text: "Task Description", align: "start", key: "description" },
        { text: "Assigned From", align: "start", key: "assignedFromId" },
        { text: "Assigned To", align: "start", key: "assignedToId" },
        { text: "Created Date", align: "start", key: "createdDate" },
        { text: "Due Date", align: "start", key: "dueDate" },
        { text: "Task Status", align: "start", key: "status" },
        { text: "Actions", key: "actions", align: "end" },
      ],
      isLoading: false,
       changedTasks: []
    };
  },
  methods: {
      async deleteUser(item) {
  console.log("item", item);
  try {
    console.log("API called to delete task");

    const token = this.getAuthToken();
    if (!token) {
      console.error("Authorization token is missing");
      return;
    } 
    const response = await axios.delete(
      `https://localhost:7124/DeleteTask?id=${item.id}`,  
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    ); 
    console.log("Task deleted successfully:", response.data);
    alert("Task deleted successfully!");
 
    this.employees = this.employees.filter((employee) => employee.id !== item.id);
  } catch (error) {
    console.error("Error in deleting task", error);
    alert("Failed to delete task.");
  } 
},

    async fetchTasks() {
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

        this.employees = response.data;
      } catch (error) {
        console.error("Error fetching employee data", error);
      } finally {
        this.isLoading = false;
      }
    },

    async fetchUsers() {
      try {
        const response = await axios.get(
          "https://localhost:7124/api/Employee/GetUsers"
        ); 
        this.users = response.data.map((user) => ({
          userId: user.userId,  
          firstName: user.firstName,  
          lastName: user.lastName,  
          fullName: `${user.firstName} ${user.lastName}`,  
        }));
        console.log("response.data", response.data);
        console.log("Fetched users:", this.users);  
      } catch (error) {
        console.error("Error fetching users", error);
      }
    },
    getDefaultUser(assignedToId) {
      return this.users.find((user) => user.userId === assignedToId);
    },
    showDropdown(item) {
      item.showDropdown = !item.showDropdown;
    },
    updateAssignedTo(item) {
      console.log(
        `Task ID ${item.id} assigned to user ID ${item.assignedToId}`
      );
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
     markAsChanged(item) {
      if (!this.changedTasks.includes(item)) {
        this.changedTasks.push(item); // Add changed task to the list
      }
    },
    async saveChanges(item) {
      try {
        const token = this.getAuthToken();
        if (!token) {
          console.error("Authorization token is missing");
          return;
        }

        // Prepare the update data
        const updateData = {
          AssignedToId: item.assignedToId, // New assigned user
          AssignedFromId: item.assignedFromId, // Original user
          Status: item.status, // Task status
        };

        // Send the PUT request with the task's ID
        const response = await axios.put(
          `https://localhost:7124/updateTask?id=${item.id}`, // Use item.id in the URL
          updateData,
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );

        // Handle the successful response
        console.log("Task updated successfully:", response.data);
        alert("Changes saved successfully!");
      } catch (error) {
        console.error("Error saving changes", error);
        alert("Failed to save changes.");
      }
    },
 
  },




  mounted() {
    this.fetchTasks();
    this.fetchUsers();
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
  background-color: #2196f3; /* Blue background for Edit button */
  color: white; /* White text */
}
.save-btn {
  margin-top: 20px;
  color: #2196f3;
  background-color: aqua;
}
</style>
