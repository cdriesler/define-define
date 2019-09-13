using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Define.Api
{
    public class InputManifest
    {
        // Statement 00
        public double Tutorial { get; set; } = 0.5;

        // Statement 01
        public double Adjacent { get; set; } = 0.5;
        public double Openings { get; set; } = 0.5;

        // Statement 02
        public double Disjoint { get; set; } = 0.5;
        public double Largethreshold { get; set; } = 0.5;

        // Statement 03
        public double Porosity { get; set; } = 0.5;
        public double Parallel { get; set; } = 0.5;
    }
}
