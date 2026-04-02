using IPC2_Proyecto2_202401939.Domain;
using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.Sistemas;

public class IndexModel : PageModel
{
    private readonly AppStateService _appState;

    public IndexModel(AppStateService appState)
    {
        _appState = appState;
        Sistemas = [];
    }

    public SistemaDrones[] Sistemas { get; private set; }

    // Obtiene los sistemas de drones para desplegarlos en pantalla.
    public void OnGet()
    {
        Sistemas = _appState.ObtenerSistemas();
    }
}
