
function showCharts(chartType) {
    var ctx = document.getElementById('chart-' + chartType);
    var chartBody = $("#chart-body-" + chartType);
    var labels = chartBody.data("labels")
    var counts = chartBody.data("counts")
    var colors = chartBody.data("colors")
    var myChart = new Chart(ctx,
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "data": counts,
                    "backgroundColor": colors
                }]

            },
            options: {
                layout: {
                    padding: {
                        left: 0,
                        right: 0,
                        top: 0,
                        bottom: 0
                    }
                },
                responsive: true,
                legend: {
                    display: false,
                    position: 'bottom'
                }
            }
        });
    var legend = myChart.generateLegend();
    var legendNodes = $.parseHTML(legend);
    var html = "";
    var count = 0;
    $(legendNodes).find("li").each(function (index) {
        var color = $(this).find("span").css("background-color");
        html += "<div class='d-flex'>";
        html += "<div class='legend-bg' style='background-color:" + color + "'></div>";
        html += $(this).text() + "(" + counts[count] + ")";
        html += "</div>"
        count++;
    });
    document.getElementById('legend-' + chartType).innerHTML = html;
}

function loadOnScroll() {
    var sections = ["ip"];
    for (var i = 0; i < sections.length; i++) {
        if (isInViewport(document.querySelector('#' + sections[i] + '-outer-container'))) {
            var icon = $("#" + sections[i] + "-title").find("i");
            var isExpanded = $(icon).hasClass("bi-chevron-up");
            if (!isExpanded) {
                //Check if the user clicked and collapsed the section. If so, don't expand again
                var userCollapsed = $("#" + sections[i] + "-title").attr("data-user-collapsed") == "true";
                if (!userCollapsed) {
                    $(icon).click();
                }
            }
        }
    }
}
function loadOnScrollProperty() {
    var sections = ["transactions", "recovery", "suspense", "attachments", "emails", "notes", "related-claims"];
    for (var i = 0; i < sections.length; i++) {
        if (isInViewport(document.querySelector('#' + sections[i] + '-outer-container'))) {
            var icon = $("#" + sections[i] + "-title").find("i");
            var isExpanded = $(icon).hasClass("bi-chevron-up");
            if (!isExpanded) {
                //Check if the user clicked and collapsed the section. If so, don't expand again
                var userCollapsed = $("#" + sections[i] + "-title").attr("data-user-collapsed") == "true";
                if (!userCollapsed) {
                    $(icon).click();
                }
            }
        }
    }
}
