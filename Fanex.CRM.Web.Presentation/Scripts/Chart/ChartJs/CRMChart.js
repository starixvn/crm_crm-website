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
    },
    getIntScaleInfo: function (maxYAxis, scaleLabelSuffix) {
        var scaleStepWidth = maxYAxis < 50 ?
            1 :
            Math.ceil(maxYAxis / 50);
        var scaleSteps = maxYAxis < 50 ? maxYAxis : 50;

        return {
            scaleStepWidth: scaleStepWidth,
            scaleSteps: scaleSteps,
            scaleStartValue: 0,
            scaleLabel: "<%=value%>" + (scaleLabelSuffix && scaleLabelSuffix != "" ? scaleLabelSuffix : "")
        };
    },

    backGroundColors: ["#3F5D7D", "#279B61", "#008AB8",
                       "#993333", "#A3E496", "#95CAE4",
                       "#CC3333", "#FFCC33", "#FFFF7A",
                       "#CC6699"],
    shadeColor: function (color, percent) {
        // color: #FFFFFF

        var colorHexaValue = parseInt(color.slice(1), 16);
        var t = percent < 0 ? 0 : 255;
        var p = percent < 0 ? percent * -1 : percent;
        var redColor = colorHexaValue >> 16;
        var greenColor = colorHexaValue >> 8 & 0x00FF;
        var blueColor = colorHexaValue & 0x0000FF;
        var backGroundColor = {
            Red: 0x1000000 + (Math.round((t - redColor) * p) + redColor) * 0x10000,
            Green: (Math.round((t - greenColor) * p) + greenColor) * 0x100,
            Blue: (Math.round((t - blueColor) * p) + blueColor)
        };

        return "#" + (backGroundColor.Red +
               backGroundColor.Green +
               backGroundColor.Blue).toString(16).slice(1);
    },
    getColorFromCode: function (colorCode) {
        var colorHexaValue = parseInt(colorCode.slice(1), 16);
        return 'rgb(' +
            (colorHexaValue >> 16) + ',' +
            (colorHexaValue >> 8 & 255) + ',' +
            (colorHexaValue & 255) + ')';
    },
    getBackGroundColor: function (index) {
        var backGroundColorLength = crmChart.backGroundColors.length;
        var backGroundColorIndex = index % backGroundColorLength;

        // Darken 10% for 1 times
        var darkenPercent = Math.floor(index / backGroundColorLength) / 10;
        if (darkenPercent >= 1) {
            darkenPercent = 0;
        }

        var backGroundColor = darkenPercent === 0 ?
            crmChart.backGroundColors[backGroundColorIndex] :
            crmChart.shadeColor(crmChart.backGroundColors[backGroundColorIndex], -darkenPercent);
        return backGroundColor;
    }
};