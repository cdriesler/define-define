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

// Store input data and returned packaged input manifest
app.post('/in/:id', (req, res) => {
    //db.collection("input_cache").doc(req.params.id)
    var val: number;
    var manifest: any;
    axios.default.post(`${process.env.API_URL}/in/${req.params.id}`, {paths: req.body["paths"]})
    .then(r => {
        // Get measurement from rhino api
        console.log(r);
        val = +r.data["measurement"];

        return db.collection("input_cache").doc(req.params.id).get();
    })
    .then(doc => {
        // Construct new doc for current input with given data
        let cache: number[] = doc.get("cache");
        let keep: number[] = [];

        if (cache.length >= 10) {
            keep = cache.slice(1, 10);
        }
        else {
            keep = cache;
        }

        keep.push(val);

        let newdoc = {
            current: keep.reduce(function(a, b) { return a + b; }) / cache.length,
            cache: keep,
        }

        return db.collection("input_cache").doc(req.params.id).set(newdoc);
    })
    .then(doc => {
        return db.collection("input_cache").listDocuments();
    })
    .then(docs => {
        // Get all values in cache
        let inputs: Promise<FirebaseFirestore.DocumentSnapshot>[] = [];
        docs.forEach(doc => {
            inputs.push(doc.get());
        });
        return Promise.all(inputs);
    })
    .then(inputs => {
        // Parse current values from cache
        inputs.forEach(x => {
            manifest[x.id] = +x.get("current")
        });

        manifest["timestamp"] = new Date();

        return db.collection("input_history").doc().set(manifest);
    })
    .then(doc => {
        // Store manifest in history and return to frontend
        res.status(200).json(manifest);
    })
    .catch(err => {
        res.status(400).json(err);
    });
})