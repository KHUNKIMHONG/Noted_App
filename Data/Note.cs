using System.ComponentModel.DataAnnotations;

namespace API_BackEnd.Data
{
    public class Note
    {
        [Key]
        public int Id { get; set; }  // Add this as your primary key
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public int Likes { get; set; }

    }
}
