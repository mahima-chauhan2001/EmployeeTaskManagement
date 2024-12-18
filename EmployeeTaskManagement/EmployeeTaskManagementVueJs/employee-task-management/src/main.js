import Vue from 'vue'
import App from './App.vue'
import router from './Router';
import Vuetify from 'vuetify';
import 'vuetify/dist/vuetify.min.css';
import '@mdi/font/css/materialdesignicons.css';

Vue.config.productionTip = false
Vue.use(Vuetify);
new Vue({
  vuetify: new Vuetify({
    icons:{
      iconfont: "mdi",
    }
  }),
  render: h => h(App),
  router,
}).$mount('#app')
