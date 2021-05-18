window.Loading =
{
    Show: function (text) {
        document.getElementsByClassName("loading")[0].style.display = "block";
        document.getElementsByClassName("loading-text")[0].innerHTML = text;
    },
    Hide: function () {
        document.getElementsByClassName("loading")[0].style.display = "none";
        document.getElementsByClassName("loading-text")[0].innerHTML = "";
    },
}

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
    SetValue: function (element, value) {
        element.value = value;
    },
    GetValue: function (element) {
        return element.value;
    },
    Focus: function (element) {
        element.focus();
    },
    SetSelectionRange: function (element, start, end) {
        element.setSelectionRange(start, end);
    },
    Mask: function (element, mask) {
        $(element).mask(mask);
    },
    MaskNumber: function (element, prefix, digits) {
        $(element).inputmask('currency', {

            prefix: prefix,
            digits: digits,

            groupSeparator: '.',
            radixPoint: ',',

            rightAlign: true,

        });
    },
    MaskPlaca: function (element) {

        $(element).mask('AAA-0U00', {
            translation: {
                'A': { pattern: /[A-Za-z]/ },
                'U': { pattern: /[A-Za-z0-9]/ },
            },
            onKeyPress: function (value, e, field, options) {
                e.currentTarget.value = value.toUpperCase();
                let val = value.replace(/[^\w]/g, '');
                let isNumeric = !isNaN(parseFloat(val[4])) && isFinite(val[4]);
                let mask = 'AAA-0U00';
                if (val.length > 4 && isNumeric) {
                    mask = 'AAA-0000';
                }
                $(field).mask(mask, options);
            }
        });

    },

};

window.Cookie =
{
    set: function (cname, cvalue, minutes) {

        var expires = "";

        if (minutes == 0)
        {
            expires = "expires=" + new Date(2147483647 * 1000).toUTCString();
        }
        else
        {
            var d = new Date();
            d.setTime(d.getTime() + (minutes * 60 * 1000));
            expires = "expires=" + d.toUTCString();
        }

        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";

    },
    get: function (cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },
    delete: function (cname) {
        document.cookie = cname + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;path=/;';
    },
}

window.Location =
{
    getPathName: function () {
        return window.location.pathname;
    },
}

window.Menu =
{
    AfterRender: function () {

        var accordions = document.getElementsByClassName("Menu-Accordion");
        var i;

        for (i = 0; i < accordions.length; i++) {
            accordions[i].addEventListener("click", function () {
                this.classList.toggle("active");
                var panel = this.nextElementSibling;
                if (panel.style.maxHeight) {
                    panel.style.maxHeight = null;
                } else {
                    panel.style.maxHeight = panel.scrollHeight + "px";
                }
            });
        }
    }
}