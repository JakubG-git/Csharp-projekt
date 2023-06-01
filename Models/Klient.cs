using System.ComponentModel.DataAnnotations;

namespace TMS.Models;

public class Klient
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Nazwa klienta")]
    public string Nazwa { get; set; }
    [Display(Name = "Adres")]
    public string? Adres { get; set; }
    [Display(Name = "NIP")]
    public string? NIP { get; set; }
    [Display(Name = "Telefon")]
    public string? Telefon { get; set; }
    [Display(Name = "Email")]
    public string? Email { get; set; }
}