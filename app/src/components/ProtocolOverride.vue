<template>
    <div id="override">
        <div 
        class="input" 
        ref="cv"
        @mousedown="onDown"
        @mousemove="onMove"
        @mouseup="onUp"
        @touchstart="onDown"
        @touchmove="onMove"
        @touchend="onUp"
        v-html="svgarPaths">
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
import Vue from 'vue';
import * as Svgar from 'svgar';

export default Vue.extend({
    name: "p-override",
    props: ["label", "inputType", "w"],
    data() {
        return {
            renderPath: {},
            svg: {},
            count: 0,
            coords: [] as number[][],
            activePath: [] as number[],
            size: 0,
        }
    },
    mounted() {
        let el = (<Element>this.$refs.cv);
        this.size = el == undefined ? 0 : el.clientWidth;
    },
    computed: {
        svgarPaths(): string {
            let dwg = new Svgar.Drawing("inputs");
            let layer = new Svgar.Layer("lines").AddTag("main");
            let style = new Svgar.State("default").AddStyle(new Svgar.StyleBuilder("bw").Fill("none").StrokeWidth("2px").Stroke("#000000").Build()).Target("bw", "main");

            this.coords.forEach(x => {
                let crv = new Svgar.PolylineBuilder([x[0], x[1]])
                for(let i = 2; i < x.length; i+=2) {
                    crv.LineTo([x[i], x[i+1]])
                }
                layer.AddGeometry(crv.Build());
            })

            dwg.AddLayer(layer).AddState(style);

            return dwg.Compile("default", this.size, this.size);
        },
    },
    methods: {
        onCancel(): void {
            this.$emit("cancel");
        },
        touchEventToNormalizedCoordinate(event: TouchEvent): number[] {
            if (event.targetTouches.length <= 0) {
                return [];
            }

            let t = event.targetTouches[0];
            let div: DOMRect = (<any>event).path[1].getBoundingClientRect();

            let x = (t.pageX - div.left) / div.width;
            let y = 1 - ((t.pageY - div.top) / div.height);

            return [+x, +y];
        },
        onDown(event: TouchEvent): void {
            let coord = this.touchEventToNormalizedCoordinate(event);

            if (coord.length == 2) {
                this.activePath.push(coord[0]);
                this.activePath.push(coord[1]);
            }
        },
        onMove(event: TouchEvent): void {
            this.count++;

            if (this.count > 3) {
                this.count = 0;
                //Log new coordinate
                let coord = this.touchEventToNormalizedCoordinate(event);

                if (coord.length == 2) {
                    this.activePath.push(coord[0]);
                    this.activePath.push(coord[1]);
                }
            }
        },
        onUp(event: TouchEvent): void {
            console.log(this.activePath);

            this.coords.push(this.activePath);
            this.activePath = [];
        }
    }
})
</script>