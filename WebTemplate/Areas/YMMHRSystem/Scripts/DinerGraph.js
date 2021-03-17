var dinerServicesByTypeChart;
var dinerCostServicesByTypeChart;
var paymentComparativeChart;
var extraDinerServicesByProcessChart;

Chart.helpers.merge(Chart.defaults.global.plugins.datalabels, {
    anchor: 'end',
    align: 'end',
});

function GenerateGraph() {

    $.ajax({
        type: "POST",
        url: $LoadDinerGraphs,
        success: function (chData) {
            var aData = chData;
            var aLabels1 = aData[0];
            var aDatasets1 = aData[1];
            var aLabels2 = aData[2];
            var aDatasets2 = aData[3];
            var aLabels3 = aData[4];
            var aDatasets3 = aData[5];
            var aLabels4 = aData[6];
            var aDatasets4 = aData[7];
            var aDatasets5 = aData[8];
            var aDatasets6 = aData[9];
            var aDatasets7 = aData[10];

            var config1 = {
                type: 'bar',
                data: {
                    labels: aLabels1,
                    datasets: [
                        {
                            data: aDatasets1,
                            label: "# of Services",
                            backgroundColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                        }
                    ]

                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
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
                type: 'bar',
                data: {
                    labels: aLabels2,
                    datasets: [
                        {
                            data: aDatasets2,
                            label: "Cost",
                            backgroundColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                        }
                    ]

                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                precision: 0
                            },
                            afterDataLimits(scale) {
                                scale.max += 5000;
                            }
                        }],
                    }
                }
            };
            var config3 = {
                type: 'bar',
                data: {
                    labels: aLabels3,
                    datasets: [
                        {
                            data: aDatasets3,
                            label: "Total Cost",
                            backgroundColor: ["rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(255, 99, 132)"],
                        }
                    ]

                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                precision: 0
                            },
                            afterDataLimits(scale) {
                                scale.max += 5000;
                            }
                        }],
                    }
                }
            };
            var config4 = {
                type: 'bar',
                data: {
                    labels: aLabels4,
                    datasets: [
                        {
                            data: aDatasets4,
                            label: "Weekend",
                            backgroundColor: "rgb(75, 192, 192)",
                        },
                        {
                            data: aDatasets5,
                            label: "Box Lunch",
                            backgroundColor: "rgb(54, 162, 235)",
                        },
                        {
                            data: aDatasets6,
                            label: "Special",
                            backgroundColor: "rgb(255, 99, 132)",
                        },
                        {
                            data: aDatasets7,
                            label: "Other",
                            backgroundColor: "rgb(153, 102, 255)",
                        },
                    ]

                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        position: 'top',
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

            //For DinerServicesByType Chart
            var ctx = document.getElementById("DinerServicesByType").getContext('2d');
            dinerServicesByTypeChart = new Chart(ctx, config1);

            //For DinerCostServicesByType Chart
            var ctx2 = document.getElementById("DinerCostServicesByType").getContext('2d');
            dinerCostServicesByTypeChart = new Chart(ctx2, config2);

            //For PaymentComparative Chart
            var ctx3 = document.getElementById("PaymentComparative").getContext('2d');
            paymentComparativeChart = new Chart(ctx3, config3);

            //For ExtraDinerServicesByProcess Chart
            var ctx4 = document.getElementById("ExtraDinerServicesByProcess").getContext('2d');
            extraDinerServicesByProcessChart = new Chart(ctx4, config4);
        },
        "error": function (data) {
        }

    });
}

function RefreshDinerServicesByType() {
    var startDate = $("#StartDate1").val();
    var endDate = $("#EndDate1").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshDinerServicesByType,
        data: { startDate: startDate, endDate: endDate },
        success: function (chData) {

            var aData = chData;
            var aLabels1 = aData[0];
            var aDatasets1 = aData[1];

            dinerServicesByTypeChart.data.labels = aLabels1;
            dinerServicesByTypeChart.data.datasets[0].data = aDatasets1;
            dinerServicesByTypeChart.update();
        }
    });
}

function RefreshDinerCostServicesByType() {
    var startDate = $("#StartDate2").val();
    var endDate = $("#EndDate2").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshDinerCostServicesByType,
        data: { startDate: startDate, endDate: endDate },
        success: function (chData) {

            var aData = chData;
            var aLabels2 = aData[0];
            var aDatasets2 = aData[1];

            dinerCostServicesByTypeChart.data.labels = aLabels2;
            dinerCostServicesByTypeChart.data.datasets[0].data = aDatasets2;
            dinerCostServicesByTypeChart.update();

        }
    });
}

function RefreshPaymentComparative() {
    var startDate = $("#StartDate3").val();
    var endDate = $("#EndDate3").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshPaymentComparative,
        data: { startDate: startDate, endDate: endDate },
        success: function (chData) {

            var aData = chData;
            var aLabels3 = aData[0];
            var aDatasets3 = aData[1]

            paymentComparativeChart.data.labels = aLabels3;
            paymentComparativeChart.data.datasets[0].data = aDatasets3;
            paymentComparativeChart.update();

        }
    });
}

function RefreshExtraDinerServicesByProcess() {
    var startDate = $("#StartDate4").val();
    var endDate = $("#EndDate4").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshExtraDinerServicesByProcess,
        data: { startDate: startDate, endDate: endDate },
        success: function (chData) {

            var aData = chData;
            var aLabels1 = aData[0]
            var aDatasets1 = aData[1];
            var aDatasets2 = aData[2];
            var aDatasets3 = aData[3];
            var aDatasets4 = aData[4];

            extraDinerServicesByProcessChart.data.labels = aLabels1;
            extraDinerServicesByProcessChart.data.datasets[0].data = aDatasets1;
            extraDinerServicesByProcessChart.data.datasets[1].data = aDatasets2;
            extraDinerServicesByProcessChart.data.datasets[2].data = aDatasets3;
            extraDinerServicesByProcessChart.data.datasets[3].data = aDatasets4;
            extraDinerServicesByProcessChart.update();
        }
    });
}

