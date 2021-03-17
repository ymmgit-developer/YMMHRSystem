var monthlyHeadCountChart;
var accumulatedHeadCountChart;
var monthlyTurnoverChart;
var averageTurnoverChart;
var yearsOfServiceChart;
var monthlyAdmissionDismissalChart;
var totalAdmissionDismissalChart;
var monthlyDismissalByTypeChart;
var accumulatedDismissalByTypeChart;
var workerAgesChart;

function GenerateGraph() {

    $.ajax({
        type: "POST",
        url: $LoadWorkerFileGraphs,
        success: function (chData) {

            var yearsOfServiceColor = [];
            var workerAgesColor = [];

            var dynamicColors = function () {
                var r = Math.floor(Math.random() * 255);
                var g = Math.floor(Math.random() * 255);
                var b = Math.floor(Math.random() * 255);
                return "rgb(" + r + "," + g + "," + b + ")";
            };

            var aData = chData;
            var headCountMonth = aData[0];
            var headCountMen = aData[1];
            var headCountWomen = aData[2];
            var headCountTotal = aData[3];
            var headCountX = aData[4];
            var headCountY = aData[5];
            var turnOverX = aData[6];
            var turnOverY = aData[7];
            var averageTurnOverX = aData[8];
            var averageTurnOverY = aData[9];
            var yearsOfServiceX = aData[10];
            var yearsOfServiceY = aData[11];
            var monthlyAdmissionX = aData[12];
            var monthlyAdmissionY = aData[13];
            var monthlyDismissalY = aData[14];
            var totalAdmissionDismissalX = aData[15];
            var totalAdmissionDismissalY = aData[16];
            var monthlyDismissDismissalX = aData[17];
            var monthlyDismissDismissalY = aData[18];
            var monthlyResignationDismissalY = aData[19];
            var monthlyEndOfContractDismissalY = aData[20];
            var totalDismissalsByTypeX = aData[21];
            var totalDismissalsByTypeY = aData[22];
            var workerAgesX = aData[23];
            var workerAgesY = aData[24];

            for (var i = 0; i < yearsOfServiceY.length; i++) {
                yearsOfServiceColor.push(dynamicColors());
            }

            for (var i = 0; i < workerAgesY.length; i++) {
                workerAgesColor.push(dynamicColors());
            }

            var config1 = {
                type: 'bar',
                data: {
                    labels: headCountMonth,
                    datasets: [
                        {
                            data: headCountMen,
                            label: "Men",
                            backgroundColor: "rgb(54, 162, 235)",
                        },
                        {
                            data: headCountWomen,
                            label: "Women",
                            backgroundColor: "rgb(255, 99, 132)",

                        },
                        {
                            data: headCountTotal,
                            label: "Total",
                            backgroundColor: "rgb(201, 203, 207)",

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
                        data: headCountY,
                        backgroundColor:
                            [
                                "rgb(255, 99, 132)",
                                "rgb(54, 162, 235)"
                               
                            ]
                        ,
                        label: ''
                    }],
                    labels: headCountX
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
                    labels: turnOverX,
                    datasets: [
                        {
                            data: turnOverY,
                            label: "Turnover Rate %",
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
                type: 'bar',
                data: {
                    labels: averageTurnOverX,
                    datasets: [
                        {
                            data: averageTurnOverY,
                            label: "",
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
            var config5 = {
                type: 'bar',
                data: {
                    datasets: [{
                        data: yearsOfServiceY,
                        backgroundColor: yearsOfServiceColor,
                        label: '# of Workers',
                    }],
                    labels: yearsOfServiceX
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
            var config6 = {
                type: 'bar',
                data: {
                    labels: monthlyAdmissionX,
                    datasets: [
                        {
                            data: monthlyAdmissionY,
                            label: "Admissions",
                            backgroundColor: "rgb(255, 159, 64)",
                        },
                        {
                            data: monthlyDismissalY,
                            label: "Dismissals",
                            backgroundColor: "rgb(255, 99, 132)",
                        },
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
            var config7 = {
                type: 'doughnut',
                data: {
                    datasets: [{
                        data: totalAdmissionDismissalY,
                        backgroundColor:
                            [
                                "rgb(255, 159, 64)",
                                "rgb(255, 99, 132)",
                            ]
                        ,
                        label: ''
                    }],
                    labels: totalAdmissionDismissalX
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
            var config8 = {
                type: 'bar',
                data: {
                    labels: monthlyDismissDismissalX,
                    datasets: [
                        {
                            data: monthlyDismissDismissalY,
                            label: "Dismissals",
                            backgroundColor: "rgb(54, 162, 235)",
                        },
                        {
                            data: monthlyResignationDismissalY,
                            label: "Resignations",
                            backgroundColor: "rgb(255, 99, 132)",

                        },
                        {
                            data: monthlyEndOfContractDismissalY,
                            label: "End of Contract",
                            backgroundColor: "rgb(75, 192, 192)",

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
            var config9 = {
                type: 'doughnut',
                data: {
                    datasets: [{
                        data: totalDismissalsByTypeY,
                        backgroundColor:
                            [
                                "rgb(54, 162, 235)",
                                "rgb(255, 99, 132)",
                                "rgb(75, 192, 192)"
                            ]
                        ,
                        label: ''
                    }],
                    labels: totalDismissalsByTypeX
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
            var config10 = {
                type: 'doughnut',
                data: {
                    datasets: [{
                        data: workerAgesY,
                        backgroundColor: workerAgesColor,
                        label: ''
                    }],
                    labels: workerAgesX
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

            //For MonthlyHeadCountChart Chart
            var ctx = document.getElementById("MonthlyHeadCount").getContext('2d');
            monthlyHeadCountChart = new Chart(ctx, config1);

            //For AccumulatedHeadCount Chart
            var ctx2 = document.getElementById("AccumulatedHeadCount").getContext('2d');
            accumulatedHeadCountChart = new Chart(ctx2, config2);

            //For MonthlyTurnover Chart
            var ctx3 = document.getElementById("MonthlyTurnover").getContext('2d');
            monthlyTurnoverChart = new Chart(ctx3, config3);

            //For AverageTurnover Chart
            var ctx4 = document.getElementById("AverageTurnover").getContext('2d');
            averageTurnoverChart = new Chart(ctx4, config4);

            //For YearsOfService Chart
            var ctx5 = document.getElementById("YearsOfService").getContext('2d');
            yearsOfServiceChart = new Chart(ctx5, config5);

            //For MonthlyAdmissionDismissal Chart
            var ctx6 = document.getElementById("MonthlyAdmissionDismissal").getContext('2d');
            monthlyAdmissionDismissalChart = new Chart(ctx6, config6);

            //For TotalAdmissionDismissal Chart
            var ctx7 = document.getElementById("TotalAdmissionDismissal").getContext('2d');
            totalAdmissionDismissalChart = new Chart(ctx7, config7);

            //For MonthlyDismissalByType Chart
            var ctx8 = document.getElementById("MonthlyDismissalByType").getContext('2d');
            monthlyDismissalByTypeChart = new Chart(ctx8, config8);

            //For AccumulatedDismissalByType Chart
            var ctx9 = document.getElementById("AccumulatedDismissalByType").getContext('2d');
            accumulatedDismissalByTypeChart = new Chart(ctx9, config9);

            //For WorkerAges Chart
            var ctx10 = document.getElementById("WorkerAges").getContext('2d');
            workerAgesChart = new Chart(ctx10, config10);
        },
        "error": function (data) {
        }

    });
}

function RefreshHeadCount() {
    var firstHalfKi = $("#HeadCountYear option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshHeadCount,
        data: { firstHalfKi: firstHalfKi },
        success: function (chData) {

            var aData = chData;
            var headCountMen = aData[0];
            var headCountWomen = aData[1];
            var headCountTotal = aData[2];
            var headCountY = aData[3];


            monthlyHeadCountChart.data.datasets[0].data = headCountMen;
            monthlyHeadCountChart.data.datasets[1].data = headCountWomen;
            monthlyHeadCountChart.data.datasets[2].data = headCountTotal;
            monthlyHeadCountChart.update();

            accumulatedHeadCountChart.data.datasets[0].data = headCountY;
            accumulatedHeadCountChart.update();

        }
    });
}

function RefreshTurnover() {
    var firstHalfKi = $("#TurnoverYear option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshTurnover,
        data: { firstHalfKi: firstHalfKi },
        success: function (chData) {

            var aData = chData;
            var turnOverY = aData[0];
            var averageTurnOverY = aData[1];


            monthlyTurnoverChart.data.datasets[0].data = turnOverY;
            monthlyTurnoverChart.update();

            averageTurnoverChart.data.datasets[0].data = averageTurnOverY;
            averageTurnoverChart.update();

        }
    });
}

function RefreshAdmissionDismissal() {
    var firstHalfKi = $("#AdmissionDismissalYear option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshAdmissionDismissal,
        data: { firstHalfKi: firstHalfKi },
        success: function (chData) {

            var aData = chData;
            var monthlyAdmissionY = aData[0];
            var monthlyDismissalY = aData[1];
            var totalAdmissionDismissalY = aData[2];


            monthlyAdmissionDismissalChart.data.datasets[0].data = monthlyAdmissionY;
            monthlyAdmissionDismissalChart.data.datasets[1].data = monthlyDismissalY;
            monthlyAdmissionDismissalChart.update();

            totalAdmissionDismissalChart.data.datasets[0].data = totalAdmissionDismissalY;
            totalAdmissionDismissalChart.update();

        }
    });
}

function RefreshDismissalByType() {
    var firstHalfKi = $("#DismissalByTypeYear option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$RefreshDismissalByType,
        data: { firstHalfKi: firstHalfKi },
        success: function (chData) {

            var aData = chData;
            var monthlyDismissDismissalX = aData[0];
            var monthlyDismissDismissalY = aData[1];
            var monthlyResignationDismissalY = aData[2];
            var monthlyEndOfContractDismissalY = aData[3];
            var totalDismissalsByTypeX = aData[4];
            var totalDismissalsByTypeY = aData[5];

            monthlyDismissalByTypeChart.data.labels = monthlyDismissDismissalX;
            monthlyDismissalByTypeChart.data.datasets[0].data = monthlyDismissDismissalY;
            monthlyDismissalByTypeChart.data.datasets[1].data = monthlyResignationDismissalY;
            monthlyDismissalByTypeChart.data.datasets[2].data = monthlyEndOfContractDismissalY;
            monthlyDismissalByTypeChart.update();

            accumulatedDismissalByTypeChart.data.labels = totalDismissalsByTypeX;
            accumulatedDismissalByTypeChart.data.datasets[0].data = totalDismissalsByTypeY;
            accumulatedDismissalByTypeChart.update();
        }
    });
}


