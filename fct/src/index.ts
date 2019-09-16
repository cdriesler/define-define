import * as functions from 'firebase-functions';

import * as admin from 'firebase-admin';
import * as express from 'express';
import * as bodyParser from 'body-parser';

import * as cors from 'cors';
import * as axios from 'axios';
import * as dotenv from 'dotenv';

dotenv.config();

// import { PolylineBuilder } from 'svgar';

admin.initializeApp(functions.config().firebase);

const db = admin.firestore();

const app = express();
const main = express();

const options: cors.CorsOptions = {
    allowedHeaders: ["Origin", "X-Requested-With", "Content-Type", "Accept", "X-Access-Token"],
    methods: "GET,HEAD,OPTIONS,PUT,PATCH,POST,DELETE",
    origin: true,
    preflightContinue: false
}

main.use(cors(options));
main.use('/v1', app);
main.use(bodyParser.json());
main.use(bodyParser.urlencoded({ extended: false }));
main.options("*", cors(options));

export const api = functions.https.onRequest(main);

// Get all registered routes
app.get('/', (req, res) => {
    res.status(200).json({message: "Under construction."});
});

// Test handshake with rhino api
app.get('/handshake', (req, res) => {
    //console.log(`${process.env.API_URL}/version`);
    axios.default.get(`${process.env.API_URL}/version`)
    .then(r => {
        res.status(200).json(r.data);
    })
    .catch(err => {
        res.status(400).json(err);
    });
})

// Get specific input from history
app.get('/in/:id', (req, res) => {
    db.collection("input_history").doc(req.params.id).get()
    .then(doc => {
        res.status(200).json(doc.data());
    })
    .catch(err => {
        res.status(400).json(err);
    });
})

// Store input data and returned packaged input manifest
app.post('/in/:id', (req, res) => {
    //db.collection("input_cache").doc(req.params.id)
    let val: number;
    let id: string;
    let inputmanifest: any = {};
    axios.default.post(`${process.env.API_URL}/in/${req.params.id}`, {paths: req.body["paths"]})
    .then(r => {
        // Get measurement from rhino api
        val = +r.data;

        return db.collection("input_cache").doc(req.params.id).get();
    })
    .then(doc => {
        // Construct new doc for current input with given data
        let cache: number[];
        let keep: number[] = [];

        if (doc === undefined) {
            cache = [];
        }
        else {
            cache = doc.get("cache");
        }

        if (cache === undefined) {
            cache = [];
        }

        if (cache.length >= 5) {
            keep = cache.slice(1, 5);
        }
        else {
            keep = cache;
        }

        keep.push(val);

        let total: number = 0;

        keep.forEach(x => {
            total = total + x;
        })

        const newdoc = {
            current: total / cache.length,
            cache: keep,
        }

        return db.collection("input_cache").doc(req.params.id).set(newdoc);
    })
    .then(doc => {
        return db.collection("input_cache").listDocuments();
    })
    .then(docs => {
        // Get all values in cache
        const inputs: Promise<FirebaseFirestore.DocumentSnapshot>[] = [];
        docs.forEach(doc => {
            inputs.push(doc.get());
        });

        return Promise.all(inputs);
    })
    .then(inputs => {
        // Parse current values from cache
        const manifest:any = {};

        inputs.forEach(x => {
            manifest[x.id] = x.get("current");
        });

        id = db.collection("input_history").doc().id;
        inputmanifest = manifest;
        return db.collection("input_history").doc(id).set(manifest);
    })
    .then(doc => {
        // Store manifest in history and return to frontend
        return db.collection("input_history").doc(id).get();
    })
    .then(end => {
        res.status(200).json({inputs: inputmanifest, inputid: id});
    })
    .catch(err => {
        res.status(400).json(err);
    });
});

// Get specific drawing from history
app.get('/d/:id', (req, res) => {
    let layernames: string[] = [];
    let layerlengths: number[] = [];
    db.collection("drawing_history").doc(req.params.id).listCollections()
    .then(layers => {
        let layercontents: Promise<FirebaseFirestore.DocumentReference[]>[] = [];
        layers.forEach(x => {
            layernames.push(x.id);
            layercontents.push(x.listDocuments());
        })

        return Promise.all(layercontents);
    })
    .then(geo => {
        let geoinfo: Promise<FirebaseFirestore.DocumentSnapshot>[] = [];
        geo.forEach(x => {
            layerlengths.push(x.length);
            x.forEach(y => {
                geoinfo.push(y.get());
            })
        });

        return Promise.all(geoinfo);
    })
    .then(data => {
        let count = 0;

        let drawing: any = {};

        for(let i = 0; i < layernames.length; i++) {
            let cache: number[][] = [];

            for(let j = count; j < count + layerlengths[i]; j++) {
                cache.push(data[j].get("path"));
            }

            drawing[layernames[i]] = cache;
        }

        res.status(200).json(drawing);
    })
    .catch(err => {
        res.status(400).json(err);
    })
})

interface DrawingCacheMap {
    fsid: string;
    uid: string;
    input_id: string;
    drawing_id: string;
}

// Get current drawing cache 
app.get('/d', (req, res) => {
    db.collection("drawing_cache").listDocuments()
    .then(docs => {
        let promises:Promise<FirebaseFirestore.DocumentSnapshot>[] = []

        docs.forEach(x => {
            promises.push(x.get());
        });

        return Promise.all(promises);
    })
    .then(c => {
        let latest: DrawingCacheMap[] = [];
        
        c.forEach(x => {
            let update: DrawingCacheMap = {
                fsid: x.id,
                uid: x.get("user_id"),
                input_id: x.get("input_id"),
                drawing_id: x.get("drawing_id"),
            }

            latest.push(update);
        })

        res.status(200).json(latest);
    })
    .catch(err => {
        res.status(400).json(err);
    })
})

// Generate drawing and add to cache
app.post('/d', (req, res) => {
    let dwg:any = {};
    let id = "";

    axios.default.post(`${process.env.API_URL}/d`, JSON.stringify(req.body.inputs))
    .then(r => {
        dwg = r.data;

        let docid = db.collection("drawing_cache").doc().id;
        id = docid;

        const dwgres:any = {}

        dwgres["uid"] = req.body.uid;

        const dwgdata:any = dwg;

        console.log(dwg)
    
        Object.keys(dwgdata).forEach(x => {
            dwgres[x] = dwgdata[x];
        })

        console.log(dwgres);

        return db.collection("drawing_history").doc(docid).set({uid: req.body.uid, input: req.body.inputid});     
    })
    .then(doc => {  
        let promises:Promise<FirebaseFirestore.WriteResult>[] = [];
  
        Object.keys(dwg).forEach((x:any) => {
            const paths:any[] = dwg[x];
            paths.forEach(y => {
                promises.push(db.collection("drawing_history").doc(id).collection(x).doc().set({path: y}));
            });
        });

        return Promise.all(promises);
    })
    .then(all => {
        return db.collection("drawing_cache").doc().set({drawing_id: id, user_id: req.body.uid, input_id: req.body.inputid});
    })
    .then(end => {
        res.status(200).json(dwg);
    })
    .catch(err => {
        console.log(err.message);
        res.status(400).json(err);
    })
});

// Remove drawing from cache
app.delete('/d/:id', (req, res) => {
    db.collection("drawing_cache").doc(req.params.id).delete()
    .then(x => {
        res.status(200).json(x.writeTime);
    })
    .catch(err => {
        res.status(400).json(err);
    })
})