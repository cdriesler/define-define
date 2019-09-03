import * as functions from 'firebase-functions';

import * as admin from 'firebase-admin';
import * as express from 'express';
import * as bodyParser from 'body-parser';

import * as cors from 'cors';

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
    db.collection('drawings').doc('6RhW4SFkP77joXssta3X').collection('layers').doc('pjTr2xIZyOIQPBO3hsez').get()
        .then(x => {
            res.status(200).send(x.get("coordinates"));
        })
        .catch(error => {
            res.status(400).send(error);
        })
});