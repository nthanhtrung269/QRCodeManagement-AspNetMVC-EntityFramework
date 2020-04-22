var Page = Page || {};
Page.Chart = Page.Chart || {};

Page.Chart = (function () {
    'use strict';

    var renderScanOverTimeChart = function () {
        var scanOverTimeOptions = {
            series: {
                lines: {
                    show: true
                },
                points: {
                    show: true
                }
            },
            grid: {
                hoverable: true //IMPORTANT! this is needed for tooltip to work
            },
            xaxis: {
                mode: "time",
                timeformat: "%Y/%m/%d",
                reserveSpace: true
            },
            legend: {
                show: false
            },
            tooltip: true,
            tooltipOpts: {
                content: "<b>%x</b><br/>Scans: <b>%y</b>",
                shifts: {
                    x: -60,
                    y: 25
                }
            }
        };

        $.plot($("#flot-line-chart"), [{ data: scanOverTimeData }], scanOverTimeOptions);
    };

    var renderScanByOperationSystemChart = function () {
        function labelFormatter(label, series) {
            return "<div style='font-size:8pt; text-align:center; padding:2px; color:white;'>" + label + "<br/>" + Math.round(series.percent) + "%</div>";
        }

        var scanByOperationSystemOptions = {
            series: {
                pie: {
                    show: true,
                    radius: 1,
                    label: {
                        show: true,
                        radius: 3 / 4,
                        formatter: labelFormatter,
                        background: {
                            opacity: 0.5
                        }
                    }
                }
            },
            legend: {
                show: false
            }
        };

        $.plot('#flot-pie-chart', scanByOperationSystemData, scanByOperationSystemOptions);
    };

    var init = function () {
        $('#scanByTopCitiesDataTables').DataTable({
            responsive: true
        });

        $('#fromDate').datepicker({
            format: 'mm/dd/yyyy'
        }).on('changeDate', function (ev) {
            $('#fromDate').attr('value', $('#fromDate').val());
        });

        $('#toDate').datepicker({
            format: 'mm/dd/yyyy'
        }).on('changeDate', function (ev) {
            $('#toDate').attr('value', $('#toDate').val());
        });

        $('#archiveQrCode').on('click', function () {
            confirm('Do you want to archive this QR code ?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    window.location = $('#archiveQrCode').attr('data-url');
                }
            });
        });

        $('#restoreQrCode').on('click', function () {
            confirm('Do you want to restore this QR code ?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    window.location = $('#restoreQrCode').attr('data-url');
                }
            });
        });

        $('#deleteQrCode').on('click', function () {
            confirm('Do you want to delete this QR code ?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    window.location = $('#deleteQrCode').attr('data-url');
                }
            });
        });

        if ($('#totalScans').val() !== '0') {
            renderScanOverTimeChart();
            renderScanByOperationSystemChart();
        }
    };

    return {
        init: init
    };
})();

$(document).ready(function () {
    Page.Chart.init();
});

