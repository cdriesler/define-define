<template>
    <div id="protocol">
        <div v-for="statement in manifest" :key="statement.statement" class="statement">
            <div class="statement__title">
                {{statement.title.toUpperCase()}}
            </div>
            <div class="statement__body">
                {{statement.statement}}
            </div> 
        </div>
        <!-- This drawing represents two (adjacent) (open) volumetric (surfaces).
        <br>
        The surfaces connect via disjoint (extensions) of (large) edges.
        <br>
        These surfaces are (porous) and (corrugated). -->
    </div>
</template>

<style>
#protocol {
    text-align: justify;
    overflow-y: auto;
}

.statement__title {
    margin-top: 15px;
    margin-bottom: 15px;

    font-size: 10px;
    line-height: 20px;
    vertical-align: middle;
    text-align: center;

    height: 20px;
    width: 100%;

    border-bottom: 1px dashed black;
    border-top: 1px dashed black;
}

.statement__body {
    font-size: 24px;
    line-height: 36px;
}
</style>

<script lang="ts">
import Vue from 'vue'
import { ProtocolManifest, ProtocolStatement } from '../models/ProtocolManifest';

export default Vue.extend({
    name: "protocol",
    data() {
        return {
            manifest: [] as ProtocolStatement[],
        }
    },
    mounted() {
        this.manifest = ProtocolManifest;
    }

})
</script>