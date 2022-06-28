
function DashboardInit() {    

    adrackAPI.getTopStates(new Date(), new Date(),
        function (result) {
            Init3DPieStates(result.Series, "Recieved");
            $("#loader").hide();
        },
        function (error) {
            alert("Server error");
        }
    );

    /*adrackAPI.getTopCountries(new Date(), new Date(),
        function (result) {
            Init3DPieLeadComparision(result.Series, "Recieved");
            $("#loader").hide();
        },
        function (error) {
            alert("Server error");
        }
    );*/

    adrackAPI.getLeadsReport(new Date(), new Date(),
        function (result) {


            Init3DPieLeadComparision(result.Summary);


            InitLeadsGraph(result.Series,24);
            InitStockChart(result.Series,1);
            InitMapContainer();
            
            
            $("#loader").hide();
        }, function (error) {
            alert("Server error");
        });
    
}


