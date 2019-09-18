import Vue from 'vue';
import Vuex from 'vuex';
import InputSnapshot from './models/InputSnapshot';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    uid: "",
    queue: [] as InputSnapshot[],
  },
  mutations: {
    setUid(state, uid: string) {
      state.uid = uid;
    },
    addToQueue(state, input: InputSnapshot) {
      state.queue.push(input);
    },
    removeFromQueue(state, iid: string) {
      state.queue = state.queue.filter(x => x.iid != iid);
    }
  },
  actions: {

  },
});
