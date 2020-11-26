using System;

namespace VehicleManagementApi.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string EngineNo { get; set; }
        public string VinNo { get; set; }
        public string RegNo { get; set; }
        public DateTime IssueDate { get; set; }
        public bool Financed { get; set; }
    }
}
