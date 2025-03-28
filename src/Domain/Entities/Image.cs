﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } = null!;
    }
}
