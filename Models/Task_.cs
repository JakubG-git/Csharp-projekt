using System.ComponentModel.DataAnnotations;

namespace TMS.Models;

public class Task_
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Tytuł")]
    public string Title { get; set; }
    [Display(Name = "Priorytet")]
    public PIORITY Piority { get; set; }
    [Display(Name = "Status")]
    public STATUS Status { get; set; }
    [Display(Name = "Zadania powiązane")]
    [DisplayFormat(NullDisplayText = "Brak")]
    public ICollection<Task_>? RelatedTasks { get; set; }
    [Display(Name = "Klient przypisany")]
    [DisplayFormat(NullDisplayText = "Lokalne zadanie")]
    public Klient? Klient { get; set; }
    [Display(Name = "Pracownik przypisany")]
    [DisplayFormat(NullDisplayText = "Brak")]
    public string? Pracownik { get; set; }
    [Display(Name = "Opis")]
    [DisplayFormat(NullDisplayText = "Brak")]
    public Description? Description { get; set; }
    [Display(Name = "Komentarze")]
    [DisplayFormat(NullDisplayText = "Brak")]
    public ICollection<Comments>? Comments { get; set; }
    [Display(Name = "Data utworzenia")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; }
    [Display(Name = "Deadline")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? Deadline { get; set; }
}
public enum PIORITY
{
    LOW,
    MEDIUM,
    HIGH
}

public enum STATUS
{
    NEW,
    IN_PROGRESS,
    DONE
}