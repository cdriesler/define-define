<template>
    <div id="viewer">
        <div ref="svgar" class="artboard">
            <div class="artboard--current" v-html="svg">
            </div>
            <div class="artboard--past" v-for="(kept, i) in storedDrawings" :key="i" :class="{'artboard--previous':i == 0, 'artboard--archived':i >= 1}" v-html="storedDrawings[i]">
            </div>
        </div>
        <div class="queue">
            <div class="queue__header">
                https://define-define.web.app
            </div>
            <div class="queue_timer">
            </div>
            <queue-card
            v-for="(entry, index) in cache"
            :key="entry.fsid + index"
            :uid="entry.uid"
            :iid="entry.iid"
            :isActive="index == 0"></queue-card>
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

.artboard--current {
    position: absolute;
    left: 17px;
    top: 17px;
}

.artboard--current svg g path {
    stroke-dasharray: 1000;
    stroke-dashoffset: 1000;
    animation-name: dash;
    animation-duration: 4s;
    animation-fill-mode: forwards;
}

@keyframes dash {
    from {
        stroke-dashoffset: 1000;
    }
    to {
        stroke-dashoffset: 0;
    }
}

.artboard--archived {
    position: absolute;
    left: 17px;
    top: 17px;

    opacity: 0.05;
}

.artboard--previous svg g path {
    animation-name: fadepath;
    animation-duration: 4s;
    animation-fill-mode: forwards;
}

@keyframes fadepath {
    from {
        opacity: 1;
    }
    to {
        opacity: .10;
    }
}

.queue {
    flex-grow: 1;

    margin-top: 15px;
}

.queue__header {
    width: calc(100% - 15px);
    height: 50px;

    box-sizing: border-box;

    text-align: center;
    line-height: 46px;
    vertical-align: middle;

    margin-bottom: 15px;

    border: 2px solid black;
}

.queue_timer {
    margin-bottom: 15px;
    border-bottom: 2px solid black;

    animation-name: timer;
    animation-duration: 20s;
    animation-iteration-count: infinite;
    animation-timing-function: linear;
}

@keyframes timer {
    from {
        width: 0%;
    }
    to {
        width: 100%;
    }
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

    .queue__header {
        width: 100%;
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
import QueueCard from '../components/QueueCard.vue';
import DrawingManifest from '../models/DrawingManifest';
import { Drawing, Layer, State, GeometryElement, StyleBuilder, PolylineBuilder } from 'svgar';

interface Snapshot {
    fsid: string,
    uid: string,
    iid: string,
    did: string,
}

export default Vue.extend({
    name: "viewer",
    components: {
        QueueCard,
    },
    data() {
        return {
            uri: "",
            drawing: {} as DrawingManifest,
            w: 0,
            timer: '',
            cache: [] as Snapshot[],
            cacheEndpoint: '',
            activeSnapshot: {} as Snapshot,
            activeDrawing: {} as DrawingManifest,
            activeSvg: '',
            storedDrawings: [] as string[],
            svgar: {} as Drawing,
        }
    },
    created() {
        this.uri = process.env.VUE_APP_FCT_URL;
        
        this.cacheEndpoint = `${this.uri}/d`
        this.updateCache();
        this.startInterval();
        this.startConsume();
    },
    mounted() {
        let canvas:any = this.$refs.svgar;
        this.w = canvas.clientWidth;
    },
    computed: {
        svg(): string {
            let data:DrawingManifest = this.activeDrawing;

            if (data.Debug == undefined) {
                return "";
            }
            // console.log(data);
            let dwg = new Drawing("main");

            // Compose layer geometries
            // let background = new Layer("background").AddTag("background").AddGeometry(new PolylineBuilder([0,0]).LineTo([1,0]).LineTo([1,1]).LineTo([0,1]).LineTo([0,0]).Build());
            // dwg.AddLayer(background);

            let debug = new Layer("debug");
            data.Debug.forEach(x => {
                debug.AddGeometry(new GeometryElement(x).AddTag("debug"))
            });
            // data.Extensions.forEach(x => {
            //     debug.AddGeometry(new GeometryElement(x).AddTag("debug"))
            // });
            // data.Holes.forEach(x => {
            //     debug.AddGeometry(new GeometryElement(x).AddTag("debug"))
            // });
            dwg.AddLayer(debug);

            // let edges = new Layer("edges-l").AddTag("e");
            // data.Edges.forEach(x => {
            //     edges.AddGeometry(new GeometryElement(x).AddTag("edges"));
            // });
            // dwg.AddLayer(edges);

            // let extensions = new Layer("extensions-l").AddTag("x");
            // data.Extensions.forEach(y => {
            //     extensions.AddGeometry(new GeometryElement(y).AddTag("extensions"));
            // });
            // dwg.AddLayer(extensions);

            // let holes = new Layer("holes-l");
            // data.Holes.forEach(x => {
            //     holes.AddGeometry(new GeometryElement(x).AddTag("holes"));
            // });
            // dwg.AddLayer(holes);

            // let parallels = new Layer("parallels-l").AddTag("p");
            // data.Parallels.forEach(z => {
            //     parallels.AddGeometry(new GeometryElement(z).AddTag("parallels"));
            // });
            // dwg.AddLayer(parallels);

            // Compose drawing style
            let style = new State("main-state")
            .AddStyle(new StyleBuilder("debug-style").Fill("none").Stroke("#000000").StrokeWidth("2px").Build()).Target("debug-style", "debug");
            //.AddStyle(new StyleBuilder("edges-style").Fill("none").Stroke("#000000").StrokeWidth("3px").Build()).Target("edges-style", "edges")
            //.AddStyle(new StyleBuilder("extensions-style").Fill("none").Stroke("#000000").StrokeWidth("2px").Build()).Target("extensions-style", "extensions")
            //.AddStyle(new StyleBuilder("holes-style").Fill("#dcdcdc").Stroke("#000000").StrokeWidth("4px").Build()).Target("holes-style", "holes")
            //.AddStyle(new StyleBuilder("parallels-style").Fill("none").Stroke("#000000").StrokeWidth("1px").Build()).Target("parallels-style", "parallels");

            dwg.AddState(style);

            this.svgar = dwg;

            let latest = dwg.Compile("main-state", this.w, this.w);
            this.activeSvg = latest;

            return latest;
        }
    },
    methods: {
        startInterval(): void {
            setInterval(() => this.updateCache(), 2500);
        },
        startConsume(): void {
            setInterval(() => this.consumeCache(), 20000);
        },
        consumeCache(): void {
            if (this.cache.length > 1) {
                this.activeSnapshot = this.cache[1];
                this.cache = this.cache.slice(1, this.cache.length);

                let dest = `${this.uri}/d/${this.activeSnapshot.did}`

                this.updateStoredDrawings();
                this.activeDrawing = {};

                this.$http.get(dest)
                .then(dwg => {
                    //console.log(dwg.data);
                    
                    this.activeDrawing = new DrawingManifest(dwg.data);
                    //console.log(this.activeDrawing);

                    //Delete consumed doc from drawing_cache on firestore.
                    let del = `${this.uri}/d/${this.cache[0].fsid}`
                    return this.$http.delete(del)
                })
                .then(() => {
                    // Delete consumed snapshot from local this.cache
                    
                })
                .catch(err => {
                    console.log(err.message);
                })
            }
            else if (Object.keys(this.activeDrawing).length == 0) {
                this.activeSnapshot = this.cache[0];

                let dest = `${this.uri}/d/${this.activeSnapshot.did}`

                this.$http.get(dest)
                .then(dwg => {
                    this.activeDrawing = new DrawingManifest(dwg.data);
                })
                .catch(err => {
                    console.log(err.message);
                })
            }
        },
        updateDrawing(snap: Snapshot): void {

        },
        updateCache(): void {
            this.$http.get(this.cacheEndpoint)
            .then((x:any) => {
                x.data.forEach((y:any) => {
                    let snap: Snapshot = {
                        fsid: y.fsid,
                        uid: y.uid,
                        iid: y.input_id,
                        did: y.drawing_id
                    }
                    if (!this.cache.map(x => x.did).includes(snap.did)) {
                        this.cache.push(snap);
                    }
                })
            })
            .catch(err => {
                console.log(err);
            })
        },
        updateStoredDrawings(): void {
            if (this.storedDrawings.length >= 4) {
                let keep = this.storedDrawings.slice(1, 4);

                this.storedDrawings = [
                    this.activeSvg,
                ]
            }
            else {
                this.storedDrawings.push(this.activeSvg);
            }
        }
    }
})
</script>