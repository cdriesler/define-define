export interface ProtocolStatement {
    title: string;
    statement: string;
    overrides: {
        word: string,
        type: string,
    }[]
}

export const ProtocolManifest:ProtocolStatement[] = [
    {
        title: "Statement 01: Massing",
        statement: "This drawing represents two adjacent and open volumetric surfaces",
        overrides: [
            {
                word: "adjacent",
                type: "number__slider"
            },
            {
                word: "open",
                type: "curve__nonvertical"
            },
            {
                word: "surfaces",
                type: "text"
            }
        ]
    },
    {
        title: "Statement 02: Connection",
        statement: "The surfaces connect via disjoint extensions of large edges",
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
        title: "Statement 03: Texture",
        statement: "These surfaces are porous and corrugated",
        overrides: [
            {
                word: "porous",
                type: "area__erasure"
            },
            {
                word: "corrugated",
                type: "curve__parallel"
            }
        ]
    }

]