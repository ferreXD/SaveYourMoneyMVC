var initGastosJs = function (file_content, file_name, file_type, selected_file, file_input, length, file_input_label) {
    function updateFileData(item) {
        var file = item.files[0];

        fs = new FileReader();
        fs.readAsArrayBuffer(file);

        fs.onload = function () {
            var base64 = convertToBase64(fs.result);

            $(file_content).val(base64);
            $(file_name).val(file.name);
            $(file_type).val(file.type);

            $(selected_file).text(formatString(file.name, length));
        };
    }

    function convertToBase64(buffer) {
        var binary = '';
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;

        for (var i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }

        return btoa(binary);
    }

    $(file_input).on("change", function () {
        updateFileData(this);
    });

    $(file_input_label).on("click", function () {
        $(file_input).click();
    });
}

function formatString(str, length) {
    return str.length > length ? str.substring(0, length) + "..." : str;
}