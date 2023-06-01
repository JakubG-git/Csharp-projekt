using System.ComponentModel.DataAnnotations;

namespace TMS.Models;

public class Comments
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Komentarz")]
    public string Komentarz { get; set; }
}