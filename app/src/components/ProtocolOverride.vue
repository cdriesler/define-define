<template>
    <div id="override">
        <div class="input">
        </div> 
        <div class="actions">
            <div class="actions--cancel" @click="onCancel">
                cancel
            </div>
            <div class="actions--define">
                ?
            </div>
            <div class="actions--submit">
                submit
            </div>
        </div>
    </div>
</template>

<style>
#override {
    overflow-y: hidden;

    animation-name: appear;
    animation-duration: 0.8s;
    animation-fill-mode: forwards;
}

@keyframes appear {
    from {
        opacity: 0;
    }
    to {
        opacity: 1;
    }
}

.input {
    width: 800px;
    max-width: calc(100vw - 30px);
    height: 800px;
    max-height: calc(100vw - 30px);

    box-sizing: border-box;
    border: 2px solid black;

    margin-top: 15px;
}

.actions {
    width: 800px;
    max-width: calc(100vw - 30px);
    height: 25px;
    box-sizing: border-box;

    border-left: 2px solid black;
    border-right: 2px solid black;
    border-bottom: 2px solid black;

    display: flex;
    flex-direction: row;
    justify-content: flex-start;

    margin-bottom: 15px;

    color: black;
    font-size: 12px;
    line-height: 23px;
    vertical-align: middle;
    text-align: center;
}

.actions--cancel {
    flex-grow: 1;

    height: 100%;
}

.actions--define {
    flex-grow: 1;

    height: 100%;

    border-right: 2px solid black;
    border-left: 2px solid black;
}

.actions--submit {
    flex-grow: 1;

    height: 100%;
}
</style>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
    name: "p-override",
    props: ["label", "inputType"],
    methods: {
        onCancel(): void {
            this.$emit("cancel");
        }
    }
})
</script>