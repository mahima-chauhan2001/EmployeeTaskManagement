import Vue from "vue";
import Router from "vue-router";
import Login from "../components/Login.vue";
import AdminDashboard from "../components/AdminDashboard.vue";
import EmployeeDashboard from "../components/EmployeeDashboard.vue";

Vue.use(Router);

const routes = [
  { path: '/', component: Login },
  { path: '/admin-dashboard', component: AdminDashboard, meta: { requiresAuth: true, requiresAdmin: true }, },
  { path: '/employee-dashboard', component: EmployeeDashboard, meta: { requiresAuth: true, requiresEmployee: true } },
  { path: "*", redirect: "/login" },
];

const router = new Router({
  routes,
});
  
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("token");
  const role = localStorage.getItem("role");
  const logOut = sessionStorage.getItem("isLogout") 
    console.log("to.meta.requiresAdmin", to.meta.requiresAdmin);
    console.log("to.meta.requiresEmployee", to.meta.requiresEmployee);
    console.log("to.path", to.path);
    console.log("logOut",logOut);

    if (token) {     
      if (role === "Employee" && !logOut) { 
        if (to.path !== '/employee-dashboard' && to.path !== '/') {
          next('/employee-dashboard');
        } 
        else if (to.path == '/'){
          next('/employee-dashboard');
        }
          else {
          next();   
        }
      }  
      else if (role === "Admin" && !logOut) { 
        if (to.meta.requiresAdmin) {
          next();   
        } else if (to.path == '/'){
          next("/admin-dashboard");  
        }
        else{
          next("/admin-dashboard");
        }
      } 
      else if (to.meta.requiresEmployee && role !== "Employee" && !logOut) {
        next("/admin-dashboard"); 
      } 
      else if (to.meta.requiresAdmin && role !== "Admin" && !logOut) {
        next("/employee-dashboard");   
      }
      else if (logOut){
        next();   
      }
    } 
     
    else {
      next();  
    }
  
});

export default router;
