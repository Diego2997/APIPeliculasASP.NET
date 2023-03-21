﻿using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;  
    }
}
