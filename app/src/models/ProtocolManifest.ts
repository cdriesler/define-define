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
                instructions: "draw anything; we promise this one does nothing",
            }
        ]
    },
    {
        index: "01",
        title: "Massing",
        statement: "The drawing represents two adjacent shapes with gently banking edges.",
        overrides: [
            {
                word: "adjacent",
                eid: "adjacent",
                instructions: "draw two lines that are relatively close"
            },
            {
                word: "gently",
                eid: "openings",
                instructions: "draw a wide angle from two lines"
            },
        ]
    },
    {
        index: "02",
        title: "Connection",
        statement: "The shapes connect via disjoint extensions from their large edges.",
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
        statement: "These shapes are porous and enclose parallel lines.",
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