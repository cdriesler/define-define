export interface ProtocolStatement {
    title: string;
    index: string;
    statement: string;
    overrides: {
        word: string,
        type: string,
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
                type: "text"
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
                type: "number__slider"
            },
            {
                word: "open",
                type: "curve__nonvertical"
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
                type: "curve__connect"
            },
            {
                word: "large",
                type: "number__slider"
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
                type: "area__erasure"
            },
            {
                word: "parallel",
                type: "curve__parallel"
            }
        ]
    }

]