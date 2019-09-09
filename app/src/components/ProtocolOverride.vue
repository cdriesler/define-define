<template>
    <div id="override">
        <div 
        class="input" 
        :class="{'definition': showDefine == true}"
        ref="cv"
        @mousedown="onDown"
        @mousemove="onMove"
        @mouseup="onUp"
        @touchstart="onDown"
        @touchmove="onMove"
        @touchend="onUp"
        >
        <div v-html="svgarPaths" v-if="showDefine == false"></div>
        </div> 
        <div class="definition">
            {{instructions.toUpperCase()}}
        </div>
        <div class="actions">
            <div class="actions__button" @click="onCancel">
                CANCEL
            </div>
            <div class="actions__button divider" @click="onReset">
                RESET
            </div>
            <div class="actions__button divider">
                SUBMIT
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

.inputpath {
    animation-name: appear;
    animation-duration: 0.8s;
    animation-fill-mode: forwards;
}

.definition {
    width: 800px;
    max-width: calc(100vw - 34px);
    height: 25px;
    border-left: 2px solid black;
    border-right: 2px solid black;
    border-bottom: 2px solid black;

    text-align: center;
    color: black;
    font-size: 12px;
    font-weight: bold;
    line-height: 25px;
    vertical-align: middle;
}


.definition__statement {
    font-size: 12px;
    font-weight: bold;
    text-align: center;
    padding: 15px;
    color: white;
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
    max-width: calc(100vw - 34px);
    height: 25px;

    display: flex;
    flex-direction: row;
    justify-content: space-between;

    margin-bottom: 15px;

    color: black;
    font-size: 12px;
    font-weight: bold;
    line-height: 25px;
    vertical-align: middle;
    text-align: center;

    border-bottom: 2px solid black;
    border-right: 2px solid black;
    border-left: 2px solid black;
}

.actions__button {
    flex-grow: 1;
    height: 100%;
}

.divider {
    border-left: 2px solid black;
}
</style>

<script lang="ts">
import Vue from 'vue';
import * as Svgar from 'svgar';

export default Vue.extend({
    name: "p-override",
    props: ["label", "inputType", "w", "instructions"],
    data() {
        return {
            renderPath: {},
            showDefine: false,
            svg: {},
            count: 0,
            coords: [] as number[][],
            activePath: [] as number[],
            size: 0,
            drawing: {},
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

            for(let i = 0; i < this.coords.length; i++) {
                let x = this.coords[i];
                let crv = new Svgar.PolylineBuilder([x[0], x[1]])
                for(let j = 2; j < x.length; j+=2) {
                    crv.LineTo([x[j], x[j+1]]);
                }

                let geo = crv.Build();

                if (i >= (<Svgar.Drawing>this.drawing).Layers[0].Geometry.length) {
                    geo.AddTag("inputpath");
                }

                layer.AddGeometry(geo);
            }

            // if (this.activePath.length >= 4) {
            //     const c = this.activePath;
            //     let current = new Svgar.PolylineBuilder([c[0], c[1]]);
            //     for (let i = 2; i < c.length; i+=2) {
            //         current.LineTo([c[i], c[i+1]]);
            //     }
            //     layer.AddGeometry(current.Build());
            // }

            dwg.AddLayer(layer).AddState(style);

            this.drawing = dwg;

            return dwg.Compile("default", this.size, this.size);
        },
    },
    methods: {
        onCancel(): void {
            this.$emit("cancel");
        },
        onReset(): void {
            this.coords = [];
        },
        onToggleDefine(): void {
            this.showDefine = !this.showDefine;
        },
        touchEventToNormalizedCoordinate(event: TouchEvent): number[] {
            if (event.targetTouches.length <= 0) {
                return [];
            }

            let t = event.targetTouches[0];
            let div: DOMRect = (<any>event).target.getBoundingClientRect();

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

            //disableBodyScroll(this.app);
        },
        onMove(event: TouchEvent): void {
            this.count++;

            event.preventDefault()
            event.stopPropagation()

            if (this.count > 1) {
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
            this.coords.push(this.activePath);
            this.activePath = [];

            //enableBodyScroll(this.app);
        }
    }
})
</script>