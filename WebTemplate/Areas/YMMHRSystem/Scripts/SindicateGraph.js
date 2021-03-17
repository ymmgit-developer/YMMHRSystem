var sindicateAmountsMonthlyChart;
var sindicateAmountsAccumulatedChart;

function GenerateGraph() {

    $.ajax({
        type: "POST",
        url: $LoadSindicateGraphs,
        success: function (chData) {
            var aData = chData;
            var aLabels1 = aData[0];
            var aDatasets1 = aData[1];
            var aDatasets2 = aData[2];
            var aLabels2 = aData[3];
            var aDatasets3 = aData[4];

            var config1 = {
                type: 'bar',
                data: {
                    labels: aLabels1,
                    datasets: [
                        {
                            data: aDatasets1,
                            label: "Administrative Support",
                            backgroundColor: "rgb(54, 162, 235)",
                        },
                        {
                            data: aDatasets2,
                            label: "Union Fee",
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
                                scale.max += 1000;
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

            //For Sindicate Amounts Monthly Chart
            var ctx = document.getElementById("SindicateAmountsMonthly").getContext('2d');
            sindicateAmountsMonthlyChart = new Chart(ctx, config1);

            //For Sindicate Amounts Accumulated Chart
            var ctx2 = document.getElementById("SindicateAmountsAccumulated").getContext('2d');
            sindicateAmountsAccumulatedChart = new Chart(ctx2, config2);
        },
        "error": function (data) {
        }

    });
}

function RefreshSindicateAmounts() {
    var firstHalfKi = $("#SindicateAmountsYear option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshSindicateAmounts,
        data: { firstHalfKi: firstHalfKi },
        success: function (chData) {

            var aData = chData;
            var aDatasets1 = aData[0];
            var aDatasets2 = aData[1];
            var aDatasets3 = aData[2];


            sindicateAmountsMonthlyChart.data.datasets[0].data = aDatasets1;
            sindicateAmountsMonthlyChart.data.datasets[1].data = aDatasets2;
            sindicateAmountsMonthlyChart.update();

            sindicateAmountsAccumulatedChart.data.datasets[0].data = aDatasets3;
            sindicateAmountsAccumulatedChart.update();

        }
    });
}

