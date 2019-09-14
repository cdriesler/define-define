<template>
    <div id="viewer">
        <div class="artboard" v-html="svg">
        </div>
    </div>
</template>

<style>

</style>

<script lang="ts">
import Vue from 'vue'
import DrawingManifest from '../models/DrawingManifest';
export default Vue.extend({
    name: "viewer",
    data() {
        return {
            uri: "",
            drawing: DrawingManifest,
        }
    },
    created() {
        this.uri = process.env.VUE_APP_FCT_URL;
        let destination: string = `${this.uri}/d`;
        this.$http.get(destination).then(dwg => {

        })
    },
    mounted() {
        
    },
    computed: {
        svg(): string {

            return "";
        }
    }
})
</script>