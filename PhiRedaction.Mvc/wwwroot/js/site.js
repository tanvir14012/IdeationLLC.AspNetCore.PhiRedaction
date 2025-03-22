function readFileName(input, index) {
    if (input.files && input.files[0]) {
        $('#fileName' + index).html(input.files[0].name);
    }
}

function fixFormInputIndices() {
    let formGroups = $("#formElements").children(".form-group");
    formGroups.each(function (index) {
        let inputEl = $(formGroups[index]).find("input[type=file]");
        $(inputEl).attr("name", `Files[${index}].File`);
        $(inputEl).attr("id", `file${index}`);
        $(inputEl).attr("data-serial", index);
        $(inputEl).next("label").attr("id", `fileName${index}`)
        $(inputEl).next("label").attr("for", `file${index}`)
    });
}

$(document).ready(function () {
    $("input[type='file']").change(function (e) {
        var index = $(e.currentTarget).data("serial");
        readFileName(this, index);
    });

    $("#addFile").click(function () {
        var nextIndex = $("#formElements").children(".form-group").length;
        //name attribute is important
        var template = `<div class="form-group">
                    <div class="d-flex">
                        <div class="col-11">
                            <div class="custom-file">
                                <input id="file${nextIndex}" data-serial=${nextIndex} name="Files[${nextIndex}].File" class="form-control custom-file-input" type="file" />
                                <label id="fileName${nextIndex}" class="custom-file-label text-wrap" for="file${nextIndex}">Choose an excel file..</label>
                            </div>
                            <span class="text-danger"></span>
                        </div>
                        <div class="col-1">
                            <button type="button" name="deleteField" class="btn btn-sm btn-light mt-1 ms-2">
                                <i class="text-danger fas fa-trash"></i>
                            </button>
                        </div>
                    </div>
                </div>`;

        $("#formElements").append(template);

        $(`#file${nextIndex}`).change(function () {
            readFileName(this, nextIndex);
        });

        $("button[name=deleteField]").click(function (evt) {
            $($(evt.currentTarget).closest(".form-group")).remove();
        });
    });

    $("button[name=deleteField]").click(function (evt) {
        $($(evt.currentTarget).closest(".form-group")).remove();
        fixFormInputIndices();
    });

    $("#formSubmit").click(function (evt) {
        let formValid = true;
        let files = $("#formElements input[type=file]");
        let extensions = ["txt", "log", "rtf"];
        files.each(function (index) {
            if ($(files[index]).val() === "") {
                $(files[index]).closest(".col-11").find("span.text-danger").html("This field is required");
                formValid = false;
            }
            else if (extensions.indexOf($(files[index]).val().split(".").pop()) == -1) {
                $(files[index]).closest(".col-11").find("span.text-danger")
                    .html("Please choose an Excel file with any of the extension (.txt, .log, .rtf)");

                formValid = false;
            }
        });

        if (!formValid) {
            evt.preventDefault();
        }
       
    });
});