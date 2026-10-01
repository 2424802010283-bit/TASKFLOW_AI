/* =========================================================
TASKFLOW COLLABORATION JS
========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    const features =
        document.querySelectorAll(".tf-collab-feature");

    const floatingApps =
        document.querySelectorAll(".tf-floating-app");


    /* =====================================================
       FEATURE ACTIVE
       ===================================================== */

    features.forEach(function (feature) {

        feature.addEventListener("click", function () {

            features.forEach(function (item) {
                item.classList.remove("active");
            });

            feature.classList.add("active");

        });

    });


    /* =====================================================
       FLOATING APP PARALLAX
       ===================================================== */

    let mouseX = 0;
    let mouseY = 0;

    document.addEventListener("mousemove", function (event) {

        mouseX =
            (event.clientX / window.innerWidth - 0.5);

        mouseY =
            (event.clientY / window.innerHeight - 0.5);

    });


    function animateFloatingApps() {

        floatingApps.forEach(function (app, index) {

            const multiplier =
                index === 0 ? 8 :
                    index === 1 ? -6 :
                        5;

            const x =
                mouseX * multiplier;

            const y =
                mouseY * multiplier;

            app.style.transform =
                `translate(${x}px, ${y}px)`;

        });

        requestAnimationFrame(animateFloatingApps);
    }

    animateFloatingApps();


    /* =====================================================
       SCROLL REVEAL
       ===================================================== */

    const observer =
        new IntersectionObserver(
            function (entries) {

                entries.forEach(function (entry) {

                    if (entry.isIntersecting) {

                        entry.target.classList.add(
                            "tf-collab-visible"
                        );

                    }

                });

            },
            {
                threshold: 0.15
            }
        );


    const section =
        document.querySelector(".tf-collaboration");

    if (section) {
        observer.observe(section);
    }

});
