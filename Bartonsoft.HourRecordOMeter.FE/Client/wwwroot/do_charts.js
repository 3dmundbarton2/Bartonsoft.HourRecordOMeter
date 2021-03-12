function generateLineChart(value, label) {
    var options = {
        series: [
            {
                name: "Air Density",
                data: value
            }
        ],
        chart: {
            height: '100%',
            width: '100%',
            type: 'line',
            toolbar: {
                show: false
            }
        },
        colors:
            [
                function ({ value, seriesIndex, w }) {
                    if (value > 1.1) {
                        return '#FF0000'
                    }
                    else if (value <= 1.1 && value > 1.0) {
                        return '#ffa500'
                    }
                    else if (value <= 1.0 && value > 0.8) {
                        return '#00FF00'
                    }
                    else {
                        return '#000000'
                    }
                }
            ],
        dataLabels: {
            enabled: true            
        },
        stroke: {
            width: 1
        },
        title: {
            text: 'Air Density',
            align: 'left'
        },
        markers: {
            size: 1            
        },
        xaxis: {
            categories: label,
            title: {
                text: 'Date'
            }
        },
        yaxis: {
            title: {
                text: 'Air Density'
            },
            min: 0.9,
            max: 1.29,
        }
    };

    //follow the same tag id for your visualization
    var chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();
}