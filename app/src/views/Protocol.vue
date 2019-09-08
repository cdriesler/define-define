<template>
    <div id="protocol" :class="{inactive: activeOverrides.length > 0 , active: activeOverrides.length == 0}">
        <div v-for="statement in manifest" :key="statement.statement" class="statement">
            <div class="statement__title" ref="c"
            :class="{active: statement.statement.split(' ').includes(activeOverrides.length > 0 ? activeOverrides[0] : 'never'), inactive: activeOverrides.length > 0 && !statement.statement.split(' ').includes(activeOverrides[0])}"
            @click="onToggleStatement(statement.index)">
                <div class="statement__title__index" :class="{inactive: activeOverrides.length > 0 && !statement.statement.split(' ').includes(activeOverrides[0])}">
                    {{statement.index}}
                </div>
                <div class="statement__title__name">
                    {{statement.title.toUpperCase()}}
                </div>   
            </div>
            <div v-show="activeStatements.includes(statement.index)" class="statement__body">
                <div v-for="word in statement.statement.split(' ')" :key="word">
                    <span 
                    v-if="statement.overrides.map(x => x.word).includes(word)" 
                    class="statement__body--override statement__body__word" 
                    :class="{'statement__body--active': activeOverrides.includes(word)}"
                    @click="toggleOverride(word)">
                        {{word.toUpperCase()}}
                    </span>
                    <span class="statement__body__word" v-else>
                    {{word.toUpperCase()}}
                    </span>
                    <div v-if="activeOverrides.includes(word)">
                        <protocol-override 
                        :label="word" 
                        :inputType="statement.overrides.find(x => x.word == word).type"
                        :w="currentSize"
                        @cancel="onCancel"></protocol-override>
                    </div>
                </div>
            </div> 
        </div>
    </div>
</template>

<style>
#protocol {
    text-align: justify;
}

.landing {
    margin-top: 15px;
    margin-bottom: 15px;
    background: green;
    padding: 0;
    width: 100%;
    height: calc(100vw - 40px);
}

.inactive {
    color: gainsboro;
    border-color: gainsboro !important;
}

.active {
    border-color: black;
    color: black;
}

.statement__title {
    margin-top: 15px;
    margin-bottom: 15px;

    display: flex;
    flex-direction: row;
    justify-content: space-between;

    position: sticky;
    top: -2px;

    font-size: 12px;
    font-weight: bold;
    line-height: 25px;
    vertical-align: middle;
    text-align: center;

    height: 25px;
    width: calc(100% - 4px);

    border: 2px solid black;
    background: white;
    z-index: 100;
}

.statement__title__index {
    height: 100%;
    width: 25px;
    border-right: 2px solid black;
}

.statement__title__name {
    height: 100%;

    flex-grow: 1;

    text-align: right;

    margin-right: 5px;
}

.statement__body {
    font-size: 25px;
    line-height: 40px;

    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: space-between;

    animation-name: body_appear;
    animation-duration: 0.8s;
    animation-fill-mode: forwards;
}

@keyframes body_appear {
    from {
        opacity: 0;
    }
    to {
        opacity: 1;
    }
}

.statement__body__word {
    margin-left: 5px;
    margin-right: 5px;
}

.statement__body--active {
    color: black;
}

.statement__body--override {
    font-weight: bold;
    text-decoration: underline;
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
            activeStatements: ["00"],
        }
    },
    mounted() {
        this.manifest = ProtocolManifest;
    },
    computed: {
        currentSize(): number {
            return (<Element>this.$refs.c).clientWidth;
        }
    },
    methods: {
        toggleOverride(word: string): void {
            if (this.activeOverrides.includes(word)) {
                return;
            }

            this.activeOverrides = [word];
        },
        onToggleStatement(index: string): void {
            if (this.activeStatements.includes(index)) {
                this.activeStatements = this.activeStatements.filter(x => x != index);
            }
            else {
                this.activeStatements.push(index);
            }
        },
        onCancel(): void {
            this.activeOverrides = [];
        }
    }
})
</script>