import Vue from 'vue';
import Router from 'vue-router';
import Protocol from './views/Protocol.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'protocol',
      component: Protocol,
    },
    {
      path: '**',
      redirect: '/'
    }
  ],
});
