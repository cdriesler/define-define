<template>
<div id="qc">
    <div class="card__id">
        {{uid}}
    </div>
    <div class="card__values">
        <div class="card__values__val" v-for="(i, index) in inputs" :key="index">
            {{i}}
        </div>
    </div>
</div>
</template>

<style>
#qc {
    width: 100%;
    height: 50px;
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    justify-content: flex-start;

    margin-bottom: 15px;
    
    animation-name: qcin;
    animation-duration: 0.8s;
    animation-fill-mode: forwards;
}

@keyframes qcin {
    from {
        opacity: 0;
    }
    to {
        opacity: 1;
    }
}

.card__id {
    height: 100%;
    width: 50px;

    box-sizing: border-box;
    margin-right: 15px;

    font-size: 14px;
    line-height: 46px;
    vertical-align: middle;
    text-align: center;

    border: 2px solid black;
}

.card__values {
    flex-grow: 1;
    height: 100%;

    display: flex;
    flex-direction: row;
    justify-content: flex-start;
}

.card__values__val {
    width: 50px;
    height: 100%;

    font-size: 12px;
    line-height: 50px;
    vertical-align: middle;
    text-align: center;
}

</style>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
    name: "queue-card",
    props: ["uid", "iid"],
    data() {
        return {
            inputs: [] as number[],
            destination: '',
        }
    },
    created() {
        this.destination = `${process.env.VUE_APP_FCT_URL}/in/${this.iid}`
        this.$http.get(this.destination)
        .then(x => {
            console.log(x.data);
            Object.keys(x.data).forEach(y => {
                this.inputs.push(Math.round(+x.data[y] * 100));
            })
        });
    }
})
</script>