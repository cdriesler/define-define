<template>
    <div id="override">
        {{label}}
    </div>
</template>

<style>

</style>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
    name: "p-override",
    props: ["label", "inputType"]
})
</script>