function generateLineChart(value, label) {
    var options = {
        series: [
            {
                name: "Air Density",
                data: value
            }
        ],
        chart: {
            height: '400px',
            width: '100%',
            type: 'line',
            toolbar: {
                show: false
            }
        },
        colors:
            [
                function ({ value, seriesIndex, w }) {

                    var upperVal = 1.2
                    var lowerVal = 1.0
                    var valueFraction = (value - lowerVal) / (upperVal - lowerVal)

                    if (valueFraction >= 0.9) {
                        return '#b42b2b'
                    }
                    else if (valueFraction < 0.9 && valueFraction >= 0.8) {
                        return '#b83f1b'
                    }
                    else if (valueFraction < 0.8 && valueFraction >= 0.7) {
                        return '#b95300'
                    }
                    else if (valueFraction < 0.7 && valueFraction >= 0.6) {
                        return '#b56700'
                    }
                    else if (valueFraction < 0.6 && valueFraction >= 0.5) {
                        return '#ae7900'
                    }
                    else if (valueFraction < 0.5 && valueFraction >= 0.4) {
                        return '#a28c00'
                    }
                    else if (valueFraction < 0.4 && valueFraction >= 0.3) {
                        return '#939d00'
                    }
                    else if (valueFraction < 0.3 && valueFraction >= 0.2) {
                        return '#7ead00'
                    }
                    else if (valueFraction < 0.2 && valueFraction >= 0.1) {
                        return '#61bd24'
                    }
                    else if (valueFraction <= 0.1 && valueFraction >= -0.5) {
                        return '#28cc4b'
                    }
                    else {
                        return '#000000'
                    }
                }
            ],
        dataLabels: {
            enabled: true,
            style: {
                fontSize: '12px'
            },
            offsetX: 0,
            offsetY: -10,
        },
        stroke: {
            width: 1
        },
        title: {
            text: 'Air Density',
            align: 'left'
        },
        markers: {
            size: 1,     
            color: ['#000000']
        },
        xaxis: {
            categories: label,
            title: {
                text: 'Date'
            }
        },
        yaxis: {
            min: 0.9,
            max: 1.29,
        }
    };

    //follow the same tag id for your visualization
    var chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();
}