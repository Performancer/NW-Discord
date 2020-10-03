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
        public int X;
        public int Y;
        public int Z;
    }
}