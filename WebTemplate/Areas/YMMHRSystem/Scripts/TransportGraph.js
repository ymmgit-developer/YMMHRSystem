var servicesByTypeMonthlyChart;
var servicesByTypeAccumulatedChart;
var servicesByProcessChart;
var totalTransportCostMonthlyChart;
var totalTransportCostAccumulatedChart;
var extraTransportCostByProcessChart;



function GenerateGraph() {

    $.ajax({
        type: "POST",
        url: $LoadTransportGraphs,
        success: function (chData) {
            var aData = chData;
            var aLabels1 = aData[0];
            var aDatasets1 = aData[1];
            var aDatasets2 = aData[2];
            var aLabels2 = aData[3];
            var aDatasets3 = aData[4];
            var aLabels3 = aData[5];
            var aDatasets4 = aData[6];
            var aLabels4 = aData[7];
            var aDatasets5 = aData[8];
            var aDatasets6 = aData[9];
            var aLabels5 = aData[10];
            var aDatasets7 = aData[11];
            var aLabels6 = aData[12];
            var aDatasets8 = aData[13];

            var config1 = {
                type: 'bar',
                data: {
                    labels: aLabels1,
                    datasets: [
                        {
                            data: aDatasets1,
                            label: "Normal",
                            backgroundColor: "rgb(54, 162, 235)",
                        },
                        {
                            data: aDatasets2,
                            label: "Extra",
                            backgroundColor: "rgb(255, 99, 132)",

                        }
                    ]

                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
                    },
                    plugins: {
                        datalabels: {
                            anchor: 'end',
                            align: 'end',
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                precision: 0
                            },
                            afterDataLimits(scale) {
                                scale.max += 1;
                            }
                        }],
                    }
                }
            };
            var config2 = {
                type: 'doughnut',
                data: {
                    datasets: [{
                        data: aDatasets3,
                        backgroundColor:
                            [
                                "rgb(54, 162, 235)",
                                "rgba(255, 99, 132)",
                            ]
                        ,
                        label: ''
                    }],
                    labels: aLabels2
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
                    },
                    animation: {
                        animateScale: true,
                        animateRotate: true
                    }
                }
            };
            var config3 = {
                type: 'bar',
                data: {
                    labels: aLabels3,
                    datasets: [
                        {
                            data: aDatasets4,
                            label: "No. of Transports",
                            backgroundColor: "rgb(153, 102, 255)",
                        }
                    ]

                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
                    },
                    plugins: {
                        datalabels: {
                            anchor: 'end',
                            align: 'end',
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                precision: 0
                            },
                            afterDataLimits(scale) {
                                scale.max += 1;
                            }
                        }],
                    }
                }
            };
            var config4 = {
                type: 'line',
                data: {
                    labels: aLabels4,
                    datasets: [
                        {
                            data: aDatasets5,
                            fill: false,
                            lineTension: 0,
                            label: "Normal",
                            borderColor: "rgb(255, 159, 64)",
                        },
                        {
                            data: aDatasets6,
                            fill: false,
                            lineTension: 0,
                            label: "Extra",
                            borderColor: "rgb(255, 99, 132)",

                        }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
                    },
                    plugins: {
                        datalabels: {
                            anchor: 'end',
                            align: 'end',
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                precision: 0
                            },
                            afterDataLimits(scale) {
                                scale.max += 1000;
                            }
                        }],
                    }
                }
            };
            var config5 = {
                type: 'doughnut',
                data: {
                    datasets: [{
                        data: aDatasets7,
                        backgroundColor:
                            [
                                "rgb(255, 159, 64)",
                                "rgb(255, 99, 132)",
                            ]
                        ,
                        label: ''
                    }],
                    labels: aLabels5
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
                    },
                    animation: {
                        animateScale: true,
                        animateRotate: true
                    }
                }
            };
            var config6 = {
                type: 'bar',
                data: {
                    labels: aLabels6,
                    datasets: [
                        {
                            data: aDatasets8,
                            label: "Cost",
                            backgroundColor: "rgb(255, 205, 86)",
                        }
                    ]

                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
                    },
                    plugins: {
                        datalabels: {
                            anchor: 'end',
                            align: 'end',
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                precision: 0
                            },
                            afterDataLimits(scale) {
                                scale.max += 1000;
                            }
                        }],
                    }
                }
            };

            //For ServicesByTypeMonthly Chart
            var ctx = document.getElementById("ServicesByTypeMonthly").getContext('2d');
            servicesByTypeMonthlyChart = new Chart(ctx, config1);

            //For ServicesByTypeAccumulated Chart
            var ctx2 = document.getElementById("ServicesByTypeAccumulated").getContext('2d');
            servicesByTypeAccumulatedChart = new Chart(ctx2, config2);

            //For ServicesByTypeAccumulated Chart
            var ctx3 = document.getElementById("ServicesByProcess").getContext('2d');
            servicesByProcessChart = new Chart(ctx3, config3);

            //For TotalTransportCostMonthly Chart
            var ctx4 = document.getElementById("TotalTransportCostMonthly").getContext('2d');
            totalTransportCostMonthlyChart = new Chart(ctx4, config4);

            //For TotalTransportCostAccumulated Chart
            var ctx5 = document.getElementById("TotalTransportCostAccumulated").getContext('2d');
            totalTransportCostAccumulatedChart = new Chart(ctx5, config5);

            //For TotalTransportCostAccumulated Chart
            var ctx6 = document.getElementById("ExtraTransportCostByProcess").getContext('2d');
            extraTransportCostByProcessChart = new Chart(ctx6, config6);
        },
        "error": function (data) {
        }

    });
}

function RefreshServicesByType() {
    var firstHalfKi = $("#ServicesByTypeYear option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshServicesByType,
        data: { firstHalfKi: firstHalfKi },
        success: function (chData) {

            var aData = chData;
            var aDatasets1 = aData[0];
            var aDatasets2 = aData[1];
            var aDatasets3 = aData[2];


            servicesByTypeMonthlyChart.data.datasets[0].data = aDatasets1;
            servicesByTypeMonthlyChart.data.datasets[1].data = aDatasets2;
            servicesByTypeMonthlyChart.update();

            servicesByTypeAccumulatedChart.data.datasets[0].data = aDatasets3;
            servicesByTypeAccumulatedChart.update();

        }
    });
}

function RefreshServicesByProcess() {
    var firstHalfKi = $("#ServicesByProcessYear option:selected").val();
    var month = $("#ServicesByProcessMonth option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshServicesByProcess,
        data: { firstHalfKi: firstHalfKi, month: month},
        success: function (chData) {

            var aData = chData;
            var aDatasets1 = aData[0];

            servicesByProcessChart.data.datasets[0].data = aDatasets1;
            servicesByProcessChart.update();

        }
    });
}

function RefreshExtraTransportCostByProcess() {
    var firstHalfKi = $("#ExtraTransportCostByProcessYear option:selected").val();
    var month = $("#ExtraTransportCostByProcessMonth option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshExtraTransportCostByProcess,
        data: { firstHalfKi: firstHalfKi, month: month },
        success: function (chData) {

            var aData = chData;
            var aDatasets1 = aData[0];

            extraTransportCostByProcessChart.data.datasets[0].data = aDatasets1;
            extraTransportCostByProcessChart.update();

        }
    });
}

function RefreshTotalTransportCost() {
    var firstHalfKi = $("#TotalTransportCostYear option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshTotalTransportCost,
        data: { firstHalfKi: firstHalfKi },
        success: function (chData) {

            var aData = chData;
            var aDatasets1 = aData[0];
            var aDatasets2 = aData[1];
            var aDatasets3 = aData[2];


            totalTransportCostMonthlyChart.data.datasets[0].data = aDatasets1;
            totalTransportCostMonthlyChart.data.datasets[1].data = aDatasets2;
            totalTransportCostMonthlyChart.update();

            totalTransportCostAccumulatedChart.data.datasets[0].data = aDatasets3;
            totalTransportCostAccumulatedChart.update();

        }
    });
}

