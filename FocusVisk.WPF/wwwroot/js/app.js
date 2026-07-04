window.focusApp = {

    _dotNetRef: null,

    registerNavigationHandler: function (dotNetRef) {
        window.focusApp._dotNetRef = dotNetRef;
        window.addEventListener('navigate', function (e) {
            const page = e.detail?.page;
            if (page && window.focusApp._dotNetRef) {
                window.focusApp._dotNetRef.invokeMethodAsync('NavigateFromJs', page);
            }
        });
    },

    navigateTo: function (page) {
        window.dispatchEvent(new CustomEvent('navigate', { detail: { page } }));
    },

    playSound: function (type) {
        const ctx = new (window.AudioContext || window.webkitAudioContext)();
        const osc = ctx.createOscillator();
        const gain = ctx.createGain();
        osc.connect(gain);
        gain.connect(ctx.destination);

        const sounds = {
            'complete': { freq: 880, duration: 0.3 },
            'break': { freq: 660, duration: 0.2 },
            'tick': { freq: 440, duration: 0.05 },
        };

        const s = sounds[type] || sounds['complete'];
        osc.frequency.setValueAtTime(s.freq, ctx.currentTime);
        osc.frequency.exponentialRampToValueAtTime(s.freq * 0.8, ctx.currentTime + s.duration);
        gain.gain.setValueAtTime(0.3, ctx.currentTime);
        gain.gain.exponentialRampToValueAtTime(0.001, ctx.currentTime + s.duration);
        osc.start(ctx.currentTime);
        osc.stop(ctx.currentTime + s.duration);
    },

    focusElement: function (id) {
        const el = document.getElementById(id);
        if (el) el.focus();
    },

    scrollTo: function (id) {
        const el = document.getElementById(id);
        if (el) el.scrollIntoView({ behavior: 'smooth' });
    },

    setThemeVars: function (css) {
        let el = document.getElementById('theme-vars');
        if (!el) {
            el = document.createElement('style');
            el.id = 'theme-vars';
            document.head.appendChild(el);
        }
        el.textContent = css;
    },

    renderFinancaCharts: function (lineLabels, lineSaldos, donutLabels, donutValues) {
        const lineCanvas = document.getElementById('financa-line-chart');
        if (lineCanvas) {
            if (lineCanvas._chartInstance) lineCanvas._chartInstance.destroy();
            lineCanvas._chartInstance = new Chart(lineCanvas, {
                type: 'line',
                data: {
                    labels: lineLabels,
                    datasets: [{
                        label: 'Saldo',
                        data: lineSaldos,
                        borderColor: '#7C6AF7',
                        backgroundColor: 'rgba(124,106,247,0.08)',
                        borderWidth: 2,
                        pointBackgroundColor: '#7C6AF7',
                        pointRadius: 4,
                        tension: 0.4,
                        fill: true
                    }]
                },
                options: {
                    responsive: true,
                    plugins: { legend: { display: false } },
                    scales: {
                        x: { ticks: { color: '#5A5A72', font: { size: 11 } }, grid: { color: 'rgba(255,255,255,0.04)' } },
                        y: { ticks: { color: '#5A5A72', font: { size: 11 } }, grid: { color: 'rgba(255,255,255,0.04)' } }
                    }
                }
            });
        }

        const donutCanvas = document.getElementById('financa-donut-chart');
        if (donutCanvas && donutValues.length > 0) {
            if (donutCanvas._chartInstance) donutCanvas._chartInstance.destroy();
            donutCanvas._chartInstance = new Chart(donutCanvas, {
                type: 'doughnut',
                data: {
                    labels: donutLabels,
                    datasets: [{
                        data: donutValues,
                        backgroundColor: ['#F07070', '#F6C644', '#4ECDC4', '#7C6AF7', '#5DD68E', '#E87CA0', '#9D8FF7', '#F0A070', '#70C0F0'],
                        borderWidth: 0
                    }]
                },
                options: {
                    responsive: true,
                    cutout: '65%',
                    plugins: {
                        legend: {
                            position: 'bottom',
                            labels: { color: '#9090A8', font: { size: 11 }, boxWidth: 10, padding: 10 }
                        }
                    }
                }
            });
        }
    }

};
