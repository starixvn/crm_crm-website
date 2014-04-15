var crosstxt1 = {
    crossText: ["Up left", "up center", "up right", "mid left", "mid center", "mid right", "down left", "down center", "down right"],
    crossTextOverlay: [true],
    crossTextFontFamily: ["'Arial'"],
    crossTextFontSize: [20],
    crossTextFontColor: ["black"],
    crossTextFontStyle: ["italic"],
    crossTextRelativePosX: [0, -2, 4, 0, -2, 4, 0, -2, 4],
    crossTextRelativePosY: [0, 0, 0, -2, -2, -2, 4, 4, 4],
    crossTextBaseline: ["default"],
    crossTextAlign: ["default"],
    crossTextPosX: [0, 0, 0, 0, 0, 0, 0, 0, 0],
    crossTextPosY: [0, 0, 0, 0, 0, 0, 0, 0, 0],
    crossTextAngle: [0],
    canvasBorders: true,
    canvasBordersWidth: 3,
    canvasBordersColor: "black",
    graphTitle: "Draw text through the graph",
    graphTitleFontFamily: "'Arial'",
    graphTitleFontSize: 24,
    graphTitleFontStyle: "bold",
    graphTitleFontColor: "#666",
    dynamicDisplay: true
}

var crosstxt2 = {
    crossText: ["Up left", "up center", "up right", "mid left", "mid center", "mid right", "down left", "down center", "down right"],
    crossTextOverlay: [true],
    crossTextFontFamily: ["'Arial'"],
    crossTextFontSize: [20],
    crossTextFontColor: ["black"],
    crossTextFontStyle: ["italic"],
    crossTextRelativePosX: [1, 2, 3, 1, 2, 3, 1, 2, 3],
    crossTextRelativePosY: [1, 1, 1, 2, 2, 2, 3, 3, 3],
    crossTextBaseline: ["default"],
    crossTextAlign: ["default"],
    crossTextPosX: [0, 0, 0, 0, 0, 0, 0, 0, 0],
    crossTextPosY: [0, 0, 0, 0, 0, 0, 0, 0, 0],
    crossTextAngle: [0],
    canvasBorders: true,
    canvasBordersWidth: 3,
    canvasBordersColor: "black",
    graphTitle: "Draw text through the graph",
    graphTitleFontFamily: "'Arial'",
    graphTitleFontSize: 24,
    graphTitleFontStyle: "bold",
    graphTitleFontColor: "#666",
    footNote: " ",
    spaceTop: 30,
    spaceBottom: 30,
    spaceLeft: 30,
    spaceRight: 30,
    dynamicDisplay: true
}

function dispPct(numtxt, valtxt, ctx, config, posX, posY, borderX, borderY, overlay, data, animPC) {
    //  alert("INIT");
    return (Math.round(100 * animPC));
}

var crosstxt3 = {
    animationEasing: "linear",
    crossText: ["%call function..."],
    crossTextOverlay: [true],
    crossTextFontSize: [50],
    crossTextFontColor: ["black"],
    crossTextRelativePosX: [2],
    crossTextRelativePosY: [2],
    crossTextFunction: dispPct,
    graphTitle: "Draw text through the graph",
    graphTitleFontFamily: "'Arial'",
    graphTitleFontSize: 24,
    graphTitleFontStyle: "bold",
    graphTitleFontColor: "#666",
    canvasBorders: true,
    canvasBordersWidth: 3,
    canvasBordersColor: "black",
    spaceTop: 30,
    spaceBottom: 30,
    spaceLeft: 30,
    spaceRight: 30,
    startAngle: 180,
    dynamicDisplay: true
}

function dispPct2(numtxt, valtxt, ctx, config, posX, posY, borderX, borderY, overlay, data, animPC) {
    //  alert("INIT");
    return (Math.round(animPC * data[0].value));
}

var crosstxt4 = {
    animationEasing: "linear",
    crossText: ["%call function..."],
    crossTextOverlay: [true],
    crossTextFontSize: [50],
    crossTextFontColor: ["black"],
    crossTextRelativePosX: [2],
    crossTextRelativePosY: [2],
    crossTextFunction: dispPct2,
    canvasBorders: true,
    canvasBordersWidth: 3,
    canvasBordersColor: "black",
    graphTitle: "Draw text through the graph",
    graphTitleFontFamily: "'Arial'",
    graphTitleFontSize: 24,
    graphTitleFontStyle: "bold",
    graphTitleFontColor: "#666",
    spaceTop: 30,
    spaceBottom: 30,
    spaceLeft: 30,
    spaceRight: 30,
    startAngle: 180,
    dynamicDisplay: true
}

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
    allopts: {
        //Boolean - If we show the scale above the chart data	  -> Default value Changed
        scaleOverlay: true,
        //Boolean - If we want to override with a hard coded scale
        scaleOverride: false,
        //** Required if scaleOverride is true **
        //Number - The number of steps in a hard coded scale
        scaleSteps: null,
        //Number - The value jump in the hard coded scale
        scaleStepWidth: null,
        //Number - The scale starting value
        scaleStartValue: null,
        //String - Colour of the scale line	
        scaleLineColor: "rgba(0,0,0,.1)",
        //Number - Pixel width of the scale line	
        scaleLineWidth: 1,
        //Boolean - Whether to show labels on the scale	
        scaleShowLabels: true,
        //Interpolated JS string - can access value
        scaleLabel: "<%=value%>",
        //String - Scale label font declaration for the scale label
        scaleFontFamily: "'Arial'",
        //Number - Scale label font size in pixels	
        scaleFontSize: 12,
        //String - Scale label font weight style	
        scaleFontStyle: "normal",
        //String - Scale label font colour	
        scaleFontColor: "#666",
        ///Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: true,
        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",
        //Number - Width of the grid lines
        scaleGridLineWidth: 1,
        //Boolean - Whether the line is curved between points -> Default value Changed 
        bezierCurve: false,
        //Boolean - Whether to show a dot for each point -> Default value Changed
        pointDot: false,
        //Number - Radius of each point dot in pixels
        pointDotRadius: 3,
        //Number - Pixel width of point dot stroke
        pointDotStrokeWidth: 1,
        //Boolean - Whether to show a stroke for datasets
        datasetStroke: true,
        //Number - Pixel width of dataset stroke
        datasetStrokeWidth: 2,
        //Boolean - Whether to fill the dataset with a colour
        datasetFill: true,
        //Boolean - Whether to animate the chart             -> Default value changed
        animation: false,
        //Number - Number of animation steps
        animationSteps: 60,
        //String - Animation easing effect
        animationEasing: "easeOutQuart",
        //Function - Fires when the animation is complete
        onAnimationComplete: null,
        canvasBorders: true,
        canvasBordersWidth: 30,
        canvasBordersColor: "black",
        yAxisLeft: true,
        yAxisRight: true,
        yAxisLabel: "Y axis",
        yAxisFontFamily: "'Arial'",
        yAxisFontSize: 50,
        yAxisFontStyle: "normal",
        yAxisFontColor: "#666",
        xAxisLabel: "",
        xAxisFontFamily: "'Arial'",
        xAxisFontSize: 16,
        xAxisFontStyle: "normal",
        xAxisFontColor: "#666",
        yAxisUnit: "UNIT",
        yAxisUnitFontFamily: "'Arial'",
        yAxisUnitFontSize: 12,
        yAxisUnitFontStyle: "normal",
        yAxisUnitFontColor: "#666",
        graphTitle: "",
        graphTitleFontFamily: "'Arial'",
        graphTitleFontSize: 24,
        graphTitleFontStyle: "bold",
        graphTitleFontColor: "#666",
        graphSubTitle: "",
        graphSubTitleFontFamily: "'Arial'",
        graphSubTitleFontSize: 18,
        graphSubTitleFontStyle: "normal",
        graphSubTitleFontColor: "#666",
        footNote: "Footnote",
        footNoteFontFamily: "'Arial'",
        footNoteFontSize: 50,
        footNoteFontStyle: "bold",
        footNoteFontColor: "#666",
        legend: true,
        legendFontFamily: "'Arial'",
        legendFontSize: 18,
        legendFontStyle: "normal",
        legendFontColor: "#666",
        legendBlockSize: 30,
        legendBorders: true,
        legendBordersWidth: 30,
        legendBordersColor: "#666",
        //  ADDED PARAMETERS 
        graphMin: "DEFAULT",
        graphMax: "DEFAULT"

    },
    noopts: {
        nooptions: "",
        yAxisRight: true,
        scaleTickSizeLeft: 0,
        scaleTickSizeRight: 0,
        scaleTickSizeBottom: 0,
        scaleTickSizeTop: 1
    },
    onlyborderopts: {
        canvasBorders: true,
        canvasBordersWidth: 3,
        canvasBordersColor: "black"
    },
    newopts: {
        inGraphDataShow: true,
        datasetFill: true,
        scaleLabel: "<%=value%>",
        scaleTickSizeRight: 5,
        scaleTickSizeLeft: 5,
        scaleTickSizeBottom: 5,
        scaleTickSizeTop: 5,
        scaleFontSize: 16,
        canvasBorders: false,
        canvasBordersWidth: 3,
        canvasBordersColor: "black",
        graphTitle: "",
        graphTitleFontFamily: "'Arial'",
        graphTitleFontSize: 24,
        graphTitleFontStyle: "bold",
        graphTitleFontColor: "#666",
        graphSubTitle: "",
        graphSubTitleFontFamily: "'Arial'",
        graphSubTitleFontSize: 18,
        graphSubTitleFontStyle: "normal",
        graphSubTitleFontColor: "#666",
        footNote: "",
        footNoteFontFamily: "'Arial'",
        footNoteFontSize: 8,
        footNoteFontStyle: "bold",
        footNoteFontColor: "#666",
        legend: true,
        legendFontFamily: "'Arial'",
        legendFontSize: 12,
        legendFontStyle: "normal",
        legendFontColor: "#666",
        legendBlockSize: 15,
        legendBorders: true,
        legendBordersWidth: 1,
        legendBordersColors: "#666",
        yAxisLeft: true,
        yAxisRight: false,
        xAxisBottom: true,
        xAxisTop: false,
        yAxisLabel: "",
        yAxisFontFamily: "'Arial'",
        yAxisFontSize: 16,
        yAxisFontStyle: "normal",
        yAxisFontColor: "#666",
        xAxisLabel: "",
        xAxisFontFamily: "'Arial'",
        xAxisFontSize: 10,
        xAxisFontStyle: "normal",
        xAxisFontColor: "#666",
        yAxisUnit: "",
        yAxisUnitFontFamily: "'Arial'",
        yAxisUnitFontSize: 10,
        yAxisUnitFontStyle: "normal",
        yAxisUnitFontColor: "#666",
        annotateDisplay: true,
        spaceTop: 0,
        spaceBottom: 0,
        spaceLeft: 0,
        spaceRight: 0,
        logarithmic: false,
        //      showYAxisMin : false,
        rotateLabels: "smart",
        xAxisSpaceOver: 0,
        xAxisSpaceUnder: 0,
        xAxisLabelSpaceAfter: 0,
        xAxisLabelSpaceBefore: 0,
        legendBordersSpaceBefore: 0,
        legendBordersSpaceAfter: 0,
        footNoteSpaceBefore: 0,
        footNoteSpaceAfter: 0,
        startAngle: 0,
        dynamicDisplay: false
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

        var canvas = document.createElement('canvas');
        document.getElementById("chartContainer").appendChild(canvas);
        canvas.setAttribute("width", this.width - 100);
        canvas.setAttribute("height", this.height - 100);

        if ($.browser.msie && parseInt($.browser.version, 10) === 8) {
            canvas = G_vmlCanvasManager.initElement(canvas);
        }

        switch (options.chartType) {
            case this.chartType.line:
                setTimeout(function () {
                    var myNewChart = new parent.Chart(canvas.getContext("2d")).Line(data, options);
                }, 200);
                break;
            case this.chartType.pie:
                setTimeout(function () {
                    var myNewChart = new parent.Chart(canvas.getContext("2d")).Pie(data, options);
                }, 200);
                break;
            case this.chartType.bar:
                setTimeout(function () {
                    var myNewChart = new parent.Chart(canvas.getContext("2d")).Bar(data, options);
                }, 200);
                break;
            case this.chartType.radar:
                setTimeout(function () {
                    var myNewChart = new parent.Chart(canvas.getContext("2d")).Radar(data, options);
                }, 200);
                break;
            case this.chartType.polarArea:
                setTimeout(function () {
                    var myNewChart = new parent.Chart(canvas.getContext("2d")).PolarArea(data, options);
                }, 200);
                break;
            case this.chartType.doughnut:
                setTimeout(function () {
                    var myNewChart = new parent.Chart(canvas.getContext("2d")).Doughnut(data, options);
                }, 200);
                break;
            default:
                setTimeout(function () {
                    var myNewChart = new parent.Chart(canvas.getContext("2d")).Bar(data, options);
                }, 200);
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
                       "#CC6699", "#9370db"],
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

        // Darken 20% for 1 times
        var darkenPercent = Math.floor(index / backGroundColorLength) / 5;
        if (darkenPercent >= 1) {
            darkenPercent = 0;
        }

        var backGroundColor = darkenPercent === 0 ?
            crmChart.backGroundColors[backGroundColorIndex] :
            crmChart.shadeColor(crmChart.backGroundColors[backGroundColorIndex], -darkenPercent);
        return backGroundColor;
    }
};