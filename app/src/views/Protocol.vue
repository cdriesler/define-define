<template>
    <div id="protocol" :class="{inactive: activeOverrides.length > 0 , active: activeOverrides.length == 0}">
        <div v-for="statement in manifest" :key="statement.statement" class="statement">
            <div class="statement__title" :class="{inactive: activeOverrides.length > 0 }">
                {{statement.title.toUpperCase()}}
            </div>
            <div class="statement__body">
                <span v-for="word in statement.statement.split(' ')" :key="word">
                    <span v-if="statement.overrides.map(x => x.word).includes(word)" class="statement__body--override" @click="toggleOverride(word)">
                        [{{word}}]&nbsp;
                        <div v-if="activeOverrides.includes(word)">
                            <protocol-override :label="word" :inputType="statement.overrides.find(x => x.word == word).type"></protocol-override>
                        </div>
                    </span>
                    <span v-else>
                    {{word}}&nbsp;
                    </span>
                </span>
            </div> 
        </div>
    </div>
</template>

<style>
#protocol {
    text-align: justify;
}

.inactive {
    color: gainsboro;
    border-color: gainsboro;
}

.active {
    border-color: black;
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

    border-bottom: 1px dashed;
    border-top: 1px dashed;
}

.statement__body {
    font-size: 24px;
    line-height: 36px;
}

.statement__body--override:hover {
    cursor: pointer;
    font-weight: bold;
}
</style>

<script lang="ts">
import Vue from 'vue'
import ProtocolOverride from './../components/ProtocolOverride.vue';
import { ProtocolManifest, ProtocolStatement } from '../models/ProtocolManifest';

export default Vue.extend({
    name: "protocol",
    components: {
        ProtocolOverride
    },
    data() {
        return {
            manifest: [] as ProtocolStatement[],
            activeOverrides: [] as string[],
        }
    },
    mounted() {
        this.manifest = ProtocolManifest;
    },
    methods: {
        toggleOverride(word: string): void {
            this.activeOverrides = this.activeOverrides.includes(word) ? [] : [word];
        },
    }
})
</script>