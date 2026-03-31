using IPC2_Proyecto2_202401939.Domain;
using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.Drones;

public class IndexModel : PageModel
{
    private readonly AppStateService _appState;

    public IndexModel(AppStateService appState)
    {
        _appState = appState;
        Drones = [];
    }

    public Dron[] Drones { get; private set; }

    // Obtiene drones para mostrarlos en la tabla de la vista.
    public void OnGet()
    {
        Drones = _appState.ObtenerDrones();
    }
}
