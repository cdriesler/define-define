export default class DrawingManifest {

    public Debug: number[][] = [];

    constructor(req: any) {
        this.Debug = req["Debug"];
    }
}