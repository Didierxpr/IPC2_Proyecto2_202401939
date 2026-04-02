using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.Xml;

public class CargarModel : PageModel
{
    private readonly XmlInputService _xmlInputService;

    public CargarModel(XmlInputService xmlInputService)
    {
        _xmlInputService = xmlInputService;
        Resumen = string.Empty;
        Errores = string.Empty;
    }

    public string Resumen { get; private set; }

    public string Errores { get; private set; }

    public bool EsExito { get; private set; }

    // Carga la vista de carga XML.
    public void OnGet()
    {
    }

    // Procesa el archivo XML subido por el usuario y realiza la carga incremental.
    public IActionResult OnPost(IFormFile? archivoXml)
    {
        if (archivoXml == null || archivoXml.Length == 0)
        {
            EsExito = false;
            Resumen = "Debes seleccionar un archivo XML valido.";
            Errores = string.Empty;
            return Page();
        }

        using Stream stream = archivoXml.OpenReadStream();
        XmlInputLoadResult resultado = _xmlInputService.CargarXml(stream);
        EsExito = resultado.Exito;
        Resumen = resultado.Resumen;
        Errores = resultado.Errores;
        return Page();
    }
}
