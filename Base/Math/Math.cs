using Godot;
using System;

namespace MathThreeDLib
{
    class MathTDL
    {
        public static double BtoMB(double bytes)
        {
            return bytes / (1024.0 * 1024.0);
        }
    }
}