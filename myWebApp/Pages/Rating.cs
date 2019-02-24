using System;
using System.ComponentModel.DataAnnotations;

namespace myWebApp
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; } 
        public double Rate { get; set; }
        public string UserName { get; set; }
    }
}