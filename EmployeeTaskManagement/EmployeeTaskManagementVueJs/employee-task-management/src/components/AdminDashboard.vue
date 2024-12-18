<template>
  <div>
    <h1>Admin Dashboard</h1>
  <div class="button-container"> 
      <button @click="showAddEmployeeForm" class="button">Add Employee</button>     
      <button @click="showGetEmployee" class="button">Get employee</button>
      <button @click="showAddTaskForm" class="button">Add Task</button>  
      <button @click="showGetTaskForm" class="button">Get Tasks</button>    
    </div>    
    <button @click="logout" class="button logout">Logout</button> 
    <transition name="fade">
      <AddEmployeeForm v-if="isAddEmployeeFormVisible" @close-form="hideAddEmployeeForm" />
    </transition>
    <transition name="fade">
      <GetEmployee v-if="isGetEmployeeFormVisible" @close-form="hideGetEmploye" />    
    </transition>
    <transition name="fade">
      <AddTaskForm v-if="isAddTaskFormVisible" @close-form="hideAddTaskForm" />
    </transition>   
     <transition name="fade">
      <GetTask v-if="isGetTaskVisible" @close-form="hideGetTask" />
    </transition>
  </div>
</template>

<script> 
import AddEmployeeForm from './AddEmployeeForm.vue';
import AddTaskForm from './AddTaskForm.vue';
import GetEmployee from './GetEmployee.vue';  
import GetTask from './GetTask.vue';
export default {
  components: {
    AddEmployeeForm,
    AddTaskForm,
    GetEmployee,
 GetTask
  },
  data() {
    return {
      isAddEmployeeFormVisible: false,  
      isGetEmployeeFormVisible : false,
      isAddTaskFormVisible: false,   
      isGetTaskVisible : false,
      isLogOut : false,  
    };
  },
  methods: {
    showAddEmployeeForm() {
      this.isAddEmployeeFormVisible = true;
     
    },
 
    hideAddEmployeeForm() {
      this.isAddEmployeeFormVisible = false;
    },
 
    showAddTaskForm() {
      this.isAddTaskFormVisible = true;
    },
    
    hideAddTaskForm() {
      this.isAddTaskFormVisible = false;
    },

    showGetEmployee(){
       this.isGetEmployeeFormVisible = true   
    },

    hideGetEmploye(){
       this.isGetEmployeeFormVisible = false  
    },

    showGetTaskForm(){
this.isGetTaskVisible = true
    },

    hideGetTask(){
      this.isGetTaskVisible = false
    },

    confirmLogout() {
      this.confirmLogoutDialog = true;
    },

    logout() {
      sessionStorage.setItem("isLogout", true)
      this.isLogOut = true,
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      this.$router.push("/");
    },

    cancelLogout() {
      this.confirmLogoutDialog = false;
    },
  },
};
</script>

<style scoped>
.button {
  padding: 10px 20px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  margin: 10px;
}

.button:hover {
  background-color: #45a049;
}

.logout {
  background-color: #f44336;
}

.logout:hover {
  background-color: #e53935;
}
 
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.5s;
}
.fade-enter, .fade-leave-to {
  opacity: 0;
} 
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