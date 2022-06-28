var colors = new Array('#E25050', '#71B3D0', '#9ED4AE', '#393A43', '#00B0F6', '#F8766D');

var DataSource = new Array();

function ConstructLeadSeries(series, group, index) {

    var seriesOutput = new Object();
    seriesOutput.name = series.name;
    seriesOutput.color = series.color;
    seriesOutput.type = series.type;

    if (seriesOutput.color == "auto")
        seriesOutput.color = colors[index];
    if (seriesOutput.type == "auto")
        seriesOutput.type = "line";

    var groupedData = new Array();

    var groupedHours = 1000 * 60 * 60 * group;

    var i = 0;
    if (!series.originalData)
        series.originalData = series.data;
    var data = series.originalData;
    while (i < data.length) {
        var record = new Array();

        var startTime = data[i][0];
        var endTime = startTime + groupedHours;
        record.push(data[i][0]);
        var result = 0;
        while (i < data.length && data[i][0] < endTime) {
            result += data[i][1];            
            i++;
        }

        record.push(result);
        groupedData.push(record);

    }
    
    seriesOutput.data = groupedData;
    return seriesOutput;
}



function SumulationGetLeadsSeries(name, color, changer, group) {
    var series = new Object();
    series.name = name;
    series.type = "line";
    series.color = color;
    var data = new Array();
    var date = new Date(2020, 1, 1, 0, 0, 0);
    var hour = 1000 * 60 * 60;

    var currentTime = date.getTime();

    for (var i = 0; i < 1000 / group; i++) {
        var a = Math.sin(i / 180 * Math.PI / changer * 2) * 1000 + changer;
        var leadsPerHour = Math.round(Math.abs(getRandomInt(a, a)));
        var record = new Array();
        record.push(currentTime);
        record.push(leadsPerHour);
        data.push(record);
        currentTime += hour * group;
    }

    series.data = data;
    return series;
}


var barchartSaved = null;

function InitStockChart(seriesSource) {



    var groupingUnits = [[
        'millisecond', // unit name
        [1, 2, 5, 10, 20, 25, 50, 100, 200, 500] // allowed multiples
    ], [
        'second',
        [1, 2, 5, 10, 15, 30]
    ], [
        'minute',
        [1, 2, 5, 10, 15, 30]
    ], [
        'hour',
        [1, 2, 3, 4, 6, 8, 12]
    ], [
        'day',
        [1]
    ], [
        'week',
        [1]
    ], [
        'month',
        [1, 3, 6]
    ], [
        'year',
        null
    ]];    

    i = 0;
    var seriesArray = new Array();

    for (var i = 0; i < seriesSource.length; i++) {
        var serie=ConstructLeadSeries(seriesSource[i], 1, i);
        serie.type = "column";
        serie.dataGrouping = {
            units: groupingUnits
        };
        seriesArray.push(serie);
    }


    // create the chart
    Highcharts.stockChart('containerLeadsStock', {
        credits: {
            enabled: false
        },
        rangeSelector:
        {
            allButtonsEnabled: true,
            buttons: [{
                type: 'month',
                count: 3,
                text: 'Day',
                dataGrouping: {
                    forced: true,
                    units: [['day', [1]]]
                }
            }, {
                type: 'year',
                count: 1,
                text: 'Week',
                dataGrouping: {
                    forced: true,
                    units: [['week', [1]]]
                }
            }, {
                type: 'all',
                text: 'Month',
                dataGrouping: {
                    forced: true,
                    units: [['month', [1]]]
                }
            }],
            buttonTheme: {
                width: 60
            },
            selected: 2

        },

        title: {
            text: 'Leads Distribution'
        },

        yAxis: [{
            opposite: false,
            labels: {
                align: 'left',
                x: 3
            },
            title: {
                text: 'Leads'
            },
            height: '100%',
            lineWidth: 2,
            resize: {
                enabled: true
            }
        }],

        tooltip: {
            split: true
        },

        series: seriesArray       
    });
}

var GlobalSeriesSource = null;

function InitLeadsGraph(seriesSource, group)
{

    var seriesArray = new Array();

    if (seriesSource) {
        GlobalSeriesSource = seriesSource;
    }
    else
        seriesSource = GlobalSeriesSource;


    for (var i = 0; i < seriesSource.length; i++) {
        var serie=ConstructLeadSeries(seriesSource[i], group, i);  //group by hours
        seriesArray.push(serie);
    }

    var xTitle = 'Timeline';
    var yTitle = 'Leads';
    
    Highcharts.chart("containerLeads", {
        credits: {
            enabled: false
        },
        chart: {
            zoomType: 'x'
        },
        title: {
            text: 'Leads Distribution by Timeline'
        },

        subtitle: {
            text: ''
        },

        yAxis: {
            title: {
                text: yTitle
            }
            /*plotLines: [{
                value: 500,
                color: 'red',
                dashStyle: 'solid',
                width: 2,
                label: {
                    text: 'Average'
                }
            }]*/
        },

        tooltip: {
            formatter: function () {
                var name = seriesArray[this.series.index].name;
                //var count = 0;//seriesArray[this.series.index].dataCount[this.point.index];
                var value = this.point.y;
                return '<b>Category:</b> ' + name +
                    //  '<br>' + count + " motors under risk" +
                    '<br>Value:' + value;
            }
        },

        xAxis: {
            units: [[
                'millisecond', // unit name
                [1, 2, 5, 10, 20, 25, 50, 100, 200, 500] // allowed multiples
            ], [
                'second',
                [1, 2, 5, 10, 15, 30]
            ], [
                'minute',
                [1, 2, 5, 10, 15, 30]
            ], [
                'hour',
                [1, 2, 3, 4, 6, 8, 12]
            ], [
                'day',
                [1]
            ], [
                'week',
                [1]
            ], [
                'month',
                [1, 3, 6]
            ], [
                'year',
                null
            ]],
            type: "datetime",
            gridLineWidth: 1,
            title: {
                text: xTitle,
            }
        },

        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom'
        },

        plotOptions: {

            series: {
                dataGrouping: {
                    forced: true,
                    enabled: true,
                    groupAll: true,
                    units: [[
                        'millisecond', // unit name
                        [1, 2, 5, 10, 20, 25, 50, 100, 200, 500] // allowed multiples
                    ], [
                        'second',
                        [1, 2, 5, 10, 15, 30]
                    ], [
                        'minute',
                        [1, 2, 5, 10, 15, 30]
                    ], [
                        'hour',
                        [1, 2, 3, 4, 6, 8, 12]
                    ], [
                        'day',
                        [1]
                    ], [
                        'week',
                        [1]
                    ], [
                        'month',
                        [1, 3, 6]
                    ], [
                        'year',
                        null
                    ]]
                },
                label: {
                    connectorAllowed: false
                },
                pointStart: 2010
            }
        },

        series: seriesArray,

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


////////// 

function InitMapContainer() {
    Highcharts.getJSON('https://cdn.jsdelivr.net/gh/highcharts/highcharts@v7.0.0/samples/data/us-population-density.json', function (data) {

        // Make codes uppercase to match the map data
        data.forEach(function (p) {
            p.code = p.code.toUpperCase();
        });

        // Instantiate the map
        Highcharts.mapChart('containerMap', {
            credits: {
                enabled: false
            },
            chart: {
                map: 'countries/us/us-all',
                borderWidth: 1
            },

            title: {
                text: 'Leads distribution in US'
            },

            exporting: {
                sourceWidth: 600,
                sourceHeight: 500
            },

            legend: {
                layout: 'horizontal',
                borderWidth: 0,
                backgroundColor: 'rgba(255,255,255,0.85)',
                floating: true,
                verticalAlign: 'top',
                y: 25
            },

            mapNavigation: {
                enabled: true
            },

            colorAxis: {
                min: 1,
                type: 'logarithmic',
                minColor: 'red',
                maxColor: 'blue',
                stops: [
                    [0, '#EFEFFF'],
                    [0.67, '#4444FF'],
                    [1, '#220000']
                ]
            },

            series: [{
                animation: {
                    duration: 1000
                },
                color: "red",
                data: data,
                joinBy: ['postal-code', 'code'],
                dataLabels: {
                    enabled: true,
                    color: '#FFFFFF',
                    format: '{point.code}'
                },
                name: 'Leads distribution',
                tooltip: {
                    pointFormat: 'Leads {point.value}'
                }
            }]
        });
    });
}

function Init3DPieLeadComparision(summary) {


    var seriesArray = new Array();

    var serie1 = new Object();
    serie1.name = "Recieved/Sold";
    serie1.type = "pie";    
    serie1.data = new Array();
    serie1.data.push(["Recieved", summary.Recieved]);
    serie1.data.push(["Sold", summary.Sold]);
    serie1.center = ["50%", 80];
    serie1.size = 200;

    var serie2 = new Object();
    serie2.type = "pie";
    serie2.name = "Cost/Profit";
    serie2.data = new Array();
    serie2.data.push(["Cost", summary.Cost]);
    serie2.data.push(["Profit", summary.Profit]);
    serie2.center = ["50%", 280];
    serie2.size = 200;

    seriesArray.push(serie1);
    seriesArray.push(serie2);

    Highcharts.chart('containerLeadComparision', {
        credits:
        {
            enabled: false
        },
        chart: {
            type: 'pie',
            options3d: {
                enabled: true,
                alpha: 45,
                beta: 0
            }
        },
        title: {
            text: 'Comparision'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                depth: 35,
                colors: pieColors,
                dataLabels: {
                    enabled: true,
                    format: '{point.name}'
                }
            }
        },
        series: seriesArray
    });

}

var pieColors = (function () {
    var colors = [],
        base = Highcharts.getOptions().colors[0],
        i;

    for (i = 0; i < 10; i += 1) {
        // Start out with a darkened base color (negative brighten), and end
        // up with a much brighter color
        colors.push(Highcharts.color(base).brighten((i - 3) / 7).get());
    }
    return colors;
}());


function GetPieSeriesArray(seriesSource,parameter) {
    var seriesArray = new Array();

    for (var i = 0; i < seriesSource.length; i++) {
        var serie = seriesSource[i];
        if (parameter != "All")
            if (serie.name != parameter) continue;
        if (serie.color == "auto")
            serie.color = colors[i];
        if (serie.type == "auto")
            serie.type = "pie";
        seriesArray.push(serie);
    }

    var maxIndex = 0;
    var max = -1;
    for (var i = 0; i < seriesArray[0].data.length; i++) {
        if (max < seriesArray[0].data[i][1]) {
            max = seriesArray[0].data[i][1];
            maxIndex = i;
        }
    }
    var data = seriesArray[0].data[maxIndex];

    seriesArray[0].data[maxIndex] = new Object();
    seriesArray[0].data[maxIndex].sliced = true;
    seriesArray[0].data[maxIndex].name = data[0];
    seriesArray[0].data[maxIndex].y = data[1];

    return seriesArray;

}
function Init3DPieStates(seriesSource,parameter)
{
    
    Highcharts.chart('containerTopStates', {
        credits:
        {
            enabled: false
        },
        chart: {
            type: 'pie',
            options3d: {
                enabled: true,
                alpha: 45,
                beta: 0
            }
        },
        title: {
            text: 'Top States'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                depth: 35,
                colors: pieColors,
                dataLabels: {
                    enabled: true,
                    format: '{point.name}'
                }
            }
        },
        series: GetPieSeriesArray(seriesSource, parameter)
    });

}