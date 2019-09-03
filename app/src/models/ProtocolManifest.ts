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
        statement: "This drawing represents two (adjacent) (open) volumetric (surfaces).",
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
        statement: "The surfaces connect via disjoint (extensions) of (large) edges.",
        overrides: [

        ]
    },
    {
        title: "Statement 03: Texture",
        statement: "These surfaces are (porous) and (corrugated).",
        overrides: [

        ]
    }

]