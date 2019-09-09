import * as functions from 'firebase-functions';

import * as admin from 'firebase-admin';
import * as express from 'express';
import * as bodyParser from 'body-parser';

import * as cors from 'cors';

// import { PolylineBuilder } from 'svgar';

admin.initializeApp(functions.config().firebase);

// const db = admin.firestore();

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

// Store input data and returned packaged input manifest
app.post('/in/:id', (req, res) => {
    //db.collection("input_cache").doc(req.params.id)
    res.status(200).json({type: req.params.id, data: req.body["paths"].length});
})