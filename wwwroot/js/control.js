let control_panel = this.document.getElementById('control-panel');
let startControlPosY = control_panel.getBoundingClientRect().top;

window.addEventListener('wheel', async function () {


    if (this.window.scrollY > control_panel.getBoundingClientRect().top) {
        control_panel.style.top = this.window.scrollY;
    }
    else {
        control_panel.style.top = startControlPosY;
    }
});