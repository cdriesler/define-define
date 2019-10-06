<template>
    <div class="thanks">
        <div>
        PROTOCOL OFFLINE SINCE OCTOBER 2019
        <br>
        thanks for playing, that was fun
        <br>
        <a href="https://www.instagram.com/p/B3Qi9lTJvmQ/" target="_blank">[ some results ]</a>
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

</style>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
    
})
</script>