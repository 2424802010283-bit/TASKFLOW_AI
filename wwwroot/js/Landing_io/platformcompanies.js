document.addEventListener("DOMContentLoaded", () => {
    /* ---------- Company cards: hover to expand ---------- */
    const stage = document.querySelector("[data-company-stage]");
    if (stage) {
        const cards = [...stage.querySelectorAll("[data-company-card]")];

        const activateCard = card => {
            cards.forEach(item => item.classList.remove("is-hovered"));
            card.classList.add("is-hovered");
            stage.classList.add("has-hover");
        };

        const resetCards = () => {
            cards.forEach(item => item.classList.remove("is-hovered"));
            stage.classList.remove("has-hover");
        };

        cards.forEach(card => {
            card.setAttribute("tabindex", "0");

            // mouseenter (not mouseover) so child elements don't retrigger it
            card.addEventListener("mouseenter", () => activateCard(card));
            card.addEventListener("focus", () => activateCard(card));

            card.addEventListener("keydown", e => {
                if (e.key === "Enter" || e.key === " ") {
                    e.preventDefault();
                    activateCard(card);
                }
            });

            // Touch devices: tap to expand
            card.addEventListener("click", () => {
                if (window.matchMedia("(hover: none)").matches) activateCard(card);
            });
        });

        // Leaving the whole stage returns to the default (featured card wide)
        stage.addEventListener("mouseleave", resetCards);
        stage.addEventListener("focusout", e => {
            if (!stage.contains(e.relatedTarget)) resetCards();
        });
    }

    /* ---------- Count-up numbers (0 -> target) ---------- */
    const counters = [...document.querySelectorAll("[data-count-to]")];
    const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    const render = (el, value) => {
        el.textContent = Math.round(value) + (el.dataset.suffix || "");
    };

    const easeOutCubic = t => 1 - Math.pow(1 - t, 3);

    const runCounter = el => {
        const target = parseFloat(el.dataset.countTo);
        const duration = parseInt(el.dataset.duration || "2000", 10);

        if (reduceMotion) {
            render(el, target);
            return;
        }

        const start = performance.now();
        const tick = now => {
            const progress = Math.min((now - start) / duration, 1);
            render(el, target * easeOutCubic(progress));
            if (progress < 1) requestAnimationFrame(tick);
        };
        requestAnimationFrame(tick);
    };

    // Show 0 first, then start counting when the section scrolls into view (once)
    counters.forEach(el => render(el, 0));

    if ("IntersectionObserver" in window) {
        const observer = new IntersectionObserver((entries, obs) => {
            entries.forEach(entry => {
                if (!entry.isIntersecting) return;
                runCounter(entry.target);
                obs.unobserve(entry.target);
            });
        }, { threshold: 0.4 });

        counters.forEach(el => observer.observe(el));
    } else {
        counters.forEach(runCounter);
    }

    /* ---------- Tab switcher ---------- */
    const tabs = [...document.querySelectorAll(".platform-switcher__item")];
    tabs.forEach(tab => {
        tab.addEventListener("click", () => {
            tabs.forEach(t => t.classList.remove("is-active"));
            tab.classList.add("is-active");
        });
    });
});