$(document).ready(function () {
    var sidebar_Icon = $("#hide_sidebar_icon");
    var delete_User_Label = $("#delete_user_label");
    var delete_User_Form = $("#delete_user");
    var change_lang_select = $("#lang_select");
    var logout_user_icon = $("#logout_icon");
    var logout_user_form = $("#logout_form");

    var logo_image = $("#logo_image");
    var sidebar_text_to_hide = $(".hide-on-change");
    var sidebar_icon = $(".sidebar-icon");
    var sidebar_logo_container = $(".sidebar-figure");
    var sidebar_headers = $(".sidebar-list-item-header");
    var sidebar_list_icons = $(".sidebar-li-icon");
    var sidebar_footer = $(".sidebar-footer");
    var sidebar_footer_icon = $(".icon-big");

    var close_change_lang_modal_button = $("#closeModalLangButton");
    var submit_change_lang_modal_button = $("#submitModalLangButton");

    var changeLang = function () {
        $("#langForm").submit();
        $("#changeLangModal").modal("toggle");
    }

    var showChangeLangModal = function (lang) {

        $("#lang").attr('value', lang)
        $("#changeLangModal").modal("toggle");
    }

    var init = function () {
        var currentLang = $("#lang_select option:selected").text();

        $(sidebar_Icon).on("click", function () {
            var aside = sidebar_Icon.parent().parent();
            $(aside).toggleClass('sidebar_size_change');

            $(logo_image).toggleClass('sidebar-figure-width-fix');
            $(sidebar_text_to_hide).toggleClass('d-none');
            $(sidebar_icon).toggleClass('icon-position-fix');
            $(sidebar_logo_container).toggleClass('sidebar_image_margin-fix');
            $(sidebar_headers).toggleClass('sidebar-list-item-header-fix'); 
            $(sidebar_list_icons).toggleClass('icon-size-fix'); 
            $(sidebar_footer).toggleClass('sidebar-footer-padding-fix');
            $(sidebar_footer_icon).toggleClass('col-lg-3');
        });

        $(delete_User_Label).on("click", function () {
            $(delete_User_Form).submit();
        });

        $(change_lang_select).on("change", function () {
            var lang = $("#lang_select option:selected").text();
            showChangeLangModal(lang);
        })

        $(logout_user_icon).on("click", function () {
            $(logout_user_form).submit();
        })

        $(close_change_lang_modal_button).on("click", function () {
            $("#changeLangModal").modal("toggle");
            $("#langSelect option:selected").text(currentLang)
        });

        $(submit_change_lang_modal_button).on("click", function () {
            changeLang();
        });
    }

    init();
});