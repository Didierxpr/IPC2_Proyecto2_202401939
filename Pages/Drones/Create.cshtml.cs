using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.Drones;

public class CreateModel : PageModel
{
    private readonly AppStateService _appState;

    public CreateModel(AppStateService appState)
    {
        _appState = appState;
        Mensaje = string.Empty;
        NombreDron = string.Empty;
    }

    [BindProperty]
    public string NombreDron { get; set; }

    public string Mensaje { get; private set; }

    public bool EsExito { get; private set; }

    // Carga la vista de creacion sin hacer cambios de estado.
    public void OnGet()
    {
    }

    // Intenta registrar un nuevo dron y muestra el resultado de validacion.
    public void OnPost()
    {
        EsExito = _appState.AgregarDron(NombreDron, out string mensajeResultado);
        Mensaje = mensajeResultado;
    }
}
