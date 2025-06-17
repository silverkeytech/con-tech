using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConTech.Migration;

public static class ObjectStatus
{
    public const int Active = 1;

    public const int Disabled = 0;
}

public static class Gender
{
    public const string Male = "M";

    public const string Female = "F";
}

public enum EntityType
{
    PolyLine = 100,
    LwPolyline = 200,
    Point = 300,
    MText = 400,
    Circle = 500,
    Line = 600
}
