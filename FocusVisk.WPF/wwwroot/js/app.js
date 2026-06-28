window.focusApp = {
  navigateTo: function(page) {
    window.dispatchEvent(new CustomEvent('navigate', { detail: { page } }));
  },

  playSound: function(type) {
    const ctx = new (window.AudioContext || window.webkitAudioContext)();
    const osc = ctx.createOscillator();
    const gain = ctx.createGain();
    osc.connect(gain);
    gain.connect(ctx.destination);

    const sounds = {
      'complete': { freq: 880, duration: 0.3 },
      'break':    { freq: 660, duration: 0.2 },
      'tick':     { freq: 440, duration: 0.05 },
    };

    const s = sounds[type] || sounds['complete'];
    osc.frequency.setValueAtTime(s.freq, ctx.currentTime);
    osc.frequency.exponentialRampToValueAtTime(s.freq * 0.8, ctx.currentTime + s.duration);
    gain.gain.setValueAtTime(0.3, ctx.currentTime);
    gain.gain.exponentialRampToValueAtTime(0.001, ctx.currentTime + s.duration);
    osc.start(ctx.currentTime);
    osc.stop(ctx.currentTime + s.duration);
  },

  focusElement: function(id) {
    const el = document.getElementById(id);
    if (el) el.focus();
    },

  scrollTo: function(id) {
    const el = document.getElementById(id);
    if (el) el.scrollIntoView({ behavior: 'smooth' });
  },

  setThemeVars: function(css) {
    let el = document.getElementById('theme-vars');
    if (!el) {
      el = document.createElement('style');
      el.id = 'theme-vars';
      document.head.appendChild(el);
    }
    el.textContent = css;
  }
};
