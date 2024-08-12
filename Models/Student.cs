using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;

namespace studentAPI.Models;

public class Student:ITableEntity
{
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; } 

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string PhotoPath { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public ETag ETag { get; set; } = ETag.All;
    public DateTimeOffset? Timestamp { get ; set ; }
}
