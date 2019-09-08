<template>
    <div id="override">
        
    </div>
</template>

<style>
#override {
    width: 800px;
    max-width: calc(100vw - 30px);
    height: 800px;
    max-height: calc(100vw - 30px);

    box-sizing: border-box;
    border: 2px solid black;

    margin-top: 15px;
    margin-bottom: 15px;
}
</style>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
    name: "p-override",
    props: ["label", "inputType"]
})
</script>