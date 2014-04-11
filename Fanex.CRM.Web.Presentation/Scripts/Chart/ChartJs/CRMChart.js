var crmChart = {
    width: 1200,
    height: 900,
    title: '',
    chartType: {
        line: 1,
        pie: 2,
        bar: 3,
        radar: 4,
        polarArea: 5,
        doughnut: 6
    },
    randomColor: function () {
        var rint = Math.round(0xffffff * Math.random());
        return 'rgb(' + (rint >> 16) + ',' + (rint >> 8 & 255) + ',' + (rint & 255) + ')';
    },
    initChart: function (data, options) {
        if ($('#chartContainer').length === 0) {
            $('<div />').attr('id', 'chartContainer').appendTo($('body'));
        }

        this.assignReportContent(data, options);

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
    assignReportContent: function (data, options) {
        $('#chartContainer').html('');
        $('#chartContainer').append('<canvas id="myChart" width="' + (this.width - 100) + '" height="' + (this.height - 100) + '"></canvas>');
        //Get context with jQuery - using jQuery's .get() method.
        var ctx = $("#chartContainer #myChart").get(0).getContext("2d");
        
        switch (options.chartType) {
            case this.chartType.line:
                new parent.Chart(ctx).Line(data, options);
                break;
            case this.chartType.pie:
                new parent.Chart(ctx).Pie(data, options);
                break;
            case this.chartType.bar:
                new parent.Chart(ctx).Bar(data, options);
                break;
            case this.chartType.radar:
                new parent.Chart(ctx).Radar(data, options);
                break;
            case this.chartType.polarArea:
                new parent.Chart(ctx).PolarArea(data, options);
                break;
            case this.chartType.doughnut:
                new parent.Chart(ctx).Doughnut(data, options);
                break;
            default:
                new parent.Chart(ctx).Bar(data, options);
                break;
        }
        
    },
    viewChart: function (data, options) {
        this.initChart(data, options);
    }
};