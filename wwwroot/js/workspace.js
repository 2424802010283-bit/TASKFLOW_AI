// script.js
const $ = id => document.getElementById(id);
const pad = n => String(n).padStart(2, '0');

// Gửi tin nhắn trong kênh chính
function send() {
    const el = $('cin');
    const t = el.textContent.trim();
    if (!t) return;

    const d = new Date();
    const m = document.createElement('div');
    m.className = 'msg';
    m.innerHTML =
        '<div class="av2"></div>' +
        '<div><div class="n">THANHGIA <small>' + pad(d.getHours()) + ' h ' + pad(d.getMinutes()) + '</small></div>' +
        '<div class="tx"></div></div>';
    m.querySelector('.tx').textContent = t; // dùng textContent để tránh chèn HTML
    $('msgs').appendChild(m);
    el.textContent = '';

    const s = $('scroll');
    s.scrollTop = s.scrollHeight;
}

// Enter để gửi, Shift+Enter để xuống dòng
$('cin').addEventListener('keydown', e => {
    if (e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault();
        send();
    }
});

// Nút gửi (➤)
$('snd').onclick = send;

// Gửi tin nhắn trong panel Slackbot
$('bin').addEventListener('keydown', e => {
    if (e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault();
        const t = e.target.textContent.trim();
        if (!t) return;

        const p = document.createElement('p');
        p.textContent = 'Vous : ' + t;
        $('btxt').appendChild(p);
        e.target.textContent = '';
        $('bsc').scrollTop = $('bsc').scrollHeight;
    }
});

// Đổi kênh ở sidebar
document.querySelectorAll('[data-ch]').forEach(el => {
    el.onclick = () => {
        document.querySelectorAll('.it.on').forEach(x => x.classList.remove('on'));
        el.classList.add('on');
        $('chname').textContent = '# ' + el.dataset.ch + ' ✎';
        $('cin').dataset.ph = 'Envoyer un message #' + el.dataset.ch;
    };
});