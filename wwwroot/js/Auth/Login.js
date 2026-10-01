document.addEventListener("DOMContentLoaded", function () {

    const loginForm = document.getElementById("loginForm");
    const emailInput = document.getElementById("email");

    if (!loginForm) {
        return;
    }

    loginForm.addEventListener("submit", function (event) {

        event.preventDefault();

        const email = emailInput.value.trim();

        if (email === "") {
            emailInput.focus();
            return;
        }

        if (!emailInput.checkValidity()) {
            emailInput.focus();
            return;
        }

        console.log("Login email:", email);

        // Sau này nối với ASP.NET MVC Controller tại đây.
        // Ví dụ:
        // window.location.href = "/Auth/LoginPassword?email="
        //     + encodeURIComponent(email);
    });


    /* Social buttons */

    const socialButtons =
        document.querySelectorAll(".social-btn");

    socialButtons.forEach(function (button) {

        button.addEventListener("click", function () {

            const provider =
                button.innerText.trim();

            console.log(
                "Selected login provider:",
                provider
            );

            // Sau này tích hợp Google / Apple OAuth ở đây.
        });

    });

});
document.addEventListener("DOMContentLoaded", function () {

    const loginForm = document.getElementById("loginForm");

    if (loginForm) {

        loginForm.addEventListener("submit", function (event) {

            event.preventDefault();

            window.location.href = "/Home/Workspace";

        });

    }

});