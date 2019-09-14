<template>
    <div id="viewer">
        <div ref="svgar" class="artboard" v-html="svg">
        </div>
        <div class="queue">
            QUEUE
        </div>
    </div>
</template>

<style>
#viewer {
    position: absolute;
    left: 0;
    top: 0;

    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    align-items: flex-start;
    align-content: flex-start;

    width: 100vw;
    height: 100vh;

    background: white;

    z-index: 1000;

    overflow: hidden;
}

.artboard {
    border: 2px solid black;
    background: none;

    box-sizing: border-box;

    margin-top: 15px;
}

.queue {
    flex-grow: 1;

    margin-top: 15px;
}

@media screen and (orientation: portrait) {
    .artboard {
        width: calc(100vw - 30px);
        height: calc(100vw - 30px);
        margin-left: 15px;
        margin-right: 15px;
    }

    .queue {
        margin-left: 15px;
        margin-right: 15px;
    }
}

@media screen and (orientation: landscape) {
    .artboard {
        height: calc(100vh - 30px);
        width: calc(100vh - 30px);
        margin-left: 15px;
        margin-right: 15px;
    }
}
</style>

<script lang="ts">
import Vue from 'vue'
import DrawingManifest from '../models/DrawingManifest';
import { Drawing, Layer, State, GeometryElement, StyleBuilder } from 'svgar';

export default Vue.extend({
    name: "viewer",
    data() {
        return {
            uri: "",
            drawing: {} as DrawingManifest,
            w: 0,
            timer: '',
            cache: [],
            cacheEndpoint: ''
        }
    },
    created() {
        this.uri = process.env.VUE_APP_FCT_URL;
        let destination: string = `${this.uri}/d/fiHJx16YQQ0Plx5sItha`;
        this.$http.get(destination).then(dwg => {
            this.drawing = new DrawingManifest(dwg.data);
        });
        this.startInterval();
        this.cacheEndpoint = `${this.uri}/d`
    },
    mounted() {
        let canvas:any = this.$refs.svgar;
        this.w = canvas.clientWidth;
    },
    computed: {
        svg(): string {
            let data:DrawingManifest = this.drawing;

            if (data.Debug == undefined) {
                return "";
            }
            
            let dwg = new Drawing("main");

            let layer = new Layer("debug").AddTag("debug");

            data.Debug.forEach(x => {
                layer.AddGeometry(new GeometryElement(x));
            });

            dwg.AddLayer(layer);

            let style = new State("debug-state").AddStyle(new StyleBuilder("debug-style").Fill("none").Stroke("#000000").StrokeWidth("2px").Build()).Target("debug-style", "debug");

            dwg.AddState(style);

            return dwg.Compile(undefined, this.w, this.w);
        }
    },
    methods: {
        startInterval(): void {
            setInterval(() => this.updateCache(), 5000);
        },
        updateCache(): void {
            this.$http.get(this.cacheEndpoint)
            .then((x:any) => {
                x.data.forEach((y:any) => {
                    if (!this.cache.includes(y)) {
                        this.cache.push(y);
                        console.log(y);
                    }
                })
            })
        }
    }
})
</script>