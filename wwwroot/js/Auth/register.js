document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById("registerForm");
    const emailInput = document.getElementById("email");
    const continueButton = document.getElementById("continueButton");

    const googleButton = document.getElementById("googleButton");
    const appleButton = document.getElementById("appleButton");
    const regionButton = document.getElementById("regionButton");


    // =====================================================
    // EMAIL VALIDATION
    // =====================================================

    if (form && emailInput) {

        form.addEventListener("submit", function (event) {

            const email = emailInput.value.trim();

            if (email === "") {

                event.preventDefault();

                emailInput.focus();

                showError("Please enter your email address.");

                return;
            }


            if (!isValidEmail(email)) {

                event.preventDefault();

                emailInput.focus();

                showError("Please enter a valid email address.");

                return;
            }


            // Hiệu ứng loading nhỏ
            if (continueButton) {

                continueButton.classList.add("loading");

                continueButton.querySelector("span").textContent =
                    "Continue...";

            }

        });

    }


    // =====================================================
    // EMAIL FORMAT
    // =====================================================

    function isValidEmail(email) {

        const pattern =
            /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        return pattern.test(email);
    }


    // =====================================================
    // ERROR MESSAGE
    // =====================================================

    function showError(message) {

        let error =
            document.querySelector(".register-error");

        if (!error) {

            error =
                document.createElement("div");

            error.className =
                "register-error";

            form.insertBefore(
                error,
                form.firstChild
            );
        }

        error.textContent = message;


        setTimeout(function () {

            if (error) {
                error.remove();
            }

        }, 3000);
    }


    // =====================================================
    // GOOGLE
    // =====================================================

    if (googleButton) {

        googleButton.addEventListener(
            "click",
            function () {

                console.log(
                    "Google registration selected."
                );

                // Sau này nối OAuth Google ở đây.
            }
        );
    }


    // =====================================================
    // APPLE
    // =====================================================

    if (appleButton) {

        appleButton.addEventListener(
            "click",
            function () {

                console.log(
                    "Apple registration selected."
                );

                // Sau này nối Sign in with Apple ở đây.
            }
        );
    }


    // =====================================================
    // REGION
    // =====================================================

    if (regionButton) {

        regionButton.addEventListener(
            "click",
            function () {

                console.log(
                    "Change region clicked."
                );

                // Sau này có thể mở region selector.
            }
        );
    }


    // =====================================================
    // REMOVE ERROR WHEN USER STARTS TYPING
    // =====================================================

    if (emailInput) {

        emailInput.addEventListener(
            "input",
            function () {

                const error =
                    document.querySelector(
                        ".register-error"
                    );

                if (error) {
                    error.remove();
                }

            }
        );
    }

});