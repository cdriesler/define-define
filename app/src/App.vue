<template>
  <div class="app" id="app">
    <div class="header">
      DEFINE-DEFINE
    </div>
    <div class="content">
      <router-view/>
    </div>
    <div class="footer">
      <div class="footer__version">
        V1.0.0
      </div>
       UID: {{uid}}
    </div>
  </div>
</template>

<style>
* {
  margin: 0;
  padding: 0;
}

.app {
  font-family: 'Quantico', sans-serif;
  -moz-osx-font-smoothing: grayscale;
  text-align: left;
  color: black;

  overflow-y: hidden;

  display: flex;
  flex-direction: column;
  justify-content: flex-start;

  height: calc(100vh - 25px);
}

.header {
  height: 25px;
  border-top: 2px solid black;
  border-bottom: 2px solid black;
  border-left: 2px solid black;
  border-right: 2px solid black;

  line-height: 25px;
  vertical-align: middle;
  font-size: 12px;
  font-weight: bold;

  width: calc(100% - 39px);
  padding-left: 5px;
  margin-left: 15px;
  margin-right: 15px;
  text-align: left;
}

.content {
  width: calc(100% - 30px);
  margin-left: 15px;
  margin-right: 15px;
  flex-grow: 1;
  overflow-y: auto;
}

.footer {
  height: 25px;

  position: absolute;
  bottom: 0;

  display: flex;
  flex-direction: row;
  justify-content: flex-start;

  font-size: 12px;
  font-weight: bold;
  line-height: 25px;

  width: calc(100% - 39px);
  margin-left: 15px;
  margin-right: 15px;
  padding-left: 5px;

  background: white;

  border-top: 2px solid black;
  border-right: 2px solid black;
  border-left: 2px solid black;

  z-index: 100;
}

.footer__version {
  height: 100%;
  border-right: 2px solid black;
  padding-right: 5px;
  margin-right: 5px;
}
</style>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data() {
    return {
      uid: "",
    }
  },
  mounted() {
    let uid = Math.round(Math.random() * 10000).toString();
    this.$store.commit("setUid", uid)
    this.uid = this.$store.state.uid;
  }
})
</script>