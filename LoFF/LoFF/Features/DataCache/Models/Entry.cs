using System;
using System.ComponentModel.DataAnnotations;

namespace LoFF.Features.DataCache.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        [MaxLength(2 << 15)]
        public string Key { get; set; }
        [MaxLength(2 << 20)]
        public string JsonValue { get; set; }

    }
}
