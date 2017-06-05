(function ($) {
    "use strict";
    $(document).ready(function () {
        /*-------------------------------------
         Mobile menu
         -------------------------------------*/
        $('#menu').slicknav();

        /*-------------------------------------
         Scroll Menu Background Color
         -------------------------------------*/

        $(window).on('scroll', function () {
            if ($(window).scrollTop() > 100) {
                $('.header-area').addClass('menu-bg');
            } else {
                $('.header-area').removeClass('menu-bg');
            }
        });

        /*-------------------------------------
         Smooth scroll
         -------------------------------------*/

        $('a.smooth-scroll').on('click', function (e) {
            var anchor = $(this);
            $('html, body').stop().animate({
                scrollTop: $(anchor.attr('href')).offset().top
            }, 1000);
            e.preventDefault();
        });

        /*-------------------------------------
         Circle Progress
         -------------------------------------*/

        function animateProgressBar() {
            // First circle Progress
            $('.first.circle').circleProgress({
                value: 0.75,
                size: 150,
                startAngle: 3,
                thickness: 5,
                emptyFill: "#000",
                animation: {duration: 2200},
                fill:
                {
                    color : "#D63138"
                }
            }).on('circle-animation-progress', function (event, progress) {
                $(this).find('strong').html(Math.round(75 * progress) + '<i>%</i>');
            });
            // Second circle Progress
            $('.second.circle').circleProgress({
                value: 0.80,
                size: 150,
                startAngle: 3,
                thickness: 5,
                emptyFill: "#000",
                animation: {duration: 2200},
                fill:
                {
                    color : "#D63138"
                }
            }).on('circle-animation-progress', function (event, progress) {
                $(this).find('strong').html(Math.round(80 * progress) + '<i>%</i>');
            });
            // Third circle Progress
            $('.third.circle').circleProgress({
                value: 1,
                size: 150,
                startAngle: 3,
                thickness: 5,
                emptyFill: "#000",
                animation: {duration: 2200},
                fill:
                {
                    color : "#D63138"
                }
            }).on('circle-animation-progress', function (event, progress) {
                $(this).find('strong').html(Math.round(100 * progress) + '<i>%</i>');
            });
        }

        var waypoint = new Waypoint({
            element: document.getElementById('progress'),
            handler: function (direction) {
                animateProgressBar();
                this.destroy();
            },
            offset: '75%'
        });

        /*-------------------------------------
         WOW js
         -------------------------------------*/

        new WOW().init();

        /*-------------------------------------
         Slick Slider
         -------------------------------------*/

        $('.schedule, .client-part').slick({
            autoplay: true
        });

        /*-------------------------------------
         Scroll Top
         -------------------------------------*/

        $.scrollUp({
            scrollName: 'scrollUp', // Element ID
            topDistance: '300', // Distance from top before showing element (px)
            topSpeed: 300, // Speed back to top (ms)
            animation: 'fade', // Fade, slide, none
            animationInSpeed: 200, // Animation in speed (ms)
            animationOutSpeed: 200, // Animation out speed (ms)
            scrollText: '<a class="hvr-icon-bob click-top"></a>', // Text for element
            activeOverlay: false // Set CSS color to display scrollUp active point, e.g '#00FFFF'
        });
    });

    $(window).on('load', function () {
        /*-------------------------------------
         preloader
         -------------------------------------*/

        $('.preloader-area').fadeOut('500');

        /*-------------------------------------
         parallax
         -------------------------------------*/

        $('.fityourself-area').parallax("50%", 0.7);

        /*-------------------------------------
         Portfolio filter
         -------------------------------------*/

        // Active isotope with jQuery code

        $('.iso-content').isotope({
            itemSelector: '.single-project',
            layoutMode: 'fitRows'
        });

        // Isotope click function

        $('.iso-nav ul li').click(function () {
            $('.iso-nav ul li').removeClass('portfolio-active');
            $(this).addClass('portfolio-active');
            var selector = $(this).attr('data-filter');
            $('.iso-content').isotope({
                filter: selector
            });
            return false;
        });
    });
}(jQuery));