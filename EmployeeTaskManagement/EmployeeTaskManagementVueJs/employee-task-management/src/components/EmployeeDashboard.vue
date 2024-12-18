<template>
  <div>
    <h1>Employee Dashboard</h1>
  <div class="button-container"> 
      <button @click="showGetEmployeeTask" class="button">Get employee task details</button> 
    </div>    
    <button @click="logout" class="button logout">Logout</button> 
    <transition name="fade">
      <GetEmployeeTaskDetails v-if="isEmployeeTaskVisible" @close-form="hideEmployeeTaskVisible" />
    </transition> 
  </div>
</template>
<script>
import GetEmployeeTaskDetails from './GetEmployeeTaskDetails.vue'; 
export default {
  components: {
    GetEmployeeTaskDetails, 
  },
  data() {
    return{ 
     isEmployeeTaskVisible : false, 
    }
  },
  
 methods: {
   showGetEmployeeTask(){
    this.isEmployeeTaskVisible = true
   },
   hideEmployeeTaskVisible(){
     this.isEmployeeTaskVisible = false
   }, 
   logout() {
     sessionStorage.setItem("isLogout", true)
      this.isLogOut = true,
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      this.$router.push("/");
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