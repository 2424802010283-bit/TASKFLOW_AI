document.addEventListener("DOMContentLoaded", () => {

    const tabs = document.querySelectorAll(".context-tab");

    tabs.forEach(tab => {

        tab.addEventListener("click", () => {

            tabs.forEach(item => {
                item.classList.remove("active");
            });

            tab.classList.add("active");

        });

    });


    /* FlowBot button */
    const aiButton =
        document.querySelector(".context-ai-button");

    if (aiButton) {

        aiButton.addEventListener("click", () => {

            const input =
                document.querySelector(".context-input span");

            if (input) {
                input.textContent =
                    "FlowBot is ready to help...";
            }

        });

    }


    /* Animation button */
    const animationButton =
        document.querySelector(".context-animation button");

    const visual =
        document.querySelector(".context-visual");

    if (animationButton && visual) {

        let paused = false;

        animationButton.addEventListener("click", () => {

            paused = !paused;

            const floating =
                document.querySelectorAll(".floating-app");

            const sparkles =
                document.querySelectorAll(".context-spark");

            floating.forEach(item => {
                item.style.animationPlayState =
                    paused ? "paused" : "running";
            });

            sparkles.forEach(item => {
                item.style.animationPlayState =
                    paused ? "paused" : "running";
            });

            animationButton.textContent =
                paused ? "▶" : "Ⅱ";

        });

    }

});
