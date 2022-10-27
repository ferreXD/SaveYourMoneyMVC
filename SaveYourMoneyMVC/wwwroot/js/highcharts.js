function createAndAddLineChart(dictionary, months) {
    let meses = months.Months;
    let setupMonths = true;

    let mesesIntervalo = [];
    let datos = [];

    for (var name in dictionary.Dictionary) {
        let data = [];

        $.each(dictionary.Dictionary[name], function (i, e) {
            data.push(e.Value);

            if (setupMonths)
                mesesIntervalo.push(meses[e.Month - 1]);
        });

        setupMonths = false;

        datos.push({ name: name, data: data });
    }

    // Configure and put the chart in the Html document
    Highcharts.chart('line_container', {
        title: {
            text: ''
        },


        yAxis: {
            title: {
                text: 'Euros'
            }
        },

        xAxis: {
            categories: mesesIntervalo

        },

        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },

        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                }
            }
        },

        series: datos,

        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }

    });
}

function createAndAddPieChart(dictionary) {
    let datos = [];

    for (var name in dictionary.Dictionary) {
        let value = 0;

        $.each(dictionary.Dictionary[name], function (i, e) {
            value += e.Value;
        });

        datos.push({ name: name, y: value });
    }

    Highcharts.chart('pie_container', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y}€</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            name: "Gastos",
            colorByPoint: true,
            data: datos
        }]
    });
}