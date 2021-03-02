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
    Show: function (element) {
        $(element).modal('show');
    },
    Hide: async function (element) {
        $(element).modal('hide');
    },
};

window.ElementReference = {
    Focus: function (element) {
        element.focus();
    },
    Mask: function (element, mask) {
        $(element).mask(mask);
    },
};