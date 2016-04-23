namespace _30sep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        public int ReviewId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public int? Rating { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public int BookId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual Book Book { get; set; }
    }
}
