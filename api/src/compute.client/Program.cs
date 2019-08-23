﻿using System;

namespace computegen
{
    class Program
    {

        static void Main(string[] args)
        {
            const string rhinocommonPath = @"C:\dev\github\mcneel\rhino\src4\DotNetSDK\rhinocommon\dotnet";
            Console.WriteLine("[BEGIN PARSE]");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            ClassBuilder.BuildClassDictionary(rhinocommonPath);
            Console.ResetColor();
            Console.WriteLine("[END PARSE]");

            // just do a small number of classes to get started
            string[] filter = new string[] {
                ".AreaMassProperties",
                ".BezierCurve",
                ".Brep", ".BrepFace",
                ".Curve", ".Extrusion", ".Intersection", ".Mesh",
                ".NurbsCurve", ".NurbsSurface", ".Surface",
                ".VolumeMassProperties"
            };

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Writing javascript client");
            var js = new JavascriptClient();
            js.Write(ClassBuilder.AllClasses, "compute.rhino3d.js", filter);
            Console.WriteLine("Writing python client");
            var py = new PythonClient();
            py.Write(ClassBuilder.AllClasses, "", filter);
            Console.WriteLine("Writing C# client");
            var cs = new DotNetClient();
            cs.Write(ClassBuilder.AllClasses, "RhinoCompute.cs", filter);

            var classes = ClassBuilder.FilteredList(ClassBuilder.AllClasses, filter);
            Console.WriteLine("Writing python docs");
            RstClient.WritePythonDocs(classes);
            Console.WriteLine("Writing javascript docs");
            RstClient.WriteJavascriptDocs(classes);


            Console.ResetColor();
        }
    }
}
