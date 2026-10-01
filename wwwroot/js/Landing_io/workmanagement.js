document.addEventListener("DOMContentLoaded", () => {
    const section = document.querySelector("#work-management");
    if (!section) return;

    const stories = section.querySelectorAll("[data-wm-story]");
    const tabs = section.querySelectorAll("[data-wm-tab]");

    stories.forEach(item => {
        item.addEventListener("click", () => {
            stories.forEach(story => story.classList.remove("wm-selected"));
            item.classList.add("wm-selected");
        });
    });

    tabs.forEach(tab => {
        tab.addEventListener("click", () => {
            tabs.forEach(button => button.classList.remove("wm-active"));
            tab.classList.add("wm-active");
        });
    });

    const addButtons = section.querySelectorAll(".wm-add, .wm-floating-add");
    addButtons.forEach(button => {
        button.addEventListener("click", () => {
            button.animate(
                [
                    { transform: "scale(1)" },
                    { transform: "scale(.96)" },
                    { transform: "scale(1)" }
                ],
                { duration: 180 }
            );
        });
    });
});
