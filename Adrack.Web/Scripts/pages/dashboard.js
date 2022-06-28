// Chart setup
function generateBarChart(element, barQty, height, animate, easing, duration, delay, color, tooltip, dataArray, dates) {
    if (!$(element).length) {
        return;
    }

    $(element).html("");

    // Add data set
    var bardata = [];
    for (var i = 0; i < dataArray.length; i++) {
        bardata.push(dataArray[i]);
    }
    // Main variables
    var d3Container = d3.select(element),
        width = d3Container.node().getBoundingClientRect().width;

    // Construct scales
    // ------------------------------
    // Horizontal
    var x = d3.scale.ordinal()
        .rangeBands([0, width], 0.3);

    // Vertical
    var y = d3.scale.linear()
        .range([0, height]);

    // Set input domains
    // ------------------------------

    // Horizontal
    x.domain(d3.range(0, bardata.length));

    // Vertical
    y.domain([0, d3.max(bardata)]);

    // Create chart
    // ------------------------------

    // Add svg element
    var container = d3Container.append("svg");

    // Add SVG group
    var svg = container
        .attr("width", width)
        .attr("height", height)
        .append("g");

    // Bars
    var bars = svg.selectAll("rect")
        .data(bardata)
        .enter()
        .append("rect")
        .attr("class", "d3-random-bars")
        .attr("width", x.rangeBand())
        .attr("x",
            function (d, i) {
                return x(i);
            })
        .style("fill", color);

    // Tooltip
    // ------------------------------
    var tip = d3.tip()
        .attr("class", "d3-tip")
        .offset([-10, 0]);

    // Show and hide
    if (tooltip == "hours" || tooltip == "goal" || tooltip == "members") {
        bars.call(tip)
            .on("mouseover", tip.show)
            .on("mouseout", tip.hide);
    }

    // Daily meetings tooltip content
    if (tooltip == "hours") {
        tip.html(function (d, i) {
            return "<div class='text-center'>" +
                "<h6 class='no-margin'>" +
                d +
                "</h6>" +
                "<span class='text-size-small'>meetings</span>" +
                "<div class='text-size-small'>" +
                i +
                ":00" +
                "</div>" +
                "</div>";
        });
    }

    // Statements tooltip content
    if (tooltip == "goal") {
        tip.html(function (d, i) {
            return "<div class='text-center'>" +
                "<h6 class='no-margin'>" +
                d +
                "</h6>" +
                "<div class='text-size-small'>" +
                dates[i] +
                "</div>" +
                "</div>";
        });
    }

    // Online members tooltip content
    if (tooltip == "members") {
        tip.html(function (d, i) {
            return "<div class='text-center'>" +
                "<h6 class='no-margin'>" +
                d +
                "0" +
                "</h6>" +
                "<span class='text-size-small'>members</span>" +
                "<div class='text-size-small'>" +
                i +
                ":00" +
                "</div>" +
                "</div>";
        });
    }

    // Bar loading animation
    // ------------------------------

    // Choose between animated or static
    if (animate) {
        withAnimation();
    } else {
        withoutAnimation();
    }

    // Animate on load
    function withAnimation() {
        bars
            .attr("height", 0)
            .attr("y", height)
            .transition()
            .attr("height",
                function (d) {
                    return y(d);
                })
            .attr("y",
                function (d) {
                    return height - y(d);
                })
            .delay(function (d, i) {
                return i * delay;
            })
            .duration(duration)
            .ease(easing);
    }

    // Load without animateion
    function withoutAnimation() {
        bars
            .attr("height",
                function (d) {
                    return y(d);
                })
            .attr("y",
                function (d) {
                    return height - y(d);
                });
    }

    // Resize chart
    // ------------------------------

    // Call function on window resize
    $(window).on("resize", barsResize);

    // Call function on sidebar width change
    $(document).on("click", ".sidebar-control", barsResize);

    // Resize function
    //
    // Since D3 doesn't support SVG resize by default,
    // we need to manually specify parts of the graph that need to
    // be updated on window resize
    function barsResize() {
        // Layout variables
        width = d3Container.node().getBoundingClientRect().width;

        // Layout
        // -------------------------

        // Main svg width
        container.attr("width", width);

        // Width of appended group
        svg.attr("width", width);

        // Horizontal range
        x.rangeBands([0, width], 0.3);

        // Chart elements
        // -------------------------

        // Bars
        svg.selectAll(".d3-random-bars")
            .attr("width", x.rangeBand())
            .attr("x",
                function (d, i) {
                    return x(i);
                });
    }
}

// Chart setup
function ticketStatusDonut(element, size, data) {
    if (!$(element).length) {
        return;
    }

    $(element).html("");

    // Basic setup
    // ------------------------------

    // Add data set
    /*
            var data = [
                {
                    "status": "Pending tickets",
                    "icon": "<i class='status-mark border-blue-300 position-left'></i>",
                    "value": 50,
                    "color": "#29B6F6"
                }, {
                    "status": "Resolved tickets",
                    "icon": "<i class='status-mark border-success-300 position-left'></i>",
                    "value": 50,
                    "color": "#66BB6A"
                }
            ];
    */
    // Main variables
    var d3Container = d3.select(element),
        distance = 2, // reserve 2px space for mouseover arc moving
        radius = (size / 2) - distance,
        sum = d3.sum(data, function (d) { return d.value; });

    // Tooltip
    // ------------------------------

    var tip = d3.tip()
        .attr("class", "d3-tip")
        .offset([-10, 0])
        .direction("e")
        .html(function (d) {
            return "<ul class='list-unstyled mb-5'>" +
                "<li>" +
                "<div class='text-size-base mb-5 mt-5'>" +
                d.data.icon +
                d.data.status +
                "</div>" +
                "</li>" +
                "<li>" +
                "Total: &nbsp;" +
                "<span class='text-semibold pull-right'>" +
                d.value.toFixed(2) +
                "</span>" +
                "</li>" +
                "<li>" +
                "Share: &nbsp;" +
                "<span class='text-semibold pull-right'>" +
                (100 / (sum / d.value)).toFixed(2) +
                "%" +
                "</span>" +
                "</li>" +
                "</ul>";
        });

    // Create chart
    // ------------------------------

    // Add svg element
    var container = d3Container.append("svg").call(tip);

    // Add SVG group
    var svg = container
        .attr("width", size)
        .attr("height", size)
        .append("g")
        .attr("transform", "translate(" + (size / 2) + "," + (size / 2) + ")");

    // Construct chart layout
    // ------------------------------

    // Pie
    var pie = d3.layout.pie()
        .sort(null)
        .startAngle(Math.PI)
        .endAngle(3 * Math.PI)
        .value(function (d) {
            return d.value;
        });

    // Arc
    var arc = d3.svg.arc()
        .outerRadius(radius)
        .innerRadius(radius / 2);

    //
    // Append chart elements
    //

    // Group chart elements
    var arcGroup = svg.selectAll(".d3-arc")
        .data(pie(data))
        .enter()
        .append("g")
        .attr("class", "d3-arc")
        .style("stroke", "#fff")
        .style("cursor", "pointer");

    // Append path
    var arcPath = arcGroup
        .append("path")
        .style("fill", function (d) { return d.data.color; });

    // Add tooltip
    arcPath
        .on("mouseover",
            function (d, i) {
                // Transition on mouseover
                d3.select(this)
                    .transition()
                    .duration(500)
                    .ease("elastic")
                    .attr("transform",
                        function (d) {
                            d.midAngle = ((d.endAngle - d.startAngle) / 2) + d.startAngle;
                            var x = Math.sin(d.midAngle) * distance;
                            var y = -Math.cos(d.midAngle) * distance;
                            return "translate(" + x + "," + y + ")";
                        });
            })
        .on("mousemove",
            function (d) {
                // Show tooltip on mousemove
                tip.show(d)
                    .style("top", (d3.event.pageY - 40) + "px")
                    .style("left", (d3.event.pageX + 30) + "px");
            })
        .on("mouseout",
            function (d, i) {
                // Mouseout transition
                d3.select(this)
                    .transition()
                    .duration(500)
                    .ease("bounce")
                    .attr("transform", "translate(0,0)");

                // Hide tooltip
                tip.hide(d);
            });

    // Animate chart on load
    arcPath
        .transition()
        .delay(function (d, i) { return i * 500; })
        .duration(500)
        .attrTween("d",
            function (d) {
                var interpolate = d3.interpolate(d.startAngle, d.endAngle);
                return function (t) {
                    d.endAngle = interpolate(t);
                    return arc(d);
                };
            });
}