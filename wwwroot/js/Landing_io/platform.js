/* =========================================================
   TASKFLOW — PLATFORM SECTION
   File: wwwroot/js/Landing_io/_platform.js
========================================================= */
document.addEventListener('DOMContentLoaded', () => {
    const section = document.querySelector('#platform-section');
    if (!section) return;

    const tabs = [...section.querySelectorAll('[data-platform-tab]')];
    const stories = [...section.querySelectorAll('[data-platform-story]')];

    const storyContent = {
        integrations: {
            title: 'Connect the tools you already use.',
            text: 'Bring files, meetings, project data, and everyday tools into one connected workspace.'
        },
        customize: {
            title: 'Customize TaskFlow to fit your needs.',
            text: 'Shape channels, projects, permissions, and workflows around the way your team actually works.'
        },
        reliable: {
            title: 'Work with confidence.',
            text: 'Keep important work organized with clear ownership, shared context, and controlled access.'
        }
    };

    function selectStory(key) {
        stories.forEach(item => {
            const active = item.dataset.platformStory === key;
            item.classList.toggle('is-active', active);

            const strong = item.querySelector('strong');
            const small = item.querySelector('small');

            if (active && storyContent[key]) {
                if (strong) strong.textContent = storyContent[key].title;
                if (small) small.textContent = storyContent[key].text;
            }
        });
    }

    stories.forEach(item => {
        item.addEventListener('click', () => selectStory(item.dataset.platformStory));
    });

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            tabs.forEach(t => t.classList.remove('is-active'));
            tab.classList.add('is-active');
        });
    });

    section.querySelectorAll('.platform-mini-nav button').forEach(button => {
        button.addEventListener('click', () => {
            section.querySelectorAll('.platform-mini-nav button').forEach(item => item.classList.remove('active'));
            button.classList.add('active');
        });
    });

    section.querySelectorAll('.platform-app-card button').forEach(button => {
        button.addEventListener('click', event => {
            event.stopPropagation();
            const card = button.closest('.platform-app-card');
            if (!card || card.classList.contains('flowbot-card')) return;

            button.textContent = 'Connected';
            button.classList.add('is-connected');
            button.disabled = true;
        });
    });
});
