import Vue from 'vue';
import Router from 'vue-router';
import Protocol from './views/Protocol.vue';
import Viewer from './views/Viewer.vue';

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
      path: '/define',
      name: 'viewer',
      component: Viewer,
    },
    {
      path: '**',
      redirect: '/'
    }
  ],
});
