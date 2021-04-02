using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class TemperatureRange
    {
        [Required]
        [Timestamp]
        public DateTime? Start {get; set;}

        [Required]
        [Timestamp]
        public DateTime? End {get; set;}
    }

    public class TemperatureMeasurement
    {
        [Required]
        public TemperatureRange Time { get; set; }

        [Required]
        [Range(-50.0, 50.0)]
        public double? Max { get; set; }

        [Required]
        [Range(-50.0, 50.0)]
        public double? Min { get; set; }

        [Required]
        [Range(-50.0, 50.0)]
        public double? Avg { get; set; }

        public override string ToString()
        {
            return $"[{Time.Start.Value.ToString("o")}-{Time.End.Value.ToString("o")}] max: {Max} min: {Min} avg: {Avg}";
        }
    }
}
