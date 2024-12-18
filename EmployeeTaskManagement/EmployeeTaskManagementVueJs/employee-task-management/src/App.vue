<template>
<div id="app">  
  <v-app>
    <router-view /> <!-- The main content of your application -->
  </v-app>
  </div>
</template>

<script> 

export default {
  name: 'App',
  components: { 
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
