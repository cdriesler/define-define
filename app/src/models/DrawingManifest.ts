export default class DrawingManifest {

    public Debug: number[][] = [];
    public Edges: number[][] = [];
    public Extensions: number[][] = [];
    public Holes: number[][] = [];
    public Parallels: number[][] = [];

    constructor(req: any) {
        this.Debug = req["Debug"];
        this.Edges = req["Edges"];
        this.Extensions = req["Extensions"];
        this.Holes = req["Holes"];
        this.Parallels = req["Parallels"];
    }
}