﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace compute.geometry.Models
{
    public class DrawingMaster
    {
        // Properties
        public Curve EdgeLeftFront { get; set; }
        public Curve EdgeLeftBehind { get; set; }
        public Curve EdgeRightFront { get; set; }
        public Curve EdgeRightBehind { get; set; }

        // Constructor
        public DrawingMaster()
        {
            //Default logic

        }

        public void Compose()
        {

        }

        // Update methods
        public void UpdateLeftAboveEdge(Curve curve)
        {

        }




    }
}
