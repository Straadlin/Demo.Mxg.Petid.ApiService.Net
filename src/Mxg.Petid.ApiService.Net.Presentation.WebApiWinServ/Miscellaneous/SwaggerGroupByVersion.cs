// =======================================================================================
// Description: Defines the SwaggerGroupByVersion class.
// Author:      Alfredo Estrada
// Date:        2025-01-04
// Version:     1.0.0
// =======================================================================================

using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Miscellaneous;

/// <summary>
/// Grouping controllers by version.
/// </summary>
public class SwaggerGroupByVersion : IControllerModelConvention
{
    /// <summary>
    /// Apply the version to the controller.
    /// </summary>
    /// <param name="controller">Controller object.</param>
    public void Apply(ControllerModel controller)
    {
        var namespaceController = controller.ControllerType.Namespace; // Controller.V1
        var versionAPI = namespaceController.Split('.').Last().ToLower(); // v1
        controller.ApiExplorer.GroupName = versionAPI;
    }
}