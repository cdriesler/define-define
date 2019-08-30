using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace compute.geometry
{
    public class DrawingMaster
    {
        // Properties
        public Curve EdgeLeftFront { get; set; }
        public Curve EdgeLeftBehind { get; set; }
        public Curve EdgeRightFront { get; set; }
        public Curve EdgeRightBehind { get; set; }


        //Inputs

        //Stage_01
        public double SeparationDistance { get; set; }
        public double NumberOfEdges { get; set; }
        public double NonVerticalLineAngle { get; set; }


        //Stage_02
        public double ExtendedCurveLength { get; set; }
        public double LargerCurveLength { get; set; }


        
        //Stage_03
        public double ErasureRegionAreaProportion { get; set; }
        public double AverageDistanceBetweenStripes { get; set; }




        //Output






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

        public void UpdateSeparationDistance(double newDistance)
        {
            if (newDistance > 30)
            {
                newDistance = 30;
            }

            SeparationDistance = newDistance;
        }



        public void UpdateNumberOfEdges ( double newNumberOfEdges)
        {
          
            NumberOfEdges = newNumberOfEdges;
        }


        
        public void UpdateNonVerticalLines (double newNonVerticalAngle)
        {
            NonVerticalLineAngle = newNonVerticalAngle;
        }



        public void UpdateExtendedCurveLength (double NewExtendedLength)
        {
            ExtendedCurveLength = NewExtendedLength;
        }



        public void UpdateLargeCurveLength (double NewLargeCurveLength)
        {
            LargerCurveLength = NewLargeCurveLength;
        }



        public void UpdateErasureProportion (double newErasureProportion)
        {


            ErasureRegionAreaProportion = newErasureProportion;
        }



        public void UpdateAverageDistanceBetweenStripes (double newAverageDistance)
        {
            AverageDistanceBetweenStripes = newAverageDistance;
        }



    }
}
