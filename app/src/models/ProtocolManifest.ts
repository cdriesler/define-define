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
        title: "Subdivision",
        statement: "Divide a square into three even sections with two non-vertical lines.",
        overrides: [
            {
                word: "even",
                eid: "adjacent",
                instructions: "divide with two relatively close lines"
            },
            {
                word: "non-vertical",
                eid: "openings",
                instructions: "draw lines that aren't vertical"
            },
        ]
    },
    {
        index: "02",
        title: "Connection",
        statement: "Connect the lines with extensions from their large segments that almost touch.",
        overrides: [
            {
                word: "almost",
                eid: "disjoint",
                instructions: "draw lines from the border that almost touch"
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
        title: "Aggregation",
        statement: "Draw a small shape in the left and right regions. Repeat them throughout.",
        overrides: [
            {
                word: "small",
                eid: "porosity",
                instructions: "draw a small shape inside a big shape",
            },
            {
                word: "Repeat",
                eid: "parallel",
                instructions: "draw two parallel lines",
            }
        ]
    }

]