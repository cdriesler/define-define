<template>
  <div class="app">
    <div class="header">
      define-define
    </div>
    <div class="content">
      <router-view/>
    </div>
    <div class="footer">
      VERSION: 0.1.2 &nbsp; UID: XXXX
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

  height: 100vh;
}

.header {
  height: 25px;
  border-bottom: 2px solid black;
  border-left: 2px solid black;
  border-right: 2px solid black;
  border-bottom-left-radius: 10px;
  border-bottom-right-radius: 10px;

  line-height: 25px;
  vertical-align: middle;
  font-size: 12px;
  font-weight: bold;

  width: calc(100% - 34px);
  margin-left: 15px;
  margin-right: 15px;
  text-align: center;
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

  font-family: 'Consolas', sans-serif;
  font-size: 10px;
  line-height: 25px;

  width: calc(100% - 39px);
  margin-left: 15px;
  margin-right: 15px;
  padding-left: 5px;

  background: white;

  border-top: 2px solid black;
  border-right: 2px solid black;
  border-left: 2px solid black;
  border-top-left-radius: 10px;
  border-top-right-radius: 10px;
}
</style>
