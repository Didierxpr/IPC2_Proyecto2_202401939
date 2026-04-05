using IPC2_Proyecto2_202401939.Domain;
using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.Sistemas;

public class IndexModel : PageModel
{
    private readonly AppStateService _appState;
    private readonly GeneradorGraphviz _generador;

    public IndexModel(AppStateService appState, GeneradorGraphviz generador)
    {
        _appState = appState;
        _generador = generador;
        Sistemas = [];
    }

    public SistemaDrones[] Sistemas { get; private set; }
    public Dictionary<string, string> SvgGraficasSistema { get; private set; } = new();

    public void OnGet()
    {
        Sistemas = _appState.ObtenerSistemas();
        
        // Generar SVG para cada sistema
        foreach (var sistema in Sistemas)
        {
            string svg = _generador.GenerarSVGSistema(sistema);
            SvgGraficasSistema[sistema.Nombre] = svg;
        }
    }

    public string GenerarTablaHTML(SistemaDrones sistema)
    {
        return _generador.GenerarTablaHTMLSistema(sistema);
    }
}
