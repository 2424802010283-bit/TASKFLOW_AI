document.addEventListener("DOMContentLoaded", () => {

    const section = document.querySelector("#deep-dive");

    if (!section) return;


    /* =====================================================
       NUMBER COUNTER
    ===================================================== */

    const counters = section.querySelectorAll("[data-count]");

    let hasStarted = false;

    const animateCounters = () => {

        if (hasStarted) return;

        hasStarted = true;

        counters.forEach(counter => {

            const target = parseFloat(
                counter.dataset.count
            );

            const suffix =
                counter.dataset.suffix || "";

            const duration = 1200;

            const startTime = performance.now();

            const update = currentTime => {

                const elapsed =
                    currentTime - startTime;

                const progress =
                    Math.min(elapsed / duration, 1);

                const eased =
                    1 - Math.pow(1 - progress, 3);

                const value =
                    target * eased;

                let displayValue;

                if (target % 1 !== 0) {
                    displayValue =
                        value.toFixed(1);
                } else {
                    displayValue =
                        Math.floor(value);
                }

                counter.textContent =
                    displayValue + suffix;

                if (progress < 1) {
                    requestAnimationFrame(update);
                } else {
                    counter.textContent =
                        target + suffix;
                }
            };

            requestAnimationFrame(update);
        });
    };


    /* =====================================================
       INTERSECTION OBSERVER
    ===================================================== */

    const observer =
        new IntersectionObserver(
            entries => {

                entries.forEach(entry => {

                    if (
                        entry.isIntersecting
                    ) {
                        animateCounters();

                        observer.unobserve(
                            entry.target
                        );
                    }

                });

            },
            {
                threshold: 0.25
            }
        );

    observer.observe(section);


    /* =====================================================
       CARD HOVER
    ===================================================== */

    const cards =
        section.querySelectorAll(
            ".deep-dive-card"
        );

    cards.forEach(card => {

        card.addEventListener(
            "mouseenter",
            () => {
                card.classList.add(
                    "is-hovered"
                );
            }
        );

        card.addEventListener(
            "mouseleave",
            () => {
                card.classList.remove(
                    "is-hovered"
                );
            }
        );

    });


    /* =====================================================
       KEYBOARD ACCESSIBILITY
    ===================================================== */

    cards.forEach(card => {

        card.setAttribute(
            "tabindex",
            "0"
        );

        card.addEventListener(
            "focus",
            () => {
                card.classList.add(
                    "is-hovered"
                );
            }
        );

        card.addEventListener(
            "blur",
            () => {
                card.classList.remove(
                    "is-hovered"
                );
            }
        );

    });

});