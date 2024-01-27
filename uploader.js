var fileType;
var parentId;
var uploadUrl;
var container;
var totalFiles = 0;
var uploadedCount = 0;
var fileInputField;
var uploadFileList = [];
function initUploadFromField(btn) {
    fileInputField = btn;
    uploadFileList = [];
    for (var i = 0; i < btn.files.length; i++) {
        uploadFileList[i] = btn.files[i];
    }

    parentId = $(btn).data("parent-id");
    uploadUrl = $(btn).data("upload-url");
    container = $(btn).data("container");
    fileType = $(btn).data("type");
    drawFileUploadTable()
}

function initUploadFromDrop(containerDiv, files) {
    uploadFileList = [];
    for (var i = 0; i < files.length; i++) {
        uploadFileList[i] = files[i];
    }
    parentId = $("#" + containerDiv).data("parent-id");
    uploadUrl = $("#" + containerDiv).data("upload-url");
    container = $("#" + containerDiv).data("container");
    fileType = $("#" + containerDiv).data("type");
    drawFileUploadTable()
}

function drawFileUploadTable() {
    totalFiles = uploadFileList.length;
    if (totalFiles === 0) {
        $("#" + container).html("");
        return false;
    }
    uploadedCount = 0;
    var fileList = "<div class='card shadow-sm m-2'>";
    for (var i = 0; i < uploadFileList.length; i++) {
        fileList += "<div class='p-2 border-bottom file-row'>";
        fileList += "<div class='d-flex'>"
        if (parentId === 0) {
            fileList += "<i class='far fa-trash-alt tt me-2 mt-1' style='cursor:pointer;' title='Delete File' onclick='removeFileFromInput(" + i + ")'></i>";
        }
        fileList += "<div class='flex-grow'>" + uploadFileList[i].name + "</div>";

        fileList += "</div>"
        if (parentId !== 0) {
            fileList += "<div class='progress' style='height: 3px;'>";
            fileList += "<div class='progress-bar' id='progress" + i + "' role='progressbar' style='width:0px' aria-valuemin='0' aria-valuemax='100'></div>";
            fileList += "</div>";
        }

        fileList += "</div>";
    }
    fileList += "</div>";

    $("#" + container).html(fileList);
    $("#" + container).slideDown();
    for (var i = 0; i < uploadFileList.length; i++) {
        uploadFile(uploadFileList[i], i);
    }
}

function uploadFile(file, fileCount) {

    if (parentId === 0)
        return false;
    var data = new FormData();
    data.append("officeKey", getOfficeKey());
    data.append("parentId", parentId);
    var url = baseUrl + "/" + uploadUrl;

    data.append("__RequestVerificationToken", $("INPUT[name=__RequestVerificationToken] ").first().val());
    data.append("file", file);

    $.ajax({
        type: "POST",
        url: url,
        contentType: false,
        processData: false,
        data: data,
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) {
                myXhr.upload.addEventListener('progress', function (event) {
                    if (event.lengthComputable) {
                        var percentComplete = (event.loaded / event.total) * 100;
                        $("#progress" + fileCount).css({ 'width': percentComplete + "%" });
                    }
                }, false);
            }
            return myXhr;
        },
        success: function (message) {
            uploadedCount++;
            if (totalFiles === uploadedCount) {
                setTimeout(function () {
                    $("#" + container).slideUp();
                    if (fileType === "claim-file") {
                        loadAttachments();
                    }
                    else if (fileType === "claim-email") {
                        loadEmails();
                    }
                    else if (fileType === "mail-template-file") {
                        loadMailTemplateFiles(parentId);
                    }
                    uploadFileList = [];
                    totalFiles = 0;

                }, 1000);
            }
        },
        error: function (message) {
            alert("Post failed " + message);
        }
    });
}

function fileDeleteCallBack() {
    if (responseData !== "") {
        var response = JSON.parse(responseData);
        if (response.success) {
            loadPartial("/files/" + getOfficeKey() + "/" + getClaimKey(), {}, "attachments-section-container");
        }
    }
}
function mailTemplateFileDeleteCallBack() {
    if (responseData !== "") {
        var response = JSON.parse(responseData);
        if (response.success) {
            loadMailTemplateFiles(response.key);
        }
    }
}
function loadMailTemplateFiles(templateId) {
    loadPartial("/mailtemplates/" + getOfficeKey() + "/" + templateId + "/files", {}, "attachments-section-container");
}
function removeFileFromInput(index) {
    uploadFileList.splice(index, 1);
    drawFileUploadTable();
}
function openImageViewer(img) {
    $("#image-preview").attr("src", img.src);
    if ($(img).data("created-by") !== "") {
        $("#image-info").html($(img).data("created-by") + " " + $(img).data("date"));
    }
    else {
        $("#image-info").html($(img).data("date"))
    }
    // openModal("image-viewer");
    $("#imageViewer").slideDown();
}
