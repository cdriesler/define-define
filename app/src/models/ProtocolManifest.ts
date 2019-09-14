export interface ProtocolStatement {
    title: string;
    index: string;
    statement: string;
    overrides: {
        word: string,
        eid: string,
        instructions: string,
    }[]
}

export const ProtocolManifest:ProtocolStatement[] = [
    {
        index: "00",
        title: "Intent",
        statement: "This project is a protocol for the creation of some drawing. Click words like this to override our definitions and alter the final output.",
        overrides: [
            {
                word: "this",
                eid: "tutorial",
                instructions: "draw anything; this one does nothing",
            }
        ]
    },
    {
        index: "01",
        title: "Massing",
        statement: "The drawing represents two adjacent and open volumetric surfaces.",
        overrides: [
            {
                word: "adjacent",
                eid: "adjacent",
                instructions: "draw two lines that are relatively close"
            },
            {
                word: "open",
                eid: "openings",
                instructions: "draw an open angle from two lines"
            },
        ]
    },
    {
        index: "02",
        title: "Connection",
        statement: "The surfaces connect via disjoint extensions of large edges.",
        overrides: [
            {
                word: "disjoint",
                eid: "disjoint",
                instructions: "draw two completely unrelated lines"
            },
            {
                word: "large",
                eid: "largethreshold",
                instructions: "draw a large line"
            }
        ]
    },
    {
        index: "03",
        title: "Texture",
        statement: "These surfaces are porous and have parallel ridges.",
        overrides: [
            {
                word: "porous",
                eid: "porosity",
                instructions: "draw a small shape inside a big shape",
            },
            {
                word: "parallel",
                eid: "parallel",
                instructions: "draw two parallel lines",
            }
        ]
    }

]