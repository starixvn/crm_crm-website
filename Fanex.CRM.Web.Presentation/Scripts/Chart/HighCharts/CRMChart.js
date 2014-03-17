var crmChart = {
    width: 1200,
    height: 900,
    defaultChartOptions: {
        chart: {
            type: 'column'
        },
        title: {
            text: ''
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            categories: []
        },
        yAxis: {
            min: 0,
            title: {
                text: ''
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true,
            valueSuffix: ' Feedback(s)'
        },
        plotOptions:{
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [],
        credits: {
            enabled: false
        }
    },
    title: '',
    initChart: function (options) {
        if ($('#chartContainer').length === 0) {
            $('<div />').attr('id', 'chartContainer').appendTo($('body'));
        }

        this.assignReportContent(options);

        $('#chartContainer').dialog({
            width: this.width,
            height: this.height,
            zIndex: 1000,
            position: 'center',
            resizable: false,
            modal: true,
            title: this.title
        });
    },
    assignReportContent: function (options) {
        $('#chartContainer').html('');

        $('<div />').attr('id', 'container').css({ width: (this.width - 100), height: (this.height - 100) }).appendTo($('#chartContainer'));

        $('#chartContainer #container').highcharts(options);
    },
    viewChart: function (options) {

        if (typeof options !== 'object' || options === null) {
            options = this.defaultChartOptions;
        }
        else {
            options = $.extend({}, this.defaultChartOptions, options);
        }

        this.initChart(options);
    }
};