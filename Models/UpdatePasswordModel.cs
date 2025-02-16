namespace API_BackEnd.Models
{
    public class UpdatePasswordModel
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
