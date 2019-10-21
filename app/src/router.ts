import Vue from 'vue';
import Router from 'vue-router';
import Protocol from './views/Protocol.vue';
import Viewer from './views/Viewer.vue';
import Thanks from './views/Thanks.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'protocol',
      component: Thanks,
    },
    {
      path: '/input',
      name: 'input',
      component: Protocol,
    },
    {
      path: '/output',
      name: 'output',
      component: Viewer,
    },
    {
      path: '/thanks',
      name: 'thanks',
      component: Thanks,
    },
    {
      path: '**',
      redirect: '/thanks'
    }
  ],
});
