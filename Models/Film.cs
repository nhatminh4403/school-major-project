﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class Film
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã phim")]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? PosterUrl { get; set; }

        public string? TrailerUrl { get; set; }
        [Required]
        public string? DirectorName { get; set; }
        [Required]
        public string? Language { get; set; }
        [Required]
        public string? FilmRated { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Thời lượng phim phải là số dương")]
        public int FilmDuration { get; set; }
        [Required]
        public string? Actors { get; set; }
        [Required]
        public string Quality { get; set; }

        [Required]
        [DisplayName("Giờ chiếu")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required]
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country? Country { get; set; }

        public ICollection<Rating>? Rating { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
        public ICollection<Category>? Categories { get; set; }
    }
}
