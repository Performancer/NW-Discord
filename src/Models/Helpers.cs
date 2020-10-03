using System;

namespace NW.Models
{
    public class Vector3
    {
        public Vector3()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}