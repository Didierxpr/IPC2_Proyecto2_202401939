using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.Ayuda;


/// Página de ayuda e información del proyecto.
/// Muestra datos y acceso a documentación.

public class IndexModel : PageModel
{
    public string NombreEstudiante { get; set; } = "Carlos Didiere Cabrera Rodriguez";
    public string Carnet { get; set; } = "202401939";
    public string Curso { get; set; } = "Introducción a la Programación y Computación 2 (IPC2)";
    public string Seccion { get; set; } = "P";
    public string Ciclo { get; set; } = "Primer Semestre 2026";
    public string Instructor { get; set; } = "Aux. Carlos Manuel Lima y Lima";
    public string Universidad { get; set; } = "Universidad de San Carlos de Guatemala";
    public string Facultad { get; set; } = "Facultad de Ingeniería";

    
    /// Carga la pagina de ayuda.
    
    public void OnGet()
    {
    }
}
