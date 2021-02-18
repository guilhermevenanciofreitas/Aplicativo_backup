window.LongPress = {
    Timer: null,
    MouseUp: function () {
        clearTimeout(Timer);
    },
    MouseDown: async function (Time) {
        return await new Promise(resolve => {
            Timer = setTimeout(() => resolve(true), Time);
        });
    },
};

window.Modal = {
    Show: function (divModal) {
        $(divModal).modal('show');
    },
    Hide: async function (divModal) {
        $(divModal).modal('hide');
    },
};