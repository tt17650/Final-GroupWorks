﻿d3.json("/assets-YiChun/js/taiwanChi.json", function (geojson) {

    //var PopulationDensity = [
    //    {city:"台北市", ps : 9917.98},
    //    {city:"嘉義市", ps : 4495.98},
    //    {city:"新竹市", ps : 4199},
    //    {city:"基隆市", ps : 2802.83},
    //    {city:"新北市", ps : 1938.65},
    //    {city:"桃園市", ps : 1759.09},
    //    {city:"台中市", ps : 1249.38},
    //    {city:"彰化縣", ps : 1198.02},
    //    {city:"高雄市", ps : 941.57},
    //    {city:"金門縣", ps : 890.92},
    //    {city:"台南市", ps : 860.55},
    //    {city:"澎湖縣", ps : 813.97},
    //    {city:"雲林縣", ps : 538.31},
    //    {city:"連江縣", ps : 437.33},
    //    {city:"新竹縣", ps : 383.51},
    //    {city:"苗栗縣", ps : 307.19},
    //    {city:"屏東縣", ps : 301.12},
    //    {city:"嘉義縣", ps : 270.7},
    //    {city:"宜蘭縣", ps : 213.44},
    //    {city:"南投縣", ps : 123.02},
    //    {city:"花蓮縣", ps : 71.49},
    //    {city:"台東縣", ps : 62.81 }
    //];
    var PopulationDensity = '{\
                "台北市": 9917.98,\
                "嘉義市": 4495.98,\
                "新竹市": 4199,\
                "基隆市": 2802.83,\
                "新北市": 1938.65,\
                "桃園市": 1759.09,\
                "台中市": 1249.38,\
                "彰化縣": 1198.02,\
                "高雄市": 941.57,\
                "金門縣": 890.92,\
                "台南市": 860.55,\
                "澎湖縣": 813.97,\
                "雲林縣": 538.31,\
                "連江縣": 437.33,\
                "新竹縣": 383.51,\
                "苗栗縣": 307.19,\
                "屏東縣": 301.12,\
                "嘉義縣": 270.7,\
                "宜蘭縣": 213.44,\
                "南投縣": 123.02,\
                "花蓮縣": 71.49,\
                "台東縣": 62.81\
            }';
    var x, y, s;

    var zoom = d3.zoom()
        .scaleExtent([1, 10])
        .on("zoom", zoomed);

    function zoomed() {
        x = d3.event.translate;
        //y = d3.event.translate;
        s = d3.event.scale;
        //svg.attr("transform", "translate(" + d3.event.translate + ") scale(" + d3.event.scale + ")");
        svg.attr("transform", d3.event.transform);
    }

    var pd = JSON.parse(PopulationDensity)
    //alert(pd["台北市"]);
    var svg = d3.select("svg").append("g");


    var features = topojson.feature(geojson, geojson.objects["Taiwan"]).features;

    var path = d3.geoPath().projection( // 路徑產生器
        d3.geoMercator().center([119.5825975, 26]).scale(5500).translate([60, 50]) // 座標變換函式

    );
    //color
    var color = d3.scaleLinear().domain([50, 10000]).range(["#008000", "#000000"]);

    var taiwan = svg.selectAll("path").data(features)
      .enter().append("path").attr("d", path);

    //繪製台灣地圖
    taiwan.attr("class", "subunit-boundary").style("fill", function (d) {
        return color(pd[d.properties["COUNTYNAME"]]);
    });


    //var zoom = d3.zoom()
    //.scaleExtent([1, 10])
    //.on("zoom", zoomed);

    //function zoomed() {
    //    taiwan.attr("transform", "translate(" + d3.event.translate + ")scale(" + d3.event.scale + ")")
    //}
    //svg.call(zoom).append("g");
    //台灣-滑鼠移入顯示縣市名稱
    d3.select("svg").selectAll("path")
    .on("mouseover", function () {
        //d3.select(this).append("svg:title").text(function (d) {
        //    return d.properties["COUNTYNAME"];
        //});
        if (d3.select(this).attr("class") == "subunit-boundary")
            d3.select(this).transition().duration(10).attr("class", "subunit-boundary").style("fill", function (d) {
                return "#ff6600";
            });

    });
    //台灣-點擊
    d3.select("svg").selectAll("path")
    .on("click", function (d) {
        if (!d3.select(this).classed("taiwanClick")) {
            d3.select(".taiwanClick").transition().duration(500)
                .attr("class", "subunit-boundary").style("fill", function (d) {
                    return color(pd[d.properties["COUNTYNAME"]]);
                });
            d3.select(this).attr("class", "taiwanClick").style("fill", "#FF0000");
        }
        else if (d3.select(this).classed("taiwanClick")) {
            d3.select(this).transition().duration(500)
                .attr("class", "subunit-boundary").style("fill", function (d) {
                    return color(pd[d.properties["COUNTYNAME"]]);
                });
        }
        //d3.select("#circle").select("p").text("你現在位於" + d.properties["COUNTYNAME"] + "!");

    })
    .on("mouseout", function () {
        //d3.select("#circle").select("p").text("隨便點個滑鼠左鍵");
        if (d3.select(this).attr("class") == "subunit-boundary")
            d3.select(this).transition().duration(500).attr("class", "subunit-boundary").style("fill", function (d) {
                return color(pd[d.properties["COUNTYNAME"]]);
            });
    });

});



//var toggle = 0;
//window.onclick = function () {

//    if (toggle == 0) {
//        d3.select("body").transition().duration(2000)
//        .style("background-color", "black");
//        toggle = 1;
//    } else {
//        d3.select("body").transition().duration(2000)
//        .style("background-color", "white");
//        toggle = 0;
//    }

//}