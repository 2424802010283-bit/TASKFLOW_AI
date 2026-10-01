document.addEventListener("DOMContentLoaded", function () {

    /* =========================================================
       ELEMENTS
    ========================================================= */

    const modal =
        document.getElementById("channelModal");

    const addChannelButton =
        document.getElementById("addChannelButton");

    const addChannelRow =
        document.getElementById("addChannelRow");

    const closeButton =
        document.getElementById("channelModalClose");

    const step1 =
        document.getElementById("channelStep1");

    const step2 =
        document.getElementById("channelStep2");

    const step3 =
        document.getElementById("channelStep3");

    const channelNameInput =
        document.getElementById("channelNameInput");

    const channelNameCounter =
        document.getElementById("channelNameCounter");

    const channelNextStep =
        document.getElementById("channelNextStep");

    const channelBackStep =
        document.getElementById("channelBackStep");

    const channelCreateButton =
        document.getElementById("channelCreateButton");

    const channelPreviewName =
        document.getElementById("channelPreviewName");

    const channelPeopleSearch =
        document.getElementById("channelPeopleSearch");

    const channelPeopleResults =
        document.getElementById("channelPeopleResults");

    const channelSelectedPeople =
        document.getElementById("channelSelectedPeople");

    const skipChannelPeople =
        document.getElementById("skipChannelPeople");

    const finishChannelButton =
        document.getElementById("finishChannelButton");

    const channelList =
        document.getElementById("channelList");


    /* =========================================================
       STATE
    ========================================================= */

    let currentStep = 1;

    let currentChannel = {

        name: "",

        isPrivate: false,

        selectedPeople: []

    };

    let peopleSearchTimer = null;


    /* =========================================================
       OPEN MODAL
    ========================================================= */

    function openChannelModal() {

        resetChannelFlow();

        modal?.classList.remove("hidden");

        channelNameInput?.focus();

    }


    addChannelButton?.addEventListener(
        "click",
        openChannelModal
    );


    addChannelRow?.addEventListener(
        "click",
        openChannelModal
    );


    /* =========================================================
       CLOSE MODAL
    ========================================================= */

    function closeChannelModal() {

        modal?.classList.add("hidden");

    }


    closeButton?.addEventListener(
        "click",
        closeChannelModal
    );


    document.querySelectorAll(
        '[data-action="close-channel"]'
    ).forEach(function (button) {

        button.addEventListener(
            "click",
            closeChannelModal
        );

    });


    modal?.addEventListener(
        "click",
        function (event) {

            if (
                event.target === modal
            ) {

                closeChannelModal();

            }

        }
    );


    /* =========================================================
       STEP SWITCH
    ========================================================= */

    function showStep(step) {

        currentStep = step;

        [step1, step2, step3]
            .filter(Boolean)
            .forEach(function (element) {

                element.classList.remove(
                    "active"
                );

            });


        if (step === 1) {

            step1?.classList.add(
                "active"
            );

        }


        if (step === 2) {

            step2?.classList.add(
                "active"
            );

        }


        if (step === 3) {

            step3?.classList.add(
                "active"
            );

        }

    }


    /* =========================================================
       STEP 1 → STEP 2
    ========================================================= */

    channelNextStep?.addEventListener(
        "click",
        function () {

            const name =
                channelNameInput.value.trim();


            if (!name) {

                channelNameInput.focus();

                channelNameInput.classList.add(
                    "channel-invalid"
                );

                return;

            }


            channelNameInput.classList.remove(
                "channel-invalid"
            );


            currentChannel.name =
                normalizeChannelName(name);


            channelPreviewName.textContent =
                `# ${currentChannel.name}`;


            showStep(2);

        }
    );


    /* =========================================================
       NAME COUNTER
    ========================================================= */

    channelNameInput?.addEventListener(
        "input",
        function () {

            const remaining =
                80 -
                channelNameInput.value.length;


            channelNameCounter.textContent =
                remaining;

        }
    );


    /* =========================================================
       NAME CLEANUP
    ========================================================= */

    function normalizeChannelName(
        value
    ) {

        return value
            .trim()
            .toLowerCase()
            .replace(/\s+/g, "-")
            .replace(/[^a-z0-9-_]/g, "");

    }


    /* =========================================================
       STEP 2 → STEP 1
    ========================================================= */

    channelBackStep?.addEventListener(
        "click",
        function () {

            showStep(1);

        }
    );


    /* =========================================================
       STEP 2 CREATE
    ========================================================= */

    channelCreateButton?.addEventListener(
        "click",
        function () {

            const selectedVisibility =
                document.querySelector(
                    'input[name="channelVisibility"]:checked'
                );


            currentChannel.isPrivate =
                selectedVisibility?.value ===
                "private";


            /*
             * Chuyển sang bước 3.
             * Chưa gọi database ở đây vì code hiện tại
             * của project mới chỉ xác nhận DbSet<Channel>,
             * chưa có Channel entity/property trong phần
             * code được cung cấp.
             */

            renderPeopleTitle();

            showStep(3);

        }
    );


    /* =========================================================
       STEP 3
    ========================================================= */

    function renderPeopleTitle() {

        const target =
            document.getElementById(
                "channelPeopleTitle"
            );


        if (!target) {
            return;
        }


        target.textContent =
            `Add people or apps to #${currentChannel.name}`;

    }


    /* =========================================================
       SEARCH PEOPLE
    ========================================================= */

    channelPeopleSearch?.addEventListener(
        "input",
        function () {

            const query =
                channelPeopleSearch.value.trim();


            clearTimeout(
                peopleSearchTimer
            );


            if (query.length < 2) {

                channelPeopleResults.innerHTML =
                    "";

                return;

            }


            peopleSearchTimer =
                setTimeout(
                    function () {

                        searchPeople(
                            query
                        );

                    },
                    300
                );

        }
    );


    async function searchPeople(
        query
    ) {

        channelPeopleResults.innerHTML = `
            <div class="channel-search-loading">
                Searching...
            </div>
        `;


        try {

            const response =
                await fetch(
                    `/Contact/SearchUser?query=${encodeURIComponent(query)}`
                );


            if (!response.ok) {

                throw new Error(
                    `HTTP ${response.status}`
                );

            }


            const result =
                await response.json();


            channelPeopleResults.innerHTML =
                "";


            if (
                !result.success ||
                !result.data ||
                result.data.length === 0
            ) {

                channelPeopleResults.innerHTML = `
                    <div class="channel-search-loading">
                        No people found.
                    </div>
                `;

                return;

            }


            result.data.forEach(
                function (user) {

                    renderPerson(
                        user
                    );

                }
            );

        }
        catch (error) {

            console.error(
                "Channel people search:",
                error
            );


            channelPeopleResults.innerHTML = `
                <div class="channel-search-loading">
                    Unable to search.
                </div>
            `;

        }

    }


    /* =========================================================
       RENDER PERSON
    ========================================================= */

    function renderPerson(
        user
    ) {

        const userId =
            Number(
                user.id ??
                user.userId ??
                user.UserId
            );


        if (!userId) {
            return;
        }


        const alreadySelected =
            currentChannel.selectedPeople
                .some(function (person) {

                    return (
                        Number(person.id) ===
                        userId
                    );

                });


        if (alreadySelected) {
            return;
        }


        const button =
            document.createElement(
                "button"
            );


        button.type = "button";

        button.className =
            "channel-person";


        button.innerHTML = `

            <span class="channel-person-avatar">
                ${escapeHtml(
            getInitials(
                user.fullName,
                user.email
            )
        )}
            </span>

            <span class="channel-person-copy">

                <strong>
                    ${escapeHtml(
            user.fullName ||
            "User"
        )}
                </strong>

                <span>
                    ${escapeHtml(
            user.email ||
            ""
        )}
                </span>

            </span>

            <span class="channel-person-add">
                +
            </span>
        `;


        button.addEventListener(
            "click",
            function () {

                currentChannel.selectedPeople.push({

                    id: userId,

                    fullName:
                        user.fullName ||
                        "User",

                    email:
                        user.email ||
                        ""

                });


                channelPeopleSearch.value =
                    "";

                channelPeopleResults.innerHTML =
                    "";

                renderSelectedPeople();

            }
        );


        channelPeopleResults.appendChild(
            button
        );

    }


    /* =========================================================
       SELECTED PEOPLE
    ========================================================= */

    function renderSelectedPeople() {

        channelSelectedPeople.innerHTML =
            "";


        currentChannel.selectedPeople
            .forEach(
                function (person) {

                    const chip =
                        document.createElement(
                            "div"
                        );


                    chip.className =
                        "channel-person-chip";


                    chip.innerHTML = `

                        <span>
                            ${escapeHtml(
                        person.fullName
                    )}
                        </span>

                        <button
                            type="button"
                            aria-label="Remove">
                            ×
                        </button>
                    `;


                    chip.querySelector(
                        "button"
                    ).addEventListener(
                        "click",
                        function () {

                            currentChannel.selectedPeople =
                                currentChannel.selectedPeople
                                    .filter(
                                        function (item) {

                                            return (
                                                Number(item.id) !==
                                                Number(person.id)
                                            );

                                        }
                                    );


                            renderSelectedPeople();

                        }
                    );


                    channelSelectedPeople.appendChild(
                        chip
                    );

                }
            );

    }


    /* =========================================================
       COMPLETE
    ========================================================= */

    skipChannelPeople?.addEventListener(
        "click",
        function () {

            finishCreateChannel();

        }
    );


    finishChannelButton?.addEventListener(
        "click",
        function () {

            finishCreateChannel();

        }
    );


    function finishCreateChannel() {

        /*
         * UI stage:
         * tạo channel ngay trên sidebar.
         *
         * Sau khi Channel.cs / schema hiện tại được xác định,
         * block này sẽ gọi POST /Channel/Create.
         */

        addChannelToSidebar(
            currentChannel.name
        );


        selectChannel(
            currentChannel.name
        );


        closeChannelModal();

        resetChannelFlow();

    }


    /* =========================================================
       ADD CHANNEL TO SIDEBAR
    ========================================================= */

    function addChannelToSidebar(
        channelName
    ) {

        if (!channelList) {
            return;
        }


        const existing =
            channelList.querySelector(
                `[data-ch="${cssEscape(channelName)}"]`
            );


        if (existing) {
            return;
        }


        const row =
            document.createElement(
                "button"
            );


        row.type = "button";

        row.className =
            "it";

        row.dataset.ch =
            channelName;


        row.innerHTML = `

            <b>#</b>

            <span>
                ${escapeHtml(
            channelName
        )}
            </span>

        `;


        row.addEventListener(
            "click",
            function () {

                document.querySelectorAll(
                    "[data-ch]"
                ).forEach(
                    function (item) {

                        item.classList.remove(
                            "on"
                        );

                    }
                );


                row.classList.add(
                    "on"
                );


                selectChannel(
                    channelName
                );

            }
        );


        channelList.insertBefore(
            row,
            document.getElementById(
                "addChannelRow"
            )
        );

    }


    /* =========================================================
       SELECT CHANNEL
    ========================================================= */

    function selectChannel(
        channelName
    ) {

        document.querySelectorAll(
            "[data-ch]"
        ).forEach(
            function (item) {

                item.classList.toggle(
                    "on",
                    item.dataset.ch ===
                    channelName
                );

            }
        );


        const channelHeader =
            document.getElementById(
                "chname"
            );


        if (channelHeader) {

            channelHeader.textContent =
                `# ${channelName} ✎`;

        }


        const composer =
            document.getElementById(
                "cin"
            );


        if (composer) {

            composer.dataset.ph =
                `Send a message #${channelName}`;

        }

    }


    /* =========================================================
       RESET
    ========================================================= */

    function resetChannelFlow() {

        currentStep = 1;


        currentChannel = {

            name: "",

            isPrivate: false,

            selectedPeople: []

        };


        if (channelNameInput) {

            channelNameInput.value =
                "";

        }


        if (channelNameCounter) {

            channelNameCounter.textContent =
                "80";

        }


        if (channelPeopleSearch) {

            channelPeopleSearch.value =
                "";

        }


        if (channelPeopleResults) {

            channelPeopleResults.innerHTML =
                "";

        }


        if (channelSelectedPeople) {

            channelSelectedPeople.innerHTML =
                "";

        }


        showStep(1);

    }


    /* =========================================================
       TOOLTIP FOR DYNAMIC / EXISTING ICONS
    ========================================================= */

    document
        .querySelectorAll(
            "[data-tooltip]"
        )
        .forEach(
            function (element) {

                element.addEventListener(
                    "mouseenter",
                    function () {

                        /*
                         * CSS handles the tooltip.
                         * This event exists intentionally so
                         * dynamically added controls can later
                         * share the same behavior.
                         */

                    }
                );

            }
        );


    /* =========================================================
       HELPERS
    ========================================================= */

    function getInitials(
        fullName,
        email = ""
    ) {

        const name =
            (fullName || "").trim();


        if (!name) {

            return (
                email ||
                "US"
            )
                .substring(
                    0,
                    2
                )
                .toUpperCase();

        }


        const words =
            name.split(/\s+/);


        if (
            words.length >= 2
        ) {

            return (
                words[0][0] +
                words[words.length - 1][0]
            ).toUpperCase();

        }


        return name
            .substring(
                0,
                2
            )
            .toUpperCase();

    }


    function escapeHtml(
        value
    ) {

        const div =
            document.createElement(
                "div"
            );


        div.textContent =
            value ??
            "";


        return div.innerHTML;

    }


    function cssEscape(
        value
    ) {

        if (
            window.CSS &&
            CSS.escape
        ) {

            return CSS.escape(
                value
            );

        }


        return value.replace(
            /["\\]/g,
            "\\$&"
        );

    }


    /* =========================================================
       INITIALIZE
    ========================================================= */

    showStep(1);

});