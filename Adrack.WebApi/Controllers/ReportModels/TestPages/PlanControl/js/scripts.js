/*!
    * Start Bootstrap - SB Admin v6.0.1 (https://startbootstrap.com/templates/sb-admin)
    * Copyright 2013-2020 Start Bootstrap
    * Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-sb-admin/blob/master/LICENSE)
    */
    (function($) {
    "use strict";

    // Add active state to sidbar nav links
    var path = window.location.href; // because the 'href' property of the DOM element is the absolute path
        $("#layoutSidenav_nav .sb-sidenav a.nav-link").each(function() {
            if (this.href === path) {
                // $(this).addClass("active");
            }
        });

    // Toggle the side navigation
    $("#sidebarToggle").on("click", function(e) {
        e.preventDefault();
        $("body").toggleClass("sb-sidenav-toggled");
    });
        $('.motors-risk a').on('click', function () {
            var site = $(this).html();
            SelectedAccountName = site;
            clinSoftDataViewer.Draw();
            $('#motors-risk-title').html(site);

    });
    $('.buttons button').on('click',function () {
        $(this).parent().find('.active').removeClass('active');
        $(this).addClass('active');
        let title = $(this).parent().data('title');
        let value = $(this).data('value');
        console.log(title, value);
        InitLeadsGraph(null,parseInt(value));
        //UpdateValueAtRisk(value);
    });
})(jQuery);
