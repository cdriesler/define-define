<template>
    <div class="thanks">
        <div>
        PROTOCOL OFFLINE SINCE OCTOBER 2019
        <br>
        thanks for playing, that was fun
        <br>
        <a href="https://www.instagram.com/p/B3Qi9lTJvmQ/" target="_blank">[ some results ]</a>
        <br>
        <span class="override" @click="goToInput">[ let me pretend it's still online ]</span>
        </div>
    </div>
</template>

<style scoped>
.thanks {
    text-align: center;
    line-height: 50px;
    vertical-align: middle;

    flex-grow: 1;

    display: flex;
    flex-direction: column;
    justify-content: center;
}

.thanks > * > a {
    color: black;
}

.thanks a:hover {
    background: gainsboro;
}

.override {
    text-decoration: underline;
}

.override:hover {
    background: gainsboro;
    cursor: pointer;
}

</style>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
    methods: {
        goToInput(): void {
            this.$router.push('/input');
        }
    }
})
</script>