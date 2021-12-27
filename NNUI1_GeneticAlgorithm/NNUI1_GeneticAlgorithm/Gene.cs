using LinqToExcel.Attributes;
using System;

namespace NNUI1_GeneticAlgorithm
{
    public class Gene
    {
        [ExcelColumn("Id")]
        public int Id { get; set; }

        [ExcelColumn("Name")]
        public string Name { get; set; }

        [ExcelColumn("Latitude")]
        public double Latitude { get; set; }

        [ExcelColumn("Longitude")]
        public double Longitude { get; set; }
        public double CountDistance(Gene gene)
        {
            return Constants.EarthRadius * Math.Acos(Math.Sin(ToRadians(Latitude)) * Math.Sin(ToRadians(gene.Latitude)) + Math.Cos(ToRadians(Latitude)) * Math.Cos(ToRadians(gene.Latitude)) * Math.Cos(ToRadians(Longitude) - ToRadians(gene.Longitude)));
        }
        public override string ToString()
        {
            return "{" + Id + ", " + Name + ", [" + Latitude + ", " + Longitude + "]}";
        }
        private double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
