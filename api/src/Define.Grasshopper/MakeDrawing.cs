using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Define.Api;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Define.Grasshopper
{
    public class MakeDrawing : GH_Component
    {
        // Constructor with name and tab placement
        public MakeDrawing() : base("MakeDrawing", "Define", "Debug utility for making the define-defin drawing", "Define", "Define")
        {

        }

        // Inputs
        protected override void RegisterInputParams(GH_InputParamManager pManager) 
        {
            pManager.AddNumberParameter("Tutorial", "T", "this", GH_ParamAccess.item);

            pManager.AddNumberParameter("Adjacent", "A", "adjacent", GH_ParamAccess.item);
            pManager.AddNumberParameter("Open", "O", "openings", GH_ParamAccess.item);

            pManager.AddNumberParameter("Disjoint", "D", "disjoint", GH_ParamAccess.item);
            pManager.AddNumberParameter("Largethreshold", "L", "large", GH_ParamAccess.item);

            pManager.AddNumberParameter("Porosity", "P", "porous", GH_ParamAccess.item);
            pManager.AddNumberParameter("Parllel", "P", "parallel", GH_ParamAccess.item);
        }

        // Outputs
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Drawing", "D", "Output drawing", GH_ParamAccess.item); 
        }

        // Solving logic
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get data and construct drawing
            var inputs = new InputManifest();

            var tutorial = 0.0;
            var adjacent = 0.0;
            var openings = 0.0;
            var disjoint = 0.0;
            var largethreshold = 0.0;
            var porosity = 0.0;
            var parallel = 0.0;

            DA.GetData(0, ref tutorial);
            DA.GetData(1, ref adjacent);
            DA.GetData(2, ref openings);
            DA.GetData(3, ref disjoint);
            DA.GetData(4, ref largethreshold);
            DA.GetData(5, ref porosity);
            DA.GetData(6, ref parallel);

            inputs.Tutorial = tutorial;
            inputs.Adjacent = adjacent;
            inputs.Openings = openings;
            inputs.Disjoint = disjoint;
            inputs.Largethreshold = largethreshold;
            inputs.Porosity = porosity;
            inputs.Parallel = parallel;

            // Make drawing
            var drawing = new DrawingManifest(inputs);

            // Assign certain parts of drawing to grasshopper component output

        }

        protected override System.Drawing.Bitmap Icon
        {
            //get { return Properties.Resources.icon; }
            get { return null; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("258b5193-cd15-4305-9233-c9aa7134b0b9"); }
        }
    }
}
