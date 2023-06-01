using System.ComponentModel.DataAnnotations;

namespace TMS.Models;

public class Description
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Opis")]
    public string Opis { get; set; }
}