<template>
<div id="qc">
    <div class="card__id">
        {{uid}}
    </div>
    <div 
    class="card__values__val" 
    v-for="(i, index) in inputs" 
    v-show="categories[index] != 'tutorial'"
    :key="iid + index" 
    :style="{ backgroundColor: colorFromVal(i)}"
    :class="{'card__values__val--active' : isActive}">
        <span v-show="isActive">{{padLeft(i, 3)}}</span>
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

@media screen and (orientation: landscape) {
    #qc {
        width: calc(100% - 15px);
    }
}

.card__id {
    height: 100%;
    width: 50px;

    box-sizing: border-box;

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
    flex-grow: 1;
    height: 100%;

    margin-left: 15px;

    font-size: 12px;
    line-height: 50px;
    vertical-align: middle;
    text-align: center;
}

.card__values__val--active {
    outline: 2px solid black;
    outline-offset: -2px;

    background: none !important;
}

</style>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
    name: "queue-card",
    props: ["uid", "iid", "isActive"],
    data() {
        return {
            inputs: [] as number[],
            destination: '',
            categories: [] as string[],
            
        }
    },
    created() {
        this.destination = `${process.env.VUE_APP_FCT_URL}/in/${this.iid}`
        this.$http.get(this.destination)
        .then(x => {
            this.categories = Object.keys(x.data).sort();
            this.categories.forEach(y => {
                this.inputs.push(Math.round(+x.data[y] * 100));
            })
        });
    },
    methods: {
        colorFromVal(val: number): string {
            if (this.isActive) {
                return "#FFFFFF"
            }
            else {
                let scaled = Math.round((val / 100) * 200);
                let hex = scaled.toString(16)

                let color = "#" + hex + hex + hex;

                return color;
            }
        },
        padLeft(val: string, length: number): string {
            var str = "" + val
            var pad = ""

            for (let i = 0; i < length; i++) {
                pad = pad + '0';
            }

            return pad.substring(0, pad.length - str.length) + str;
        }
    }
})
</script>