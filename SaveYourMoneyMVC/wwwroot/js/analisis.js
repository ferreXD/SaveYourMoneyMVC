var initAnalysisView = function (dictionary, months) {
    var cleanFilters = $("#clean_filters");

    if (Object.keys(dictionary.Dictionary).length > 0) {
        createAndAddLineChart(dictionary, months);
        createAndAddPieChart(dictionary);
    }

    var limpiarFiltros = function (e) {
        e.preventDefault();

        document.getElementById("Tipo").value = -1;
        document.getElementById("IntervaloInicio").value = null;
        document.getElementById("IntervaloFin").value = null;
    }

    $(cleanFilters).on("click", function (event) {
        limpiarFiltros(event);
    })
}