using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages;

public class IndexModel : PageModel
{
    private readonly AppStateService _appState;

    public IndexModel(AppStateService appState)
    {
        _appState = appState;
        Mensaje = string.Empty;
    }

    public string Mensaje { get; private set; }

    // Carga la vista principal sin modificar estado.
    public void OnGet()
    {
    }

    // Reinicia toda la informacion en memoria del sistema.
    public void OnPost()
    {
        _appState.Inicializar();
        Mensaje = "Sistema inicializado correctamente.";
    }
}
